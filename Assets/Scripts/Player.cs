using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField]
    private float _speed = 5f;
    [SerializeField]
    private float _afterburner = 1.5f;
    [SerializeField]
    private float _boostFactor = 2;

    [SerializeField]
    private GameObject _laserPrefab;
    [SerializeField]
    private GameObject _tripleShotPrefab;
    [SerializeField]
    private GameObject _explosionPrefab;
    [SerializeField]
    private GameObject _blackHolePrefab;
    [SerializeField]
    private SpriteRenderer _shieldSprite;
    [SerializeField]
    private GameObject _leftEngine, _rightEngine;
    [SerializeField]
    private GameObject _thrusters;
    [SerializeField]
    private CameraShake _camera;
    [SerializeField]
    private AudioClip _laserSfx;
    [SerializeField]
    private float _boostDrainRate = .5f;
    [SerializeField]
    private float _boostRechargeRate = 1f;
    [SerializeField]
    private float _boostCooldown = 1f;

    private BoostBar _boostBar;
    private AudioSource _audio;
    private SpawnManager _spawnManager;
    private UIManager _uIManager;


    [SerializeField]
    private int _score;
    [SerializeField]
    private int _lives = 3;
    [SerializeField]
    private float _boostFuel = 100;
    [SerializeField]
    public float _boostMax = 100;
    [SerializeField]
    private float _fireRate = 0.15f;
    [SerializeField]
    private int _ammoCount = 15;

    [SerializeField]
    private bool _isBoosting = false;
    private float _canFire = -1f;
    private int _shieldStrength = 0;
    private bool _leftDamage = false;
    private bool _rightDamage = false;

    [SerializeField]
    private bool _tripleShotActive = false;
    [SerializeField]
    private bool _speedBoostActive = false;
    [SerializeField]
    private bool _shieldsActive = false;
    [SerializeField]
    private bool _blackHoleCannonActive = false;


    void Start()
    {
        transform.position = new Vector3(0, 0, 0);
        _spawnManager = GameObject.Find("Spawn_Manager").GetComponent<SpawnManager>();
        if (_spawnManager == null)
        {
            Debug.LogError("The Spawn Manager is NULL");
        }
        _uIManager = GameObject.Find("Canvas").GetComponent<UIManager>();
        if (_uIManager == null)
        {
            Debug.LogError("The UI Manager is NULL");
        }
        _audio = GetComponent<AudioSource>();
        if (_audio == null)
        {
            Debug.LogError("The Player Audio Source is NULL");
        }
        _boostBar = GameObject.Find("Boost_Bar").GetComponent<BoostBar>();
        if (_boostBar == null)
        {
            Debug.LogError("The Boost Bar is NULL");
        }
        _camera = GameObject.Find("Main Camera").GetComponent<CameraShake>();
        if (_camera == null)
        {
            Debug.LogError("The Main Camera is NULL");
        }
    }


    void Update()
    {
        CalculateMovement();

        if (Input.GetKeyDown(KeyCode.Space) && Time.time > _canFire)
        {
            FireLaser();
        }

        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            AfterburnersOn();
        }
        else if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            if (_isBoosting == true)
            {
                AfterburnersOff();
            }
            
        }

    }
    void CalculateMovement()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        Vector3 direction = new Vector3(horizontalInput, verticalInput, 0);

        transform.Translate(direction * _speed * Time.deltaTime);

        transform.position = new Vector3(transform.position.x, Mathf.Clamp(transform.position.y, -3.8f, 0), 0);

        if (transform.position.x > 11.3f)
        {
            transform.position = new Vector3(-11.3f, transform.position.y, 0);
        }
        else if (transform.position.x < -11.3f)
        {
            transform.position = new Vector3(11.3f, transform.position.y, 0);
        }
    }
    void FireLaser()
    {
        if (_ammoCount > 0)
        {
            _canFire = Time.time + _fireRate;

            if (_tripleShotActive == true)
            {
                Instantiate(_tripleShotPrefab, transform.position + new Vector3(0.06f, 0.75f, 0), Quaternion.identity);
                _ammoCount -= 3;
                if (_ammoCount < 0) { _ammoCount = 0; }
            }
            else if (_blackHoleCannonActive == true)
            {
                Instantiate(_blackHolePrefab, transform.position + new Vector3(0, 1, 0), Quaternion.identity);
            }
            else
            {
                Instantiate(_laserPrefab, transform.position + new Vector3(0, 1.05f, 0), Quaternion.identity);
                _ammoCount -= 1;
            }

            _audio.clip = _laserSfx;
            _audio.Play();
        }
        else
        {
            //notify the player there is no ammo.
            Debug.Log("Out of Ammunition!");
        }

        UIManager uimanager = _uIManager.transform.GetComponent<UIManager>();
        if (uimanager != null)
        {
            uimanager.Ammo_UI_Update(_ammoCount);
        }
    }
    public void Damage()
    {
        if (_shieldStrength == 3)
        {
            _shieldStrength--;
            _camera.ShakeCamera(.025f);
            _shieldSprite.color = new Color(1f, 1f, 1f, 0.66f);
            return;
        }
        else if (_shieldStrength == 2)
        {
            _shieldStrength--;
            _camera.ShakeCamera(.05f);
            _shieldSprite.color = new Color(1f, 1f, 1f, 0.33f);
            return;
        }
        else if (_shieldStrength == 1)
        {
            _shieldStrength--;
            _camera.ShakeCamera(.075f);
            _shieldSprite.color = new Color(1f, 1f, 1f, 0f);
            return;
        }

        _lives--;
        _camera.ShakeCamera(.1f);

        if (_lives == 2)
        {
            int hitLocation = Random.Range(0, 2);  // 0 is left engine, 1 is right engine
            if (hitLocation == 0)
            {
                _leftEngine.SetActive(true);
                _leftDamage = true;
            }
            else
            {
                _rightEngine.SetActive(true);
                _rightDamage = true;
            }
        }
        else if (_lives == 1)
        {
            if (_leftDamage == true)
            {
                _rightEngine.SetActive(true);
                _rightDamage = true;
            }
            else
            {
                _leftEngine.SetActive(true);
                _rightDamage = true;
            }
        }

        _uIManager.UpdateLives(_lives);


        if (_lives < 1)
        {
            _spawnManager.OnPlayerDeath();
            _speed = 1;
            GameObject explosion = Instantiate(_explosionPrefab, transform.position, Quaternion.identity);
            Destroy(this.gameObject, 0.5f);
        }
    }

    public void TripleShotGet()
    {
        _tripleShotActive = true;
        StartCoroutine(TripleShotPowerDownRoutine());
    }
    public void SpeedBoostGet()
    {
        _speedBoostActive = true;
        _speed *= _boostFactor;
        StartCoroutine(SpeedBoostPowerDownRoutine());
    }
    public void SheildsGet()
    {
        _shieldStrength = 3;
        Debug.Log("Shield Strength: " + _shieldStrength);
        _shieldSprite.color = new Color(1f, 1f, 1f, 1f);
        Debug.Log("Shield Alpha: " + _shieldSprite.color.a);
    }
    public void AmmoGet()
    {
        _ammoCount = 15;
        UIManager uimanager = _uIManager.transform.GetComponent<UIManager>();
        if (uimanager != null)
        {
            uimanager.Ammo_UI_Update(_ammoCount);
        }
    }
    public void RepairGet()
    {
        if (_lives < 3)
        {
            _lives++;
            _uIManager.UpdateLives(_lives);
        }
        if (_leftDamage == true)
        {
            _leftEngine.SetActive(false);
            _leftDamage = false;
        }
        else if (_rightDamage == true)
        {
            _rightEngine.SetActive(false);
            _rightDamage = false;
        }
    }
    public void BlackHoleCannonGet()
    {
        _blackHoleCannonActive = true;
        StartCoroutine(BlackHoleCannonPowerDownRoutine());

    }

    public void AfterburnersOn()
    {
        _isBoosting = true;
        _speed += _afterburner;

       _thrusters.transform.localScale = new Vector3(1, 1.5f, 1);
       _thrusters.transform.position = new Vector3(_thrusters.transform.position.x, _thrusters.transform.position.y - 0.4f, 0);

        
        StartCoroutine(BoostDrainRoutine());
    }

    public void AfterburnersOff()
    {
        _isBoosting = false;
        _speed += _afterburner * -1;

       _thrusters.transform.localScale = new Vector3(1, 1, 1);
       _thrusters.transform.position = new Vector3(_thrusters.transform.position.x, _thrusters.transform.position.y + 0.4f, 0);

        
        StartCoroutine(BoostRechargeRoutine());
    }
    IEnumerator BoostDrainRoutine()
    {
        StopCoroutine(BoostRechargeRoutine());
        while (_isBoosting == true)
        {
            if (_boostFuel > 0)
            {
                _boostFuel -= _boostDrainRate;
                yield return new WaitForSeconds(0.01f);
                _boostBar.UpdateBoostBar(_boostFuel);
            }
            else if (_boostFuel <= 0)
            {
                AfterburnersOff();
                yield break;
            }

        }

    }
    IEnumerator BoostRechargeRoutine()
    {
        StopCoroutine(BoostDrainRoutine());
        yield return new WaitForSeconds(_boostCooldown);
        while (_isBoosting == false)
        {
            if (_boostFuel < _boostMax)
            {
                _boostFuel += _boostRechargeRate;
                yield return new WaitForSeconds(0.02f);
                _boostBar.UpdateBoostBar(_boostFuel);
            }
            else
            {
                yield break;
            }

        }
    }
    public void AddScore(int points)
    {
        _score += points;
        _uIManager.UpdateScoreText(_score);
    }

    IEnumerator TripleShotPowerDownRoutine()
    {
        while (_tripleShotActive == true)
        {
            yield return new WaitForSeconds(5f);
            _tripleShotActive = false;
        }
    }
    IEnumerator SpeedBoostPowerDownRoutine()
    {
        while (_speedBoostActive == true)
        {
            yield return new WaitForSeconds(5f);
            _speed /= _boostFactor;
            _speedBoostActive = false;
        }
    }
    IEnumerator BlackHoleCannonPowerDownRoutine()
    {
        while (_blackHoleCannonActive == true)
        {
            yield return new WaitForSeconds(5f);
            _blackHoleCannonActive = false;
        }
    }
    
}

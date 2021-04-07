using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField]
    private float _speed = 5f;
    [SerializeField]
    private GameObject _laserPrefab;
    [SerializeField]
    private GameObject _tripleShotPrefab;
    [SerializeField]
    private float _fireRate = 0.15f;
    private float _canFire = -1f;
    [SerializeField]
    private int _lives = 3;
    private SpawnManager _spawnManager;
    private UIManager _uIManager;
    [SerializeField]
    private bool _tripleShotActive = false;
    [SerializeField]
    private bool _speedBoostActive = false;
    [SerializeField]
    private float _boostFactor = 2;
    [SerializeField]
    private bool _shieldsActive = false;
    [SerializeField]
    private GameObject _shields;
    [SerializeField]
    private int _score;
    [SerializeField]
    private GameObject _leftEngine, _rightEngine;
    private bool _leftDamage = false;
    private bool _rightDamage = false;
    //variable to store audio clip
    [SerializeField]
    private AudioClip _laserSfx;
    [SerializeField]
    private GameObject _explosionPrefab;
    private AudioSource _audio;

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
    }


    void Update()
    {
        CalculateMovement();

        if (Input.GetKeyDown(KeyCode.Space) && Time.time > _canFire)
        {
            FireLaser();
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
        _canFire = Time.time + _fireRate;
        if (_tripleShotActive == true)
        {
            Instantiate(_tripleShotPrefab, transform.position + new Vector3(0, 1.05f, 0), Quaternion.identity);
        }
        else
        {
            Instantiate(_laserPrefab, transform.position + new Vector3(0, 1.05f, 0), Quaternion.identity);
        }

        //play audio clip
        _audio.clip = _laserSfx;
        _audio.Play();
    }
    public void Damage()
    {
        if (_shieldsActive == true)
        {
            _shieldsActive = false;
            _shields.SetActive(false);
            return;
        }

        _lives--;
        //if lives is 2 
        //damage random engine
        //if lives is 1
        //damage other engine
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
        _shieldsActive = true;
        _shields.SetActive(true);
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
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Enemy : MonoBehaviour
{
    [SerializeField]
    private float _ySpeed = 4;
    private Player _player;
    [SerializeField]
    private GameObject _enemyLaserPrefab;
    [SerializeField]
    private GameObject _enemyRearLaserPrefab;
    [SerializeField]
    private GameObject _enemyBlackHolePrefab;
    [SerializeField]
    private GameObject _explosionPrefab;
    [SerializeField]
    private SpawnManager _spawnManager;
    private float _time;
    private float _amplitude = 200;
    private bool _isDead = false;
    [SerializeField]
    private int _enemyID; //0 - Basic, 1 - Weaver, 2 - Porter, 3 - Shielder, 4 - Rammer
    private bool _shieldsUp;
    private Vector3 _initialVector = Vector3.zero;
    private Vector3 _secondVector = Vector3.zero;
    private bool _initialMove;
    private bool _secondMove;
    [SerializeField]
    private bool _canRearFire = false;
    private float _fireCooldown = -1f;
    [SerializeField]
    private float _detectionRange = 7f;
    [SerializeField]
    private float _rammingSpeedMultiplier = 2;


    private void Start()
    {
        _spawnManager = GameObject.Find("Spawn_Manager").GetComponent<SpawnManager>();
        if (_spawnManager == null)
        {
            Debug.LogError("The Spawn Manager is NULL");
        }
        _player = GameObject.Find("Player").GetComponent<Player>();
        if (_player == null)
        {
            Debug.LogError("Player is NULL!");
        }
        int d10 = Random.Range(0, 10);
        switch (_enemyID)
        {
            case 0:
                if (d10 == 0)
                {
                    _canRearFire = true;
                }
                StartCoroutine(EnemyLaserFireRoutine());
                break;
            case 1:
                if (d10 == 0)
                {
                    _canRearFire = true;
                }
                StartCoroutine(EnemyLaserFireRoutine());
                break;
            case 2:
                StartCoroutine(PorterMovementRoutine());
                break;
            case 3:
                if (d10 == 0)
                {
                    _canRearFire = true;
                }
                _shieldsUp = true;
                StartCoroutine(EnemyLaserFireRoutine());
                StartCoroutine(ShielderMovementRoutine());
                break;
            case 4:
                //No fire routine for this enemy.
                break;
            default:
                StartCoroutine(EnemyLaserFireRoutine());
                break;
        }
    }
    void Update()
    {
        TrackTime();
        MoveMe();

        //excluding enemies that don't have conventional weapons from targeting powerups.
        if (_enemyID != 2 & _enemyID != 4)
        {
            LookForTargets();
        }
        
        if (_canRearFire == true)
        {
            RearFire();
        }
      
        if (transform.position.x > 10f)
        {
            transform.position = new Vector3(-10f, transform.position.y, 0);
        }
        else if (transform.position.x < -10f)
        {
            transform.position = new Vector3(10f, transform.position.y, 0);
        }
        if (transform.position.y <= -6f)
        {
            float randomX = Random.Range(-9.5f, 9.5f);
            transform.position = new Vector3(randomX, 8f, 0);
        }
        if (transform.position.y > 8f)
        {
            transform.position = new Vector3(transform.position.x, 8f, 0);
        }
        
    }

    private void TrackTime()
    {
        _time++;
    }

    private void MoveMe()
    {
        switch (_enemyID)
        {
            case 0: //Basic
                transform.Translate(Vector3.down * _ySpeed * Time.deltaTime);
                break;
            case 1: //Weaver
                float frequency = 0.025f;
                float xSpeed = (Mathf.Cos(_time * frequency) * _amplitude * frequency) * Time.deltaTime;
                transform.Translate(new Vector2(xSpeed, -_ySpeed * Time.deltaTime));
                break;
            case 2: //Porter
                    //porter movement can't be run on update. 
                    //This note is here to remind you not to put the movement coroutine here. 
                    //It has to be in Start with an "if id = 2"
                break;
            case 3: //Shielder
                if (_initialMove == true)
                {
                    transform.Translate(_initialVector * _ySpeed * Time.deltaTime);
                }
                if (_secondMove == true)
                {
                    transform.Translate(_secondVector * _ySpeed * Time.deltaTime);
                }
                break;
            case 4: //Rammer
                float distanceToPlayer = Vector3.Distance(transform.position, _player.transform.position);
                Rigidbody2D rigidbody = GetComponent<Rigidbody2D>();
                if (distanceToPlayer <= _detectionRange)
                {
                    Vector2 direction = (Vector2)_player.transform.position - rigidbody.position;
                    direction.Normalize();
                    float rotateAmount = Vector3.Cross(direction, transform.up).z;
                    rigidbody.angularVelocity = -rotateAmount * 500;
                    rigidbody.velocity = transform.up * _ySpeed * _rammingSpeedMultiplier;
                }
                else
                {
                    transform.rotation = Quaternion.Euler(0, 0, 180);
                    transform.Translate(Vector3.up * _ySpeed * Time.deltaTime);
                }
                break;
            default:
                Debug.Log("Cannot MoveMe - Unknown Movement Type");
                break;
        }
    }
    private void LookForTargets()
    {
        _spawnManager._powerUpList = _spawnManager._powerUpList.Where(e => e != null).ToList();
        int count = _spawnManager._powerUpList.Count;
        for (int i = 0; i < count; i++)
        {
            GameObject powerup = _spawnManager._powerUpList[i];
            float directionToTarget = Vector3.SignedAngle(powerup.transform.position - transform.position, transform.up, Vector3.forward);
            if (directionToTarget < -175f | directionToTarget > 175f)
            {
                if (_isDead == false && Time.time > _fireCooldown)
                {
                    Instantiate(_enemyLaserPrefab, transform.position + new Vector3(0, -0.5f, 0), Quaternion.identity);
                    _fireCooldown = Time.time + 2f;
                }
            }
            if (directionToTarget < 5f & directionToTarget > -5f)
            {
                
                if (_isDead == false && Time.time > _fireCooldown)
                {
                    Instantiate(_enemyRearLaserPrefab, transform.position + new Vector3(0, 1.2f, 0), Quaternion.identity);
                    _fireCooldown = Time.time + 2f;
                }
            }
        }
        //for each powerup in the powerups list
        //get the Vector3.SignedAngle to powerup.
        //if powerupAngle <5 && > -5 or <-175 && > 175
        //shoooooot
        //cooldown the shoot

    }
    private void RearFire()
    {
        float directionToPlayer = Vector3.SignedAngle(_player.transform.position - transform.position, transform.up, Vector3.forward);
        if (directionToPlayer < 10 & directionToPlayer > -10)
        {
            if (_isDead == false && Time.time > _fireCooldown)
            {
                Debug.Log("Player Target Locked: AFT CANNON FIRING!");
                Instantiate(_enemyRearLaserPrefab, transform.position + new Vector3(0, 1.2f, 0), Quaternion.identity);
                _fireCooldown = Time.time + 2f;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        
        if (other.tag == "Player")
        {
            if (_shieldsUp == true)
            {
                _shieldsUp = false;
                Destroy(transform.GetChild(0).gameObject); //top child should be shields
            }
            else
            {
                StopAllCoroutines();
                _player.Damage();
                _ySpeed = 0f;
                _amplitude = 0f;
                Instantiate(_explosionPrefab, transform.position, Quaternion.identity);
                _isDead = true;
                Destroy(this.gameObject, 0.5f);
            }
            

        }
        else if (other.tag == "Laser")
        {
            if (_shieldsUp == true)
            {
                _shieldsUp = false;
                Destroy(other.gameObject);
                Destroy(transform.GetChild(0).gameObject); //top child should be shields
            }
            else
            {
                StopAllCoroutines();
                Destroy(other.gameObject);
                int randomScore = Random.Range(5, 11);
                _player.AddScore(randomScore);
                _ySpeed = 0f;
                _amplitude = 0f;
                Instantiate(_explosionPrefab, transform.position, Quaternion.identity);
                _isDead = true;
                Destroy(GetComponent<Collider2D>());
                Destroy(this.gameObject, 0.5f);
            }

           
        }
    }
    IEnumerator EnemyLaserFireRoutine()
    {
        float randomTime = Random.Range(3, 7);
        while (_isDead == false)
        {
            yield return new WaitForSeconds(randomTime);
            if (_isDead == false)
            {
                Instantiate(_enemyLaserPrefab, transform.position + new Vector3(0, -0.5f, 0), Quaternion.identity);
            }
        }
    }
    IEnumerator PorterMovementRoutine()
    {
        yield return new WaitForSeconds(2);
        while (_isDead == false)
        {
            float randomX = Random.Range(-9.75f, 9.75f);
            float randomY = Random.Range(1f, 7f);
            float cooldown = Random.Range(1f, 2f);

            gameObject.GetComponent<ScaleUpScaleDown>().ScaleMe(false, true);
            yield return new WaitForSeconds(1f);
            transform.position = new Vector3(randomX, randomY, 0);

            gameObject.GetComponent<ScaleUpScaleDown>().ScaleMe(true, false);
            yield return new WaitForSeconds(1f);
            if (_isDead == false)
            {
                Instantiate(_enemyBlackHolePrefab, transform.position, Quaternion.identity);
            }
            yield return new WaitForSeconds(cooldown);
        }


    }
    IEnumerator ShielderMovementRoutine()
    {
        while (_isDead == false)
        {
            int randomV1 = Random.Range(0, 8);
            Debug.Log("randomV1 = " + randomV1);
            int randomV2 = Random.Range(0, 2);
            Debug.Log("randomV2 = " + randomV2);
            
            switch (randomV1)
            {
                case 0:
                    _initialVector = Vector3.up;
                    if (randomV2 == 0)
                    {
                        _secondVector = Vector3.left;
                    }
                    else
                    {
                        _secondVector = Vector3.right;
                    }
                    break;
                case 1:
                    _initialVector = new Vector3(1, 1, 0);
                    if (randomV2 == 0)
                    {
                        _secondVector = new Vector3(-1, 1, 0);
                    }
                    else
                    {
                        _secondVector = new Vector3(1, -1, 0);
                    }
                    break;
                case 2:
                    _initialVector = Vector3.right;
                    if (randomV2 == 0)
                    {
                        _secondVector = Vector3.up;
                    }
                    else
                    {
                        _secondVector = Vector3.down;
                    }
                    break;
                case 3:
                    _initialVector = new Vector3(1, -1, 0);
                    if (randomV2 == 0)
                    {
                        _secondVector = new Vector3(1, 1, 0);
                    }
                    else
                    {
                        _secondVector = new Vector3(-1, -1, 0);
                    }
                    break;
                case 4:
                    _initialVector = Vector3.down;
                    if (randomV2 == 0)
                    {
                        _secondVector = Vector3.left;
                    }
                    else
                    {
                        _secondVector = Vector3.right;
                    }
                    break;
                case 5:
                    _initialVector = new Vector3(-1, -1, 0);
                    if (randomV2 == 0)
                    {
                        _secondVector = new Vector3(-1, 1, 0);
                    }
                    else
                    {
                        _secondVector = new Vector3(1, -1, 0);
                    }
                    break;
                case 6:
                    _initialVector = Vector3.left;
                    if (randomV2 == 0)
                    {
                        _secondVector = Vector3.up;
                    }
                    else
                    {
                        _secondVector = Vector3.down;
                    }
                    break;
                case 7:
                    _initialVector = new Vector3(-1, 1, 0);
                    if (randomV2 == 0)
                    {
                        _secondVector = new Vector3(-1, -1, 0);
                    }
                    else
                    {
                        _secondVector = new Vector3(1, 1, 0);
                    }
                    break;
                default:
                    break;
            }
            _initialMove = true;
            yield return new WaitForSeconds(1f);
            _initialMove = false;
            _secondMove = true;
            yield return new WaitForSeconds(.5f);
            _secondMove = false;
            yield return new WaitForSeconds(.25f);

        }
    }
}
    

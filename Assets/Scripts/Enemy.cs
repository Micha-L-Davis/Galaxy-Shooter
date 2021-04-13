using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField]
    private float _ySpeed = 4;
    private Player _player;
    private Animator _animator;
    private AudioSource _audio;
    [SerializeField]
    private AudioClip _explosion_Sfx;
    [SerializeField]
    private GameObject _enemyLaserPrefab;
    private int _numberOfUpdates = 0;
    private float _time;
    private float _amplitude = 200;
    private bool _isDead = false;
    [SerializeField]
    private int _enemyID; //0 - Basic 1 - Weavers
    [SerializeField]
    private SpawnManager _spawnManager;

    private void Start()
    {
        _animator = gameObject.GetComponent<Animator>();
        if (_animator == null)
        {
            Debug.LogError("Animator is NULL!");
        }
        _player = GameObject.Find("Player").GetComponent<Player>();
        if (_player == null)
        {
            Debug.LogError("Player is NULL!");
        }
        _audio = GetComponent<AudioSource>();
        if(_audio == null)
        {
            Debug.LogError("The Enemy audio source is NULL");
        }
        StartCoroutine(EnemyLaserFireRoutine());
        _spawnManager = GameObject.Find("Spawn_Manager").GetComponent<SpawnManager>();
        if (_spawnManager == null)
        {
            Debug.LogError("Spawn Manager is NULL!");
        }
    }
    void Update()
    {
        TrackTime();
        MoveMe();
        if (transform.position.y <= -6f)
        {
            transform.position = new Vector3(0, 8f, 0);
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
            default:
                Debug.Log("Cannot MoveMe - Unknown Movement Type");
                break;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            StopCoroutine(EnemyLaserFireRoutine());
            _player.Damage();
            _ySpeed = 0f;
            _amplitude = 0f;
            _animator.SetTrigger("OnEnemyDeath");
            _audio.clip = _explosion_Sfx;
            _audio.Play();
            _isDead = true;
            Destroy(this.gameObject, 2.3f);

        }
        else if (other.tag == "Laser")
        {
            StopCoroutine(EnemyLaserFireRoutine());
            Destroy(other.gameObject);
            int randomScore = Random.Range(5, 11);
            _player.AddScore(randomScore);
            _ySpeed = 0f;
            _amplitude = 0f;
            _animator.SetTrigger("OnEnemyDeath");
            _audio.clip = _explosion_Sfx;
            _audio.Play();
            _isDead = true;
            Destroy(GetComponent<Collider2D>());
            Destroy(this.gameObject, 2.3f);
           
        }
    }
    IEnumerator EnemyLaserFireRoutine()
    {
        float randomTime = Random.Range(3, 7);
        while (_isDead == false)
        {
            yield return new WaitForSeconds(randomTime);
            Instantiate(_enemyLaserPrefab, transform.position + new Vector3(0, -0.5f, 0), Quaternion.identity);
        }
    }
}
    

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField]
    private float _ySpeed = 4;
    private float _xSpeed;
    private Player _player;
    private Animator _animator;
    private AudioSource _audio;
    [SerializeField]
    private AudioClip _explosion_Sfx;
    [SerializeField]
    private GameObject _enemyLaserPrefab;
    private int _numberOfUpdates = 0;
    private float _time;
    [SerializeField]
    private float _frequency = 2;
    [SerializeField]
    private float _amplitude = 10;


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
    }
    void Update()
    {
        _time ++;
        //float currentX = transform.position.x;
        //transform.Translate(Vector3.down * _speed * Time.deltaTime);
        //sinusoidal movement
        _xSpeed = (Mathf.Cos(_time * _frequency) * _amplitude * _frequency) * Time.deltaTime;
        transform.Translate(new Vector2(_xSpeed, (_ySpeed * Time.deltaTime) * -1));
        if (transform.position.y < -6.4f)
        {
            transform.position = new Vector3(Random.Range(-10, 10), 8, 0);
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
            Destroy(GetComponent<Collider2D>());
            Destroy(this.gameObject, 2.3f);
           
        }
    }
    IEnumerator EnemyLaserFireRoutine()
    {
        float randomTime = Random.Range(3, 7);
        while (true)
        {
            yield return new WaitForSeconds(randomTime);
            Instantiate(_enemyLaserPrefab, transform.position + new Vector3(0, -0.5f, 0), Quaternion.identity);
        }
    }
}
    

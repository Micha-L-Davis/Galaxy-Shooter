using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField]
    private float _ySpeed = 4;
    private Player _player;
    [SerializeField]
    private GameObject _enemyLaserPrefab;
    [SerializeField]
    private GameObject _enemyBlackHolePrefab;
    [SerializeField]
    private GameObject _explosionPrefab;
    private int _numberOfUpdates = 0;
    private float _time;
    private float _amplitude = 200;
    private bool _isDead = false;
    [SerializeField]
    private int _enemyID; //0 - Basic, 1 - Weaver, 2 - Porter

    private void Start()
    {
        _player = GameObject.Find("Player").GetComponent<Player>();
        if (_player == null)
        {
            Debug.LogError("Player is NULL!");
        }

        if (_enemyID != 2)
        {
            StartCoroutine(EnemyLaserFireRoutine());
        }

        if (_enemyID == 2)
        {
            StartCoroutine(PorterMovementRoutine());
        }

    }
    void Update()
    {
        TrackTime();
        MoveMe();
        if (transform.position.y <= -6f)
        {
            float randomX = Random.Range(-9.5f, 9.5f);
            transform.position = new Vector3(randomX, 8f, 0);
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
            default:
                Debug.Log("Cannot MoveMe - Unknown Movement Type");
                break;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            StopAllCoroutines();
            _player.Damage();
            _ySpeed = 0f;
            _amplitude = 0f;
            Instantiate(_explosionPrefab, transform.position, Quaternion.identity);
            _isDead = true;
            Destroy(this.gameObject, 0.5f);

        }
        else if (other.tag == "Laser")
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
}
    

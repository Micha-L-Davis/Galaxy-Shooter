using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField]
    private float _speed = 4;
    private Player _player;

    private void Start()
    {
        _player = GameObject.Find("Player").GetComponent<Player>();
        if (_player == null)
        {
            Debug.LogError("Player is NULL!");
        }
    }
    void Update()
    {

        transform.Translate(Vector3.down * _speed * Time.deltaTime);
        if (transform.position.y < -5.4f)
        {
            transform.position = new Vector3(Random.Range(-10, 10), 8, 0);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {            
            _player.Damage();
            Destroy(this.gameObject);
        }
        else if (other.tag == "Laser")
        {
            int randomScore = Random.Range(5, 11);
            _player.AddScore(randomScore);
            Destroy(other.gameObject);
            Destroy(this.gameObject);
        }
    }
}

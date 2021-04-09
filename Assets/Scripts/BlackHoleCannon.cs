using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlackHoleCannon : MonoBehaviour
{
    public GameObject _target;
    private Rigidbody2D _rigidbody;
    [SerializeField]
    private float _speed = 8f;
    [SerializeField]
    private float _rotationSpeed = 200f;

    void Start()
    {
        _target = GameObject.FindGameObjectWithTag("Enemy");
        if (_target == null)
        {
            transform.Translate(Vector3.back * _speed);
            Destroy(this.gameObject, 1);
        }
        _rigidbody = GetComponent<Rigidbody2D>();
        StartCoroutine(EvaporationRoutine());
    }

    private void Update()
    {
        if (transform.position.x > 11.3f)
        {
            transform.position = new Vector3(-11.3f, transform.position.y, 0);
        }
        else if (transform.position.x < -11.3f)
        {
            transform.position = new Vector3(11.3f, transform.position.y, 0);
        }

        if (transform.position.y > 8)
        {
            transform.position = new Vector3(transform.position.x, -5.4f, 0);
        }
        else if (transform.position.y < -5.4f)
        {
            transform.position = new Vector3(transform.position.x, 8, 0);
        }
        if (_target == null)
        {
            _target = GameObject.FindGameObjectWithTag("Enemy");
        }

    }

    void FixedUpdate()
    {
        if (_target != null)
        {
            Vector2 direction = (Vector2)_target.transform.position - _rigidbody.position;
            direction.Normalize();
            float rotateAmount = Vector3.Cross(direction, transform.up).z;
            _rigidbody.angularVelocity = -rotateAmount * _rotationSpeed;
            _rigidbody.velocity = transform.up * _speed;
        }
          
    }

    IEnumerator EvaporationRoutine()
    {
        while (true)
        {
            float randomTime = Random.Range(0.5f, 2);
            yield return new WaitForSeconds(randomTime);
            transform.Translate(Vector3.back * _speed);
            Destroy(this.gameObject, 1);
        }
        

    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField]
    private float _speed = 4;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //move down at 4 m/s
        transform.Translate(Vector3.down * _speed * Time.deltaTime);
        //if bottom of screen
        //respawn at top with a new random x position
        if (transform.position.y < -5.4f)
        {
            transform.position = new Vector3(Random.Range(-10, 10), 8, 0);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        //if other is player
        //damage player
        //destroy us
        if (other.tag == "Player")
        {
            Player player = other.transform.GetComponent<Player>();
            if (player != null)
            {
                player.Damage();
            }
            
            Destroy(this.gameObject);
        }
        else if (other.tag == "Laser")
        {
            Destroy(other.gameObject);
            Destroy(this.gameObject);
        }
        //if other is laser
        //destroy laser
        //destroy us
    }
}

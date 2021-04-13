using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Powerup : MonoBehaviour
{
    [SerializeField]
    private float _speed = 3.0f;
    //Powerup ID 0 - Triple Shot, 1 - Speed, 2 - Shield, 3 - Ammo, 4 - Repair
    [SerializeField]
    private int _powerupID;
    private AudioSource _audio;

    private void Start()
    {
        _audio = GameObject.Find("Powerup_SFX").GetComponent<AudioSource>();
        if (_audio == null)
        {
            Debug.LogError("Powerup Audio Source is Null");
        }
    }

    void Update()
    {
        transform.Translate(Vector3.down * _speed * Time.deltaTime);
        if (transform.position.y < -5.4f)
        {
            Destroy(this.gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            Player player = other.transform.GetComponent<Player>();
            if (player != null)
            {
                switch (_powerupID)
                {
                    case 0:
                        player.TripleShotGet();
                        break;
                    case 1:
                        player.SpeedBoostGet();
                        break;
                    case 2:
                        player.SheildsGet();
                        break;
                    case 3:
                        player.AmmoGet();
                        break;
                    case 4:
                        player.RepairGet();
                        break;
                    case 5:
                        player.BlackHoleCannonGet();
                        break;
                    case 6:
                        player.SlowNegaGet();
                        break;
                    default:
                        Debug.Log("Default Value");
                        break;
                }
                //play audio
                _audio.Play();
                Destroy(this.gameObject);
         
            }

        }
    }

}

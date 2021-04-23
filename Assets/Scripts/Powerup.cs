using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Powerup : MonoBehaviour
{
    [SerializeField]
    private float _speed = 3.0f;
    //Powerup ID 0 - Triple Shot, 1 - Speed, 2 - Shield, 3 - Ammo, 4 - Repair, 5 - Black Hole, 6 - Speed Debuff, 7 - EMP
    [SerializeField]
    private int _powerupID;
    private AudioSource _audio;
    private bool _tractored;
    private Vector3 _tractorBearing = Vector3.zero;

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
        if (_tractored == true)
        {
            transform.Translate(_tractorBearing * _speed * Time.deltaTime);
        }
        else
        {
            transform.Translate(Vector3.down * _speed * Time.deltaTime);
        }
        
        if (transform.position.y < -5.4f)
        {
            Destroy(this.gameObject);
        }
    }
    public void TractorBeam(float range, Vector3 bearing)
    {
        if (range <= 5f)
        {
            _tractored = true;
            _tractorBearing = bearing;
        }
        else
        {
            Debug.Log(this.gameObject.name + " is out of tractor range.");
        }
        //if range is less than 5
        //move toward bearing at speed * 2
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
                    case 7:
                        player.EMPGet();
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
        else if (other.tag == "Enemy_Laser")
        {
            Destroy(other.gameObject);
            Destroy(this.gameObject);
        }
    }

}

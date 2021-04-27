using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossSegmentDamage : MonoBehaviour
{
    [SerializeField]
    private int _bodyID; //0 = main body, 1 = stage2 left, 2 = stage2 right, 3 = stage1 left, 4 = stage1 right
    private int _shieldStrength = 3;
    private SpriteRenderer _shieldSprite;
    private int _lives = 3;
    [SerializeField]
    private GameObject _leftDamage;
    private bool _leftPlume;
    [SerializeField]
    private GameObject _rightDamage;
    private bool _rightPlume;
    [SerializeField]
    private GameObject _centerDamage;
    private bool _centerPlume;
    private BigBadBoss _bigBadBoss;
    [SerializeField]
    private GameObject _shieldPrefab;
    private GameObject _shieldInstance;

    private void Start()
    {
        _bigBadBoss = gameObject.transform.parent.transform.parent.GetComponent<BigBadBoss>();
        if (_bigBadBoss == null)
        {
            Debug.LogError("BigBadBoss is null");
        }
        _shieldInstance = Instantiate(_shieldPrefab, transform.position, Quaternion.identity);
        _shieldInstance.transform.parent = this.gameObject.transform;
        _shieldSprite = _shieldInstance.GetComponent<SpriteRenderer>();
        if (_shieldSprite == null)
        {
            Debug.LogError("Shield Sprite Renderer is null");
        }
        //configure shield size for each part by case
        switch (_bodyID)
        {
            case 0:
                _shieldInstance.transform.localScale = new Vector2(1.27f, 1.28f);
                _shieldInstance.transform.position = transform.position + new Vector3(0, .63f, 0);
                break;
            case 1:
                _shieldInstance.transform.position = transform.position + new Vector3(-3, -.3f, 0);
                break;
            case 2:
                _shieldInstance.transform.position = transform.position + new Vector3(3, -.3f, 0);
                break;
            case 3:
                _shieldInstance.transform.localScale = new Vector2(.75f, .97f);
                _shieldInstance.transform.position = transform.position + new Vector3(-4.82f, -.25f, 0);
                break;
            case 4:
                _shieldInstance.transform.localScale = new Vector2(.75f, .97f);
                _shieldInstance.transform.position = transform.position + new Vector3(4.82f, -.25f, 0);
                break;
            default:
                break;
        }
         
    }


    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Laser")
        {
            if (_shieldStrength == 3)
            {
                _shieldStrength--;
                _shieldSprite.color = new Color(1f, 1f, 1f, 0.66f);               
                return;
            }
            else if (_shieldStrength == 2)
            {
                _shieldStrength--;
                _shieldSprite.color = new Color(1f, 1f, 1f, 0.33f);
                return;
            }
            else if (_shieldStrength == 1)
            {
                _shieldStrength--;
                _shieldSprite.color = new Color(1f, 1f, 1f, 0f);
                return;
            }

            _lives--;


            if (_lives == 2)
            {
                _leftDamage.SetActive(true);
                _leftPlume = true;
            }
            else if (_lives == 1)
            {
                _rightDamage.SetActive(true);
                _rightPlume = true;
            }
            else if (_lives == 0)
            {
                _centerDamage.SetActive(true);
                _centerPlume = true;
                _bigBadBoss.OnSegmentDestruction(_bodyID);
            }

            Destroy(other.gameObject);

        }
    }
}

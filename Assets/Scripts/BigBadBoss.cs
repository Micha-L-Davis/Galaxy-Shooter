using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BigBadBoss : MonoBehaviour
{
    private int _stage = 1;
    [SerializeField]
    private GameObject _laserPrefab;
    [SerializeField]
    private GameObject _blackHolePrefab;
    [SerializeField]
    private GameObject _blastOrigin1L, _blastOrigin2L, _blastOrigin1R, _blastOrigin2R, _blastOriginML, _blastOriginMR;
    [SerializeField]
    private GameObject _blackHoleOriginL, _blackHoleOriginR, _blackHoleOriginM;
    [SerializeField]
    private GameObject _stageOneBodyLeft, _stageOneBodyRight, _stageTwoBodyLeft, _stageTwoBodyRight, _stageThreeBody;
    [SerializeField]
    private GameObject _explosionPrefab;
    private bool _isAlive = true;
    [SerializeField]
    private float _speed;
    [SerializeField]
    private float _volleyPause = 0.5f;
    private Vector3 _initialVector = Vector3.zero;
    private Vector3 _secondVector = Vector3.zero;
    private bool _initialMove;
    private bool _secondMove;
    public int _stageOneProgress = 0;
    public int _stageTwoProgress = 0;
    private Player _player;
    Vector3 _moveDirection = Vector3.left;



    void Start()
    {
        _player = GameObject.Find("Player").GetComponent<Player>();
        if (_player == null)
        {
            Debug.LogError("The Player is NULL");
        }

        BossChoreography();
    }


    void Update()
    {
        
        switch (_stage)
        {
            case 1:
                transform.position = new Vector3(Mathf.Clamp(transform.position.x, -4.5f, 4.5f), Mathf.Clamp(transform.position.y, 3, 6.25f), 0);
                if (_initialMove == true)
                {
                    transform.Translate(_initialVector * _speed * Time.deltaTime);
                }
                if (_secondMove == true)
                {
                    transform.Translate(_secondVector * _speed * Time.deltaTime);
                }
                break;
            case 2:
                _stageTwoBodyLeft.GetComponent<CapsuleCollider2D>().enabled = true;
                _stageTwoBodyRight.GetComponent<CapsuleCollider2D>().enabled = true;
                transform.position = new Vector3(Mathf.Clamp(transform.position.x, -6.3f, 6.3f), Mathf.Clamp(transform.position.y, 3, 6.25f), 0);

                if (transform.position.x <= -6.3)
                {
                    _moveDirection = Vector3.right;
                }
                else if (transform.position.x >= 6.3)
                {
                    _moveDirection = Vector3.left;
                }
                transform.Translate(_moveDirection * _speed * Time.deltaTime);
                break;
            case 3:
                _stageThreeBody.GetComponent<CapsuleCollider2D>().enabled = true;
                break;
            default:
                break;
        }
    }

    private void BossChoreography()
    {
        switch (_stage)
        {
            case 1:
                _speed = 1;
                StartCoroutine(Stage1MovementRoutine());
                StartCoroutine(FireRoutine(3));
                break;
            case 2:
                StopCoroutine(Stage1MovementRoutine());
                _speed = 3;
                _volleyPause = .25f;
                StartCoroutine(FireRoutine(2));
                break;
            case 3:
                StartCoroutine(EmergencyTeleportRoutine());
                StartCoroutine(PorterMovementRoutine());
                break;
            default:
                break;
        }
    }

    public void OnSegmentDestruction(int bodyID)
    {
        StopCoroutine(FireRoutine(10));
        if (bodyID == 3 || bodyID == 4)
        {
            _stageOneProgress++;
        }
        else if (bodyID == 1 || bodyID == 2)
        {
            _stageTwoProgress++;
        }
        if (_stageOneProgress == 2)
        {
            _stage = 2;
            _stageOneProgress = 0;
            Instantiate(_explosionPrefab, _stageOneBodyLeft.transform.position + new Vector3(-4, 0, 0), Quaternion.identity);
            Destroy(_stageOneBodyLeft.gameObject, 1);
            Instantiate(_explosionPrefab, _stageOneBodyRight.transform.position + new Vector3(4, 0, 0), Quaternion.identity);
            Destroy(_stageOneBodyRight.gameObject, 1);
            BossChoreography();
            return;
        }
        if (_stageTwoProgress == 2)
        {
            _stage = 3;
            Debug.Log("Entering stage three, cleaning up damaged components");
            _stageTwoProgress = 0;
            Instantiate(_explosionPrefab, _stageTwoBodyLeft.transform.position + new Vector3(-3, 0, 0), Quaternion.identity);
            Destroy(_stageTwoBodyLeft.gameObject, 1);
            Instantiate(_explosionPrefab, _stageTwoBodyRight.transform.position + new Vector3(3, 0, 0), Quaternion.identity);
            Destroy(_stageTwoBodyRight.gameObject, 1);
            BossChoreography();
            return;
        }       
        if (bodyID == 0)
        {
            _isAlive = false;
            Instantiate(_explosionPrefab, _stageThreeBody.transform.position, Quaternion.identity);
            Destroy(this.gameObject, 1f);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            _player.Damage();
        }
    }

    IEnumerator EmergencyTeleportRoutine()
    {
        float randomX = Random.Range(-9.75f, 9.75f);
        float randomY = Random.Range(3f, 6.5f);
        float cooldown = Random.Range(1f, 2f);

        gameObject.GetComponent<ScaleUpScaleDown>().ScaleMe(false, true);
        yield return new WaitForSeconds(1f);
        transform.position = new Vector3(randomX, randomY, 0);
        gameObject.GetComponent<ScaleUpScaleDown>().ScaleMe(true, false);
        yield return new WaitForSeconds(cooldown);
    }

    IEnumerator Stage1MovementRoutine()
    {
        while (_isAlive == true)
        {
            int randomV1 = Random.Range(0, 8);
            int randomV2 = Random.Range(0, 2);

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
            yield return new WaitForSeconds(.5f);
            _initialMove = false;
            _secondMove = true;
            yield return new WaitForSeconds(.25f);
            _secondMove = false;
            yield return new WaitForSeconds(.5f);

        }
    }

    IEnumerator PorterMovementRoutine()
    {
        yield return new WaitForSeconds(2);
        while (_isAlive == true)
        {
            float randomX = Random.Range(-9.75f, 9.75f);
            float randomY = Random.Range(1f, 6.5f);
            float cooldown = Random.Range(1f, 2f);

            gameObject.GetComponent<ScaleUpScaleDown>().ScaleMe(false, true);
            yield return new WaitForSeconds(1f);
            transform.position = new Vector3(randomX, randomY, 0);

            gameObject.GetComponent<ScaleUpScaleDown>().ScaleMe(true, false);
            yield return new WaitForSeconds(1f);
            if (_isAlive == true)
            {
                Instantiate(_blackHolePrefab, _blackHoleOriginM.transform.position, Quaternion.identity);
                yield return new WaitForSeconds(.25f);
                Instantiate(_blackHolePrefab, _blackHoleOriginM.transform.position, Quaternion.identity);
            }
            yield return new WaitForSeconds(cooldown);
        }


    }
    IEnumerator FireRoutine(int cycles)
    {

        for (int i = 0; i < cycles; i++)
        {
            yield return new WaitForSeconds(Random.Range(1, 4));
            if (_blastOrigin1L != null)
            {
                Instantiate(_laserPrefab, _blastOrigin1L.transform.position, Quaternion.identity);

            }
            
            if (_blastOrigin1R != null)
            {
                Instantiate(_laserPrefab, _blastOrigin1R.transform.position, Quaternion.identity);
                yield return new WaitForSeconds(_volleyPause);
            }
            if (_blastOrigin2L != null)
            {
                Instantiate(_laserPrefab, _blastOrigin2L.transform.position, Quaternion.identity);
            }
            if (_blastOrigin2R != null)
            {
                Instantiate(_laserPrefab, _blastOrigin2R.transform.position, Quaternion.identity);
                yield return new WaitForSeconds(_volleyPause);
            }
            Instantiate(_laserPrefab, _blastOriginML.transform.position, Quaternion.identity);
            Instantiate(_laserPrefab, _blastOriginMR.transform.position, Quaternion.identity);
            if (i == cycles - 1)
            {
                yield return new WaitForSeconds(_volleyPause);
                Instantiate(_blackHolePrefab, _blackHoleOriginL.transform.position, Quaternion.identity);
                i = 0;
            }
        }
    }
}

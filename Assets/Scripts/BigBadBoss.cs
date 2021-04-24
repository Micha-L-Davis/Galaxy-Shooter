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
    private SpriteRenderer _shieldsSprite1L, _shieldsSprite1R, _shieldsSprite2L, _shieldsSprite2R, _shieldsSpriteMain;
    [SerializeField]
    private GameObject _blastOrigin1L, _blastOrigin2L, _blastOrigin1R, _blastOrigin2R, _blastOriginMain;
    [SerializeField]
    private GameObject _blackHoleOriginL, _blackHoleOriginR, _blackHoleOriginM;
    private bool _isAlive;
    [SerializeField]
    private float _speed;


    void Start()
    {
        //start BossChoreographyRoutine
    }


    void Update()
    {
        //if stage one 
        // slow little knight move wiggles
        //else if stage two 
        // scan back and forth at top of screen
        //else if stage three
        // return
    }

    IEnumerator BossChoreographyRoutine()
    {
        while (_isAlive == true)
        {
            switch (_stage)
            {
                case 1:
                    //for i < 3
                        //wait for 1-3 seconds
                        //fire l&r origin 1
                        //wait half a sec
                        //fire l&r origin 2
                        //wait half a sec
                        //fire main origin
                    break;
                case 2:
                    //for i < 2
                        //wait for 1-2 seconds
                        //set speed to zero
                        //fire full volley
                        //reset speed
                    break;
                case 3:
                    //Start EmergencyTeleportRoutine
                    //Start PorterMovementRoutine
                    break;
                default:
                    break;
            }
        }

        yield break;
    }

    public void OnSegmentDestruction()
    {
        //if both segments in current stage are destroyed
        // increment stage
    }

    IEnumerator EmergencyTeleportRoutine()
    {
        float randomX = Random.Range(-9.75f, 9.75f);
        float randomY = Random.Range(1f, 7f);
        float cooldown = Random.Range(1f, 2f);

        gameObject.GetComponent<ScaleUpScaleDown>().ScaleMe(false, true);
        yield return new WaitForSeconds(1f);
        transform.position = new Vector3(randomX, randomY, 0);
        gameObject.GetComponent<ScaleUpScaleDown>().ScaleMe(true, false);
        yield return new WaitForSeconds(1f);
    }


}

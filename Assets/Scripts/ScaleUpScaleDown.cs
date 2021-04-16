using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScaleUpScaleDown : MonoBehaviour
{
    private Vector3 scaleDown = new Vector3(0, 1, 1);
    private Vector3 scaleUp = new Vector3(1, 1, 1);
    private bool _isScalingDown;
    private bool _isScalingUp;

    void Update()
    {

        if (_isScalingDown == true && _isScalingUp == false)
        {
            transform.localScale = Vector3.Lerp(transform.localScale, scaleDown, 50f * Time.deltaTime);
            Debug.Log("X Scale is " + transform.localScale.x);
            if (transform.localScale.x <= 0)
            {
                _isScalingDown = false;
            }
        }
    
        else if (_isScalingDown == false && _isScalingUp == true)
        {                
            transform.localScale = Vector3.Lerp(transform.localScale, scaleUp, 50f * Time.deltaTime);
            Debug.Log("X Scale is " + transform.localScale.x);
            if (transform.localScale.x >= 1)
            {
                _isScalingUp = false;
            }     
        } 
        else if (_isScalingDown == true && _isScalingUp == true)
        {
            Debug.LogError("Impossible Scaling task assigned.");
        }
    }
    public void ScaleMe(bool isScalingUp, bool isScalingDown)
    {
        _isScalingDown = isScalingDown;
        _isScalingUp = isScalingUp;
    }
}

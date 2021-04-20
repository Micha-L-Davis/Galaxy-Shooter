using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGScroll : MonoBehaviour
{
    [SerializeField]
    public float _speed = 0.1f;
   
    void Update()
    {
        Vector2 offset = new Vector2(0, Time.time * _speed);
        GetComponent<Renderer>().material.mainTextureOffset = offset;
    }
}

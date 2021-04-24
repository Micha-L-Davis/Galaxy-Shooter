using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EMPBurst : MonoBehaviour
{

    void Update()
    {
        transform.localScale = Vector3.Lerp(transform.localScale, transform.localScale + new Vector3(1, 1, 0), 10f * Time.deltaTime);
        if (transform.localScale.y >= 6)
        {
            Destroy(this.gameObject);
        }
        //scale prefab up until it reaches max size.
        //destroy prefab
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Enemy" || other.tag == "Enemy_Laser")
        {
            Destroy(other.gameObject);
        }
    }
}

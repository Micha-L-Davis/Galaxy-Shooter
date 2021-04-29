using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EMPBurst : MonoBehaviour
{
    [SerializeField]
    private GameObject _explosionPrefab;
    void Update()
    {
        transform.localScale = Vector3.Lerp(transform.localScale, transform.localScale + new Vector3(1, 1, 0), 10f * Time.deltaTime);
        if (transform.localScale.y >= 6)
        {
            Destroy(this.gameObject);
        }

    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Enemy")
        {
            Instantiate(_explosionPrefab, other.transform.position, Quaternion.identity);
            Destroy(other.gameObject);
        }
        else if (other.tag == "Enemy_Laser")
        {
            Destroy(other.gameObject);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    private Vector3 _startPosition;
    private Quaternion _startRotation;
    public float intensity;
    public float settle_rate;
    [SerializeField]
    public float _wiggle = .25f;

    void Update()
    {
        if (intensity > 0)
        {
            transform.position = _startPosition + Random.insideUnitSphere * intensity;
            transform.rotation = new Quaternion
                (
                    _startRotation.x + Random.Range(-intensity, intensity) * _wiggle, 
                    _startRotation.y + Random.Range(-intensity, intensity) * _wiggle, 
                    _startRotation.z + Random.Range(-intensity, intensity) * _wiggle, 
                    _startRotation.w + Random.Range(-intensity, intensity) * _wiggle
                );
            intensity -= settle_rate;
        }
    }
    public void ShakeCamera(float intensity)
    {
        _startPosition = transform.position;
        _startRotation = transform.rotation;
        settle_rate = 0.0025f;
        this.intensity = intensity;
    }
}

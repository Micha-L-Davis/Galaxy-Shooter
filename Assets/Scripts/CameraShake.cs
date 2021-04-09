using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    private Vector3 _startPosition;
    private Quaternion _startRotation;
    public float _intensity;
    public float _settle_rate;
    [SerializeField]
    public float _wiggle = .25f;

    void Update()
    {
        if (_intensity > 0)
        {
            transform.position = _startPosition + Random.insideUnitSphere * _intensity;
            transform.rotation = new Quaternion
                (
                    _startRotation.x + Random.Range(-_intensity, _intensity) * _wiggle, 
                    _startRotation.y + Random.Range(-_intensity, _intensity) * _wiggle, 
                    _startRotation.x + Random.Range(-_intensity, _intensity) * _wiggle, 
                    _startRotation.w + Random.Range(-_intensity, _intensity) * _wiggle
                );
            _intensity -= _settle_rate;
        }
    }
    public void ShakeCamera(float intensity)
    {
        _startPosition = transform.position;
        _startRotation = transform.rotation;
        _settle_rate = 0.0025f;
        _intensity = intensity;
    }
}

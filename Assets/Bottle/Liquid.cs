using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Liquid : MonoBehaviour
{
    [SerializeField] private Material _material;
    [SerializeField] private float _recovery = 10f;

    [SerializeField] private float _positionK = 0.1f;
    [SerializeField] private float _angularK = 0.1f;

    private Quaternion _prevRotation;
    private Vector3 _prevPosition;

    float _wobbleX, _wobbleZ;

    void Start()
    {
        _prevRotation = transform.rotation;
    }

    void Update()
    {
        _material.SetVector("_SurfaceOffset", transform.position);
        _material.SetFloat("_WobbleX", _wobbleX);
        _material.SetFloat("_WobbleZ", _wobbleZ);

        Vector3 angularVelocity = GetAngleVelocity(_prevRotation, transform.rotation);
        Vector3 velocity = (transform.position - _prevPosition) / Time.deltaTime;

        float wobbleXTarget = angularVelocity.z*_angularK + velocity.x*_positionK;
        float wobbleYTarget = angularVelocity.x*_angularK + velocity.z*_positionK;

        _wobbleX = Mathf.Lerp(_wobbleX, wobbleXTarget, Time.deltaTime*_recovery);
        _wobbleZ = Mathf.Lerp(_wobbleZ, wobbleYTarget, Time.deltaTime*_recovery);

        _prevPosition = transform.position;
        _prevRotation = transform.rotation;
    }

    private Vector3 GetAngleVelocity(Quaternion prev, Quaternion cur)
    {
        Quaternion delta = cur * Quaternion.Inverse(prev);

        delta.ToAngleAxis(out float angle, out Vector3 axis);
        angle *= Mathf.Deg2Rad;

        return axis * angle * (1.0f / Time.deltaTime);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowderTest : MonoBehaviour
{
    [SerializeField] private float _level = 0f;
    [SerializeField] private float _pour = 0f;

    [SerializeField] private float _pourScale = 1f;

    [SerializeField] private float _levelToElevation = 0.1f;
    [SerializeField] private float _pourToElevation = 0.1f;

    [SerializeField] private float _pourDecrease = 0.2f;

    void Update()
    {
        transform.position = (_level * _levelToElevation - _pour * _pourToElevation) * Vector3.up;
        transform.localScale = new Vector3(1f, 1f, 1f + _pourScale * _pour);

        // _pour = Lerping.Lerp(_pour, 0f, Lerping.Smooth.Slow, epsilon: 0.05f);

        _pour = Mathf.Max(0f, _pour - _pourDecrease*Time.deltaTime);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class ContainerContentVisualizer : MonoBehaviour
{
    [SerializeField] private Transform _content;

    [Header("Fill")]
    [SerializeField] private float _bottomElevation = 0f;
    [SerializeField] private float _topElevation = 0.1f;

    [SerializeField] private float _bottomScale = 1f;
    [SerializeField] private float _topScale = 1f;

    [SerializeField] private AnimationCurve _radiusCurve;

    [SerializeField] private float _debugRadius = 1f;

    [Header("Animation")]
    [SerializeField] private float _pourScale = 1f;
    [SerializeField] private float _pourToElevation = 0.1f;
    [SerializeField] private float _pourDecrease = 0.5f;
    [SerializeField] private float _deltaToPour = 0.1f;

    private float _amount, _pour;

    private Container _container;

    void Awake()
    {
        _container = GetComponent<Container>();

        _container.OnAmountChanged += AmountChanged;
    }

    void Update()
    {
        if (_container != null)
        {
            _amount = (float)_container.GetAmount() / _container.volume;
        }

        if (Mathf.Approximately(_amount, 0f))
        {
            _content.gameObject.SetActive(false);
            _content.localScale = Vector3.zero;
            _content.localPosition = Vector3.zero;
            return;
        }
        else _content.gameObject.SetActive(true);

        float amount = Mathf.Clamp01(_amount- _pour*_pourToElevation);

        float baseScale = Mathf.Lerp(_bottomScale, _topScale, amount);
        float scale = _radiusCurve.Evaluate(amount);
        float height = Mathf.Lerp(_bottomElevation, _topElevation, amount);

        _content.localPosition = Lerping.Lerp(_content.localPosition, Vector3.up * height, Lerping.Smooth.Slow);
        _content.localScale = Lerping.Lerp(_content.localScale, new Vector3(scale, baseScale + _pour * _pourScale, scale), Lerping.Smooth.Slow);

        _pour = Mathf.MoveTowards(_pour, 0f, _pourDecrease * Time.deltaTime);
    }

    private void AmountChanged(int delta)
    {
        _pour += delta * _deltaToPour;

        if(_pour < 0) _pour = 0;
    }

    void OnDrawGizmosSelected()
    {
        Handles.color = Color.blue;

        for (int i = 0; i < 10; i++)
        {
            float t = (i + 1) / 10f;

            float scale = _radiusCurve.Evaluate(t);
            float height = Mathf.Lerp(_bottomElevation, _topElevation, t);

            Handles.DrawWireArc(transform.position + _content.up * height, _content.up, _content.forward, 360, _debugRadius * scale);
        }
        // Handles.DrawWireArc(TopPlane.transform.position, Vector3.up, Vector3.forward, 360, TopRadius);
        // Handles.DrawWireArc(TopPlane.transform.position, Vector3.up, Vector3.forward, 360, ClampTopRadius);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class ContainerContentVisualizer : MonoBehaviour
{
    [SerializeField] private Transform _content;

    [SerializeField] private float _bottomY;
    [SerializeField] private float _topY;

    [SerializeField] private AnimationCurve _radiusCurve;

    [SerializeField][Range(0f, 1f)] private float _amount;

    [SerializeField] private float _debugRadius = 1f;

    private Container _container;

    void Awake()
    {
        _container = GetComponent<Container>();
    }
    
    void Update()
    {
        if(_container != null) 
        {
            _amount = (float) _container.GetAmount() / _container.volume;
        }

        _content.gameObject.SetActive(_amount > 0.01f);

        float scale = _radiusCurve.Evaluate(_amount);
        float height = Mathf.Lerp(_bottomY, _topY, _amount);

        _content.localPosition = Vector3.up * height;
        _content.localScale = new Vector3(scale, 1f, scale);
    }

    void OnDrawGizmosSelected()
    {
        Handles.color = Color.blue;

        for (int i = 0; i < 10; i++)
        {
            float t = (i + 1) / 10f;

            float scale = _radiusCurve.Evaluate(t);
            float height = Mathf.Lerp(_bottomY, _topY, t);

            Handles.DrawWireArc(transform.position + _content.up * height, _content.up, _content.forward, 360, _debugRadius * scale);
        }
        // Handles.DrawWireArc(TopPlane.transform.position, Vector3.up, Vector3.forward, 360, TopRadius);
        // Handles.DrawWireArc(TopPlane.transform.position, Vector3.up, Vector3.forward, 360, ClampTopRadius);
    }
}

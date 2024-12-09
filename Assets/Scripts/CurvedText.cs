using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

[ExecuteInEditMode]
public class CurvedText : MonoBehaviour
{
    [SerializeField] private float _radius = 1f;
    [SerializeField] private float _halfArc = 60f;
    [SerializeField] private float _halfWidth = 1f;

    private TMP_Text _text;

    [SerializeField] private float _angleOffset = -45f;

    void Awake()
    {
        _text = GetComponent<TMP_Text>();
    }

    void Start()
    {
        Redraw();
    }

    void Update()
    {
        if (Application.platform == RuntimePlatform.WindowsEditor) Redraw();
    }

    private void Redraw()
    {
        _text.ForceMeshUpdate();

        TMP_TextInfo info = _text.textInfo;
        int count = info.characterCount;

        if (count == 0) return;

        for (int i = 0; i < count; i++)
        {
            if (!info.characterInfo[i].isVisible) continue;

            int vertex = info.characterInfo[i].vertexIndex;
            int material = info.characterInfo[i].materialReferenceIndex;

            Vector3[] vertices = info.meshInfo[material].vertices;

            vertices[vertex] = RemapVertex(vertices[vertex]);
            vertices[vertex + 1] = RemapVertex(vertices[vertex + 1]);
            vertices[vertex + 2] = RemapVertex(vertices[vertex + 2]);
            vertices[vertex + 3] = RemapVertex(vertices[vertex + 3]);
        }

        _text.UpdateVertexData();
    }

    private Vector3 RemapVertex(Vector3 vert)
    {
        float t = vert.x / _halfWidth;
        float angle = (_halfArc * t + _angleOffset) * Mathf.Deg2Rad;

        return new Vector3(Mathf.Cos(angle) * _radius, vert.y, Mathf.Sin(angle) * _radius);
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.matrix = transform.localToWorldMatrix;

        float angleFrom = (-_halfArc + _angleOffset) * Mathf.Deg2Rad;
        float angleTo = (_halfArc + _angleOffset) * Mathf.Deg2Rad;

        Gizmos.DrawWireSphere(new Vector3(Mathf.Cos(angleFrom), 0f, Mathf.Sin(angleFrom)) * _radius, 0.1f);
        Gizmos.DrawWireSphere(new Vector3(Mathf.Cos(angleTo), 0f, Mathf.Sin(angleTo)) * _radius, 0.1f);
    }
}

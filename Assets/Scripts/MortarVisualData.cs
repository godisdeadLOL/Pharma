using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class MortarVisualData : MonoBehaviour
{
    [field: SerializeField] public Transform MortarBase { get; private set; }

    [field: SerializeField] public Transform PestlePivot { get; private set; }
    [field: SerializeField] public Transform PestleHandle { get; private set; }

    [field: SerializeField] public Collider InteractionPlane { get; private set; }

    [field: SerializeField] public Transform TopPlane { get; private set; }
    [field: SerializeField] public Transform BottomPlane { get; private set; }

    [field: SerializeField] public float PestleOffset { get; private set; } = 1f;

    [field: SerializeField] public float TopRadius { get; private set; } = 2f;
    [field: SerializeField] public float BottomRadius { get; private set; } = 0.8f;

    [field: SerializeField] public float ClampTopRadius { get; private set; } = 1.2f;

    void Start()
    {
        InteractionPlane.enabled = false;
    }

    void OnDrawGizmosSelected()
    {
        Handles.color = Color.red;
        Handles.DrawWireArc(BottomPlane.transform.position, Vector3.up, BottomPlane.forward, 360, BottomRadius);
        Handles.DrawWireArc(TopPlane.transform.position, Vector3.up, Vector3.forward, 360, TopRadius);
        Handles.DrawWireArc(TopPlane.transform.position, Vector3.up, Vector3.forward, 360, ClampTopRadius);
    }
}

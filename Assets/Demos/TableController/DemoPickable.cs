using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class DemoPickable : MonoBehaviour
{
    public float Height;
    public float Radius;

    public float Elevation = 0f;

    private Collider _collider;
    private Transform _modelTransform;

    void Awake()
    {
        GameObject colliderObject = new GameObject();
        _collider = colliderObject.AddComponent<SphereCollider>();
        ((SphereCollider)_collider).radius = Radius;

        colliderObject.transform.parent = transform;
        colliderObject.transform.localPosition = Vector3.zero;
        colliderObject.layer = LayerMask.NameToLayer("PickableCollider");
        colliderObject.name = "PickableCollider";


        if (Physics.Raycast(transform.position, Vector3.down, out RaycastHit hit, float.PositiveInfinity, LayerMask.GetMask("Surface")))
        {
            transform.position = hit.point;
        }

        _modelTransform = transform.Find("Model");
    }

    void Update()
    {
        _modelTransform.localPosition = Lerping.Lerp(_modelTransform.localPosition, Vector3.up * Elevation, Lerping.Smooth.Fast);
    }


    public void Pick()
    {
        _collider.enabled = false;
    }
    public void Drop()
    {
        _collider.enabled = true;
        Elevation = 0f;
    }


    void OnDrawGizmosSelected()
    {
        Handles.color = Color.red;

        if (Physics.Raycast(transform.position, Vector3.down, out RaycastHit hit, float.PositiveInfinity, LayerMask.GetMask("Surface")))
        {
            Handles.DrawLine(transform.position, hit.point);
            Handles.DrawWireDisc(hit.point, Vector3.up, Radius);
        }

        Handles.matrix = transform.localToWorldMatrix;

        Handles.DrawWireDisc(Vector3.zero, Vector3.up, Radius);
        Handles.DrawWireDisc(Vector3.up * Height, Vector3.up, Radius);
        Handles.DrawLine(Vector3.zero, Vector3.up * Height);
    }
}

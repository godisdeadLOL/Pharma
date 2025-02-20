using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickableHoldPivot : MonoBehaviour
{
    private Transform _holdingPivot;
    private Transform _lyingPivot;
    private Transform _model;

    void Awake()
    {
        _holdingPivot = transform.Find("holdingPivot");
        _lyingPivot = transform.Find("lyingPivot");
        _model = transform.Find("model");

        SetPivot(_lyingPivot);

        var pickable = GetComponentInParent<Pickable>();

        pickable.OnPick += () => SetPivot(_holdingPivot);
        pickable.OnDrop += () => SetPivot(_lyingPivot);
    }

    private void SetPivot(Transform pivot)
    {
        _model.parent = pivot;
        _model.SetLocalPositionAndRotation(Vector3.zero, Quaternion.identity);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaceWeighterAction : Action
{
    public override string Name => _moved ? "На место" : "Достать";

    [SerializeField] private Transform _target;
    private Vector3 _startPosition;
    private Quaternion _startRotation;
    private bool _moved = false;

    public override void Activate(GameObject pickable)
    {
        if (_moved)
        {
            pickable.transform.position = _startPosition;
            pickable.transform.rotation = _startRotation;

            _moved = false;
        }
        else
        {
            _startPosition = pickable.transform.position;
            _startRotation = pickable.transform.rotation;

            pickable.transform.position = _target.position;
            pickable.transform.rotation = _target.rotation;

            _moved = true;
        }
    }
}

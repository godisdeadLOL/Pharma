using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExamineController : MonoBehaviour
{
    [SerializeField]
    private Transform _pivot;

    private Vector3 _oldPosition;

    public GameObject pickable;

    private bool _tempKinematic;

    void Update()
    {
        if (InputHandler.GetMouseButtonDown(1)) StateManager._inst.ChangeState<TableController>();
    }

    void OnEnable()
    {
        Rigidbody _rb = pickable.GetComponent<Rigidbody>();
        _tempKinematic = _rb.isKinematic;
        _rb.isKinematic = true;

        _oldPosition = pickable.transform.position;
        pickable.transform.position = _pivot.position;

        CameraManager._inst.SetPivot(_pivot);
        CameraManager._inst.CanPickArea = false;
    }

    void OnDisable()
    {
        if (pickable != null)
        {
            Rigidbody _rb = pickable.GetComponent<Rigidbody>();
            _rb.isKinematic = _tempKinematic;

            pickable.transform.position = _oldPosition;
            pickable = null;
        }
    }
}

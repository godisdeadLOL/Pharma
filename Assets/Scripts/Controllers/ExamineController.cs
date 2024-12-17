using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExamineController : MonoBehaviour
{
    public Transform Target { get; set; }

    [SerializeField] private float _distance = 0.6f;

    private Vector3 _tempPosition;

    private float _rotationX;
    private float _rotationY;

    void Update()
    {
        if (InputHandler.GetMouseButtonDown(1)) StateManager._inst.ChangeState<TableController>();

        if (InputHandler.GetMouseButton(0))
        {
            _rotationY += Input.GetAxis("Mouse X") * Time.deltaTime * 500f;
            _rotationX += Input.GetAxis("Mouse Y") * Time.deltaTime * 500f;

            Target.rotation = Quaternion.Euler(_rotationX, _rotationY, 0f);
        }
    }

    void OnEnable()
    {
        if (Target == null) return;

        _tempPosition = Target.position;

        Target.position = Camera.main.transform.position + Camera.main.transform.forward * _distance;

        // Rigidbody _rb = pickable.GetComponent<Rigidbody>();
        // _tempKinematic = _rb.isKinematic;
        // _rb.isKinematic = true;

        // _oldPosition = pickable.transform.position;
        // pickable.transform.position = _pivot.position;

        // CameraManager._inst.SetPivot(_pivot);
        // CameraManager._inst.CanPickArea = false;
    }

    void OnDisable()
    {
        if (Target == null) return;

        Target.position = _tempPosition;
        Target.rotation = Quaternion.identity;

        _rotationX = 0f;
        _rotationY = 0f;

        // if (pickable != null)
        // {
        //     Rigidbody _rb = pickable.GetComponent<Rigidbody>();
        //     _rb.isKinematic = _tempKinematic;

        //     pickable.transform.position = _oldPosition;
        //     pickable = null;
        // }
    }
}

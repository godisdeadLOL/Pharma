using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransferController : MonoBehaviour
{

    public Container Source { get; set; }

    public Container Target {get; set;}

    [Header("Procedural Animation")]


    [SerializeField]
    private float _sideOffset;

    [SerializeField] private float _vertOffset;

    [SerializeField] private float _leanAngle;


    [Header("Transfer")]

    [SerializeField] private int _stepAmount = 5;

    [SerializeField] private float _stepTime = 0.2f;

    [SerializeField] private float _transferDelay = 0.4f;

    private float _timer;

    void Update()
    {
        if (InputHandler.GetMouseButtonDown(1))
        {
            StateManager._inst.ChangeState<TableController>();
            return;
        }

        // анимация
        Vector3 targetPosition = Target.transform.position + Vector3.right * _sideOffset;
        Quaternion targetRotation = Quaternion.identity;

        if (InputHandler.GetMouseButton(0))
        {
            targetPosition += Vector3.up * _vertOffset;
            targetRotation = Quaternion.AngleAxis(_leanAngle, Vector3.forward);
        }

        Source.transform.position = Lerping.Lerp(Source.transform.position, targetPosition, Lerping.Smooth.Fast);
        Source.transform.rotation = Lerping.Lerp(Source.transform.rotation, targetRotation, Lerping.Smooth.Fast);

        // transfer
        if (InputHandler.GetMouseButtonDown(0))
        {
            _timer = -_transferDelay;
        }

        if (InputHandler.GetMouseButton(0))
        {
            _timer += Time.deltaTime;

            if (_timer > _stepTime)
            {
                _timer -= _stepTime;

                Source.Transfer(Target, _stepAmount);
            }
        }
    }

    void OnEnable()
    {
        // CameraManager._inst.SetPivot(interactable.transform);
        // CameraManager._inst.CanPickArea = false;

        _timer = -_transferDelay;
    }

    void OnDisable()
    {
        if (Target != null)
        {
            Target.transform.rotation = Quaternion.identity;

            Target = null;
            Source = null;
        }
    }
}

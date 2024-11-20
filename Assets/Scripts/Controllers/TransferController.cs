using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransferController : MonoBehaviour
{
    // todo: рефакторинг в соответствии с изменениями interactable

    [HideInInspector]
    public GameObject interactable;

    [HideInInspector]
    public GameObject pickable;

    [Header("Procedural Animation")]


    [SerializeField]
    private float _sideOffset;

    [SerializeField]
    private float _vertOffset;

    [SerializeField]
    private float _leanAngle;

    [SerializeField]
    private float _positionLerp = 10f;

    [SerializeField]
    private float _rotationLerp = 10f;

    [Header("Transfer")]

    [SerializeField]
    private int _stepAmount = 5;

    [SerializeField]
    private float _stepTime = 0.2f;

    [SerializeField]
    private float _transferDelay = 0.4f;

    private float _timer;

    private Container _containerFrom, _containerTo;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            StateManager._inst.ChangeState<TableController>();
            return;
        }

        // анимация
        Vector3 targetPosition = interactable.transform.position + Vector3.right * _sideOffset;
        Quaternion targetRotation = Quaternion.identity;

        if (Input.GetMouseButton(0))
        {
            targetPosition += Vector3.up * _vertOffset;
            targetRotation = Quaternion.AngleAxis(_leanAngle, Vector3.forward);
        }

        pickable.transform.position = Vector3.Lerp(pickable.transform.position, targetPosition, Time.deltaTime * _positionLerp);
        pickable.transform.rotation = Quaternion.Lerp(pickable.transform.rotation, targetRotation, Time.deltaTime * _rotationLerp);

        // transfer
        if (Input.GetMouseButtonDown(0))
        {
            _timer = -_transferDelay;
        }

        if (Input.GetMouseButton(0))
        {
            _timer += Time.deltaTime;

            if(_timer > _stepTime)
            {
                _timer -= _stepTime;

                _containerFrom.Transfer(_containerTo, _stepAmount);
            }
        }
    }

    void OnEnable()
    {
        CameraManager._inst.SetPivot(interactable.transform);
        CameraManager._inst.CanPickArea = false;

        _timer = -_transferDelay;

        _containerFrom = pickable.GetComponent<Container>();
        _containerTo = interactable.GetComponent<Container>();
    }

    void OnDisable()
    {
        if (pickable != null)
        {
            pickable.transform.rotation = Quaternion.identity;

            pickable = null;
            interactable = null;
        }
    }
}

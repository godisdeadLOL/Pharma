using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TableController : MonoBehaviour
{
    public float ElevationOffset { get; set; }

    [SerializeField]
    private LayerMask _tableLayer;

    [SerializeField]
    private LayerMask _pickableLayer;

    [SerializeField]
    private float _elevate = 0.5f;

    [SerializeField]
    private float _positionLerp = 10f;

    private Rigidbody _picked = null;
    private Interactable _interactable = null;

    private Vector3 _target;

    void Update()
    {
        float interactionDistance = CameraManager._inst.CurrentArea.InteractionDistance;

        // поднятие/опускание предмета
        if (InputHandler.GetMouseButtonDown(0) && _picked == null)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out RaycastHit hit, interactionDistance, _pickableLayer))
            {
                _picked = hit.rigidbody;
                _picked.isKinematic = true;

                Pick(_picked.gameObject);
            }

        }

        if (InputHandler.GetMouseButtonDown(1) && _picked != null)
        {
            Drop(_picked.gameObject);

            _picked.isKinematic = false;
            _picked = null;
        }

        // взаимодействия

        if (_interactable != null)
        {
            if (InputHandler.GetMouseButtonDown(0))
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

                // взаимодействие с другим предметом
                if (Physics.Raycast(ray, out RaycastHit hit, interactionDistance, _pickableLayer))
                {
                    if (hit.rigidbody.gameObject != _picked.gameObject) _interactable.Interact(hit.rigidbody.gameObject);
                }
                else // самовзаимодействие
                {
                    _interactable.Hold();
                }
            }
            else if (InputHandler.GetMouseButtonUp(0))
            {
                _interactable.Release();
            }
        }


        // движение предмета за курсором
        if (_picked != null)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out RaycastHit hit, interactionDistance, _tableLayer))
                _target = hit.point + Vector3.up * (_elevate+ElevationOffset);

            _picked.MovePosition(Vector3.Lerp(_picked.position, _target, Time.fixedDeltaTime * _positionLerp));
        }
    }

    void FixedUpdate()
    {
        if (_picked != null)
        {
            _picked.MovePosition(Vector3.Lerp(_picked.position, _target, Time.fixedDeltaTime * _positionLerp));
        }
    }

    void OnEnable()
    {
        CameraManager._inst.ResetPivot();
        CameraManager._inst.CanPickArea = _picked == null;

        if (_picked != null) Pick(_picked.gameObject);
    }
    void OnDisable()
    {
        if (_picked != null) Drop(_picked.gameObject);
    }

    private void Pick(GameObject pickable)
    {
        CameraManager._inst.CanPickArea = false;

        Actions actions = pickable.GetComponent<Actions>();

        if (actions != null)
            ActionManager._inst.Display(pickable, actions.profile);

        _interactable = pickable.GetComponent<Interactable>();
    }

    private void Drop(GameObject pickable)
    {
        CameraManager._inst.CanPickArea = true;

        ActionManager._inst.Hide();

        _interactable = null;
    }
}

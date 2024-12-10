using UnityEngine;

public class TableController : MonoBehaviour
{

    [SerializeField] private float _elevation = 0.1f;

    private LayerMask _interactableMask, _pickableColliderMask, _tableMask;

    private Pickable _pickable;
    private Vector3 _targetPosition;
    private Vector3 _realTargetPosition;

    void Awake()
    {
        _interactableMask = LayerMask.GetMask("Pickable", "Button", "Extra");
        _pickableColliderMask = LayerMask.GetMask("PickableCollider");
        _tableMask = LayerMask.GetMask("Surface");
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            StateManager._inst.GetState<MovementController>().ExitTarget();
            return;
        }

        if (_pickable == null) return;

        // 
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        float interactionDistance = InteractionManager.Inst.InteractionDistance;

        if (Physics.Raycast(ray, out RaycastHit hit, interactionDistance, _tableMask))
        {
            _realTargetPosition = hit.point;

            // bool up = Mathf.Approximately(Vector3.Dot(hit.normal, Vector3.up), 0f);

            float radius = _pickable.Collider.radius * _pickable.Collider.transform.localScale.x;

            Collider[] buffer = new Collider[1];
            if (Physics.OverlapSphereNonAlloc(hit.point, radius, buffer, _pickableColliderMask) == 0)
            {
                _targetPosition = hit.point;
            }
        }

        //
        _pickable.transform.position = Lerping.Lerp(
            _pickable.transform.position,
            Vector3.Lerp(_targetPosition, _realTargetPosition, 0.1f) + Vector3.up * _elevation,
            Lerping.Smooth.Fast
        );

        //
        if (InputHandler.GetMouseButtonDown(1)) Drop();
    }

    void OnEnable()
    {
        InteractionManager.Inst.PushLayer(InteractionLayer.Pickable);
        InteractionManager.Inst.PushLayer(InteractionLayer.Button);
        InteractionManager.Inst.PushLayer(InteractionLayer.Extra);

        InteractionManager.Inst.RaycastMask = _interactableMask;

        InteractionManager.Inst.OnInteractPrimary += OnInteractPrimary;
    }
    void OnDisable()
    {
        InteractionManager.Inst.ClearLayers();

        InteractionManager.Inst.OnInteractPrimary -= OnInteractPrimary;
    }

    private void OnInteractPrimary(Interactable interactable)
    {
        if (interactable.Layer == InteractionLayer.Pickable)
        {

            if (_pickable == null)
            {
                Pickable pickable = interactable.GetComponent<Pickable>();
                Pick(pickable);
            }
            else
            {
                Pickable source = _pickable;
                Pickable target = interactable.GetComponent<Pickable>();

                if(source == target) return;

                source.OnInteract?.Invoke(target);
                target.OnInteracted?.Invoke(source);
            }
        }
    }

    private void Pick(Pickable pickable)
    {
        _pickable = pickable;
        _targetPosition = pickable.transform.position;
        pickable.OnPick?.Invoke();


        InteractionManager.Inst.BlockSecondary = true;
    }

    private void Drop()
    {
        _pickable.OnDrop?.Invoke();
        _pickable.transform.position = _targetPosition;
        _pickable = null;

        InteractionManager.Inst.BlockSecondary = false;
    }
}

using UnityEngine;


public class TableController : MonoBehaviour
{
    public Transform Anchors { get; set; } = null;

    [SerializeField] private float _elevation = 0.1f;

    private LayerMask _interactableMask, _pickableColliderMask, _tableMask;

    public Vector3 TargetPosition { get; private set; }
    public Transform TargetAnchor { get; private set; }

    private Pickable _pickable;
    private Vector3 _realTargetPosition;

    private Transform GetCurrentAnchor(Ray ray, bool active = true)
    {
        if(Anchors == null) return null;

        Transform closest = null;
        float curDist = float.PositiveInfinity;

        foreach (Transform anchor in Anchors)
        {
            if (active && !anchor.gameObject.activeSelf) continue;

            float dist = Utils.DistanceToRay(anchor.position, ray);

            if (dist < curDist)
            {
                curDist = dist;
                closest = anchor;
            }
        }

        return closest;
    }

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

        // 
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        float interactionDistance = InteractionManager.Inst.InteractionDistance;

        if (Anchors != null)
        {
            Transform anchor = GetCurrentAnchor(ray);

            _realTargetPosition = anchor.position;
            TargetPosition = anchor.position;
            TargetAnchor = anchor;
        }
        else if (Physics.Raycast(ray, out RaycastHit hit, interactionDistance, _tableMask))
        {
            _realTargetPosition = hit.point;


            float radius = _pickable ? (_pickable.Collider.radius * _pickable.Collider.transform.localScale.x) : 0.1f;

            Collider[] buffer = new Collider[1];
            if (Physics.OverlapSphereNonAlloc(hit.point, radius, buffer, _pickableColliderMask) == 0)
            {
                TargetPosition = hit.point;
            }
        }

        if (_pickable == null) return;

        //
        _pickable.transform.position = Lerping.Lerp(
            _pickable.transform.position,
            Vector3.Lerp(TargetPosition, _realTargetPosition, 0.1f) + Vector3.up * _elevation,
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

        Anchors = null;
        TargetAnchor = null;
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

                if (source == target) return;

                source.OnInteract?.Invoke(target);
                target.OnInteracted?.Invoke(source);
            }
        }
    }

    private void Pick(Pickable pickable)
    {
        _pickable = pickable;
        TargetPosition = pickable.transform.position;
        pickable.OnPick?.Invoke();


        InteractionManager.Inst.BlockSecondary = true;


        Transform anchor = GetCurrentAnchor(Camera.main.ScreenPointToRay(Input.mousePosition), false);
        if(anchor != null) anchor.gameObject.SetActive(true);
    }

    private void Drop()
    {
        _pickable.OnDrop?.Invoke();
        _pickable.transform.position = TargetPosition;
        _pickable = null;

        InteractionManager.Inst.BlockSecondary = false;

        if (TargetAnchor != null) TargetAnchor.gameObject.SetActive(false);
    }
}

using System;
using UnityEngine;


public class TableController : MonoBehaviour
{
    public Transform Anchors { get; set; } = null;

    [SerializeField] private float _elevation = 0.1f;

    private LayerMask _interactableMask, _pickableColliderMask, _surfaceMask;

    public Vector3 TargetPosition { get; private set; }
    public Transform TargetAnchor { get; private set; }

    private Pickable _pickable;
    private Vector3 _realTargetPosition;
    private float _currentElevation;

    private Pickable _pickableReleased;
    private Vector3 _releaseTarget;



    private Transform GetCurrentAnchor(Ray ray, bool active = true)
    {
        if (Anchors == null) return null;

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
        _surfaceMask = LayerMask.GetMask("Surface");
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            StateManager._inst.GetState<MovementController>().ExitTarget();
            return;
        }

        // зафорсить конец анимации
        if (Input.anyKeyDown)
        {
            if (_pickableReleased != null)
            {
                _pickableReleased.transform.position = _releaseTarget;
                _pickableReleased = null;
            }
        }

        // release
        if (_pickableReleased != null)
        {
            _pickableReleased.transform.position = Lerping.Lerp(_pickableReleased.transform.position, _releaseTarget, Lerping.Smooth.Fast);
        }


        if (_pickable == null) return;

        _pickable.transform.position = Lerping.Lerp(
            _pickable.transform.position,
            _realTargetPosition + Vector3.up * _currentElevation,
            Lerping.Smooth.Fast
        );


        if (InputHandler.GetMouseButtonDown(1)) Drop();
    }

    void FixedUpdate()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        float interactionDistance = InteractionManager.Inst.InteractionDistance;

        _currentElevation = _elevation;

        if (Anchors != null)
        {
            Transform anchor = GetCurrentAnchor(ray);

            _realTargetPosition = anchor.position;
            TargetPosition = anchor.position;
            TargetAnchor = anchor;
        }
        else if (SurfaceManager.Instance.Raycast(ray, out var point, out var surface, maxDistance: interactionDistance))
        {
            // print($"{Time.time}: {point}");

            _realTargetPosition = point;

            float radius = _pickable ? _pickable.Radius : 0.05f;


            TargetPosition = Vector3.MoveTowards(TargetPosition, _realTargetPosition, 10f * Time.fixedDeltaTime);


            Collider[] buffer = new Collider[1];
            if (Physics.OverlapSphereNonAlloc(point, radius, buffer, _pickableColliderMask) > 0)
            {
                Collider collider = buffer[0];
                Pickable pickable = collider.transform.GetComponentInParent<Pickable>();

                // if(_pickable != null && pickable.transform.IsChildOf(_pickable.transform)) return;



                _currentElevation += pickable.Height / 2f;


                Vector3 colliderCenter = collider.transform.position;
                colliderCenter.y = 0f;

                Vector3 pointCenter = TargetPosition;
                pointCenter.y = 0f;

                Vector3 dir = pointCenter - colliderCenter;
                float offset = dir.magnitude;
                dir.Normalize();

                offset = Mathf.Max(0f, -offset + radius + pickable.Radius);
                TargetPosition += dir * offset;
            }
            // else
            // {
            //     TargetPosition = point;
            // }
        }
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

        // Anchors = null;
        TargetAnchor = null;

        // if(_pickable != null) Drop();
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
        // pickable.OnPick?.Invoke();
        _pickable.Pick();


        InteractionManager.Inst.BlockSecondary = true;


        Transform anchor = GetCurrentAnchor(Camera.main.ScreenPointToRay(Input.mousePosition), false);
        if (anchor != null) anchor.gameObject.SetActive(true);
    }

    private void Drop()
    {
        if (SurfaceManager.Instance.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out var point, out var surface))
        {
            _pickable.SetSurface(surface);
        }

        _releaseTarget = TargetPosition;
        _pickableReleased = _pickable;

        // _pickable.OnDrop?.Invoke();
        _pickable.Drop();
        // _pickable.transform.position = TargetPosition;
        _pickable = null;


        InteractionManager.Inst.BlockSecondary = false;

        if (TargetAnchor != null) TargetAnchor.gameObject.SetActive(false);



        // SurfaceManager.Instance.Raycast()
    }

    public void ResetAnchors()
    {
        Anchors = null;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;

        Gizmos.DrawWireSphere(TargetPosition, 0.05f);
    }
}

using UnityEditor;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Interactable))]
public class Pickable : MonoBehaviour
{
    public UnityAction OnPick;
    public UnityAction OnDrop;

    public UnityAction<Pickable> OnInteract;
    public UnityAction<Pickable> OnInteracted;

    // public UnityAction<Pickable> OnInteractStart;
    // public UnityAction<Pickable> OnInteractHold;
    // public UnityAction<Pickable> OnInteractEnd;

    public Surface Surface { get; private set; }

    [field: SerializeField] public float Mass { get; set; } = 0.1f;
    [field: SerializeField] public float Height { get; private set; } = 0.2f;
    [field: SerializeField] public float Radius { get; private set; } = 0.3f;

    public SphereCollider Collider { get; private set; }
    public bool Picked { get; private set; }

    [field: SerializeField] public bool SkipCollider { get; private set; } = false;
    [SerializeField] private bool _skipLand = false;

    void Awake()
    {
        // Collider = transform.Find("PickableCollider").GetComponent<SphereCollider>();

        // OnPick += () => Pick();
        // OnDrop += () => Drop();

        if (!SkipCollider) GenerateCollider();
    }

    void Start()
    {
        if (!_skipLand && SurfaceManager.Instance.Raycast(new Ray(transform.position, Vector3.down), out var point, out var surface))
        {
            transform.position = point;
            SetSurface(surface);
        }
    }

    public void SetSurface(Surface surface)
    {
        if (Surface != null) Surface.Deattatch(this);

        transform.SetParent(surface.transform, true);
        Surface = surface;

        Surface.Attatch(this);
    }

    public void Pick()
    {
        Picked = true;
        if (!SkipCollider) Collider.gameObject.SetActive(false);

        OnPick?.Invoke();
    }

    public void Drop()
    {
        Picked = false;
        if (!SkipCollider) Collider.gameObject.SetActive(true);

        OnDrop?.Invoke();
    }

    private void GenerateCollider()
    {
        var colliderObject = transform.Find("PickableCollider")?.gameObject;

        if (colliderObject != null)
        {
            Collider = colliderObject.GetComponent<SphereCollider>();
            return;
        }

        colliderObject = new();
        Collider = colliderObject.AddComponent<SphereCollider>();
        ((SphereCollider)Collider).radius = Radius;

        colliderObject.transform.parent = transform;
        colliderObject.transform.localPosition = Vector3.zero;
        colliderObject.layer = LayerMask.NameToLayer("PickableCollider");
        colliderObject.name = "PickableCollider";
    }

    void OnDrawGizmosSelected()
    {
        // Handles.color = Color.red;

        // if (SurfaceManager.Instance != null && SurfaceManager.Instance.Raycast(new Ray(transform.position, Vector3.down), out var point, out var surface))
        // {
        //     Handles.DrawLine(transform.position, point);
        //     Handles.DrawWireDisc(point, Vector3.up, Radius);
        // }

        // Handles.matrix = transform.localToWorldMatrix;

        // Handles.DrawWireDisc(Vector3.zero, Vector3.up, Radius);
        // Handles.DrawWireDisc(Vector3.up * Height, Vector3.up, Radius);
        // Handles.DrawLine(Vector3.zero, Vector3.up * Height);
    }
}

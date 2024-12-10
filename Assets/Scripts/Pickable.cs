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

    [field: SerializeField] public float Mass { get; set; } = 0.1f;

    public SphereCollider Collider { get; private set; }
    public bool Picked { get; private set; }

    void Awake()
    {
        Collider = transform.Find("PickableCollider").GetComponent<SphereCollider>();

        OnPick += () => Pick();
        OnDrop += () => Drop();
    }

    private void Pick()
    {
        Picked = true;
        Collider.gameObject.SetActive(false);
    }

    private void Drop()
    {
        Picked = false;
        Collider.gameObject.SetActive(true);
    }
}

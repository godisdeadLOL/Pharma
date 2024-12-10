using UnityEngine;
using UnityEngine.Events;

public class Pickable : MonoBehaviour
{
    public UnityAction OnPick;
    public UnityAction OnDrop;

    public SphereCollider Collider { get; private set; }

    public bool Picked { get; private set; }

    void Awake()
    {
        Collider = transform.Find("PickableCollider").GetComponent<SphereCollider>();

        OnPick += () => Pick();
        OnDrop += () => Drop();
    }

    void Pick()
    {
        Picked = true;
        Collider.gameObject.SetActive(false);
    }

    void Drop()
    {
        Picked = false;
        Collider.gameObject.SetActive(true);
    }
}

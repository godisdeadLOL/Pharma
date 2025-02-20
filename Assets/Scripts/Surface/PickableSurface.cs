using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickableSurface : MonoBehaviour
{
    private Surface _surface;

    void Awake()
    {
        _surface = GetComponent<Surface>();

        var pickable = GetComponentInParent<Pickable>();

        pickable.OnPick = OnPick;
        pickable.OnDrop = OnDrop;
    }

    private void OnPick()
    {
        _surface.Ignored = true;

        foreach(var pickable in _surface.Pickables)
        {
            if(!pickable.SkipCollider) pickable.Collider.enabled = false;
        }
    }
    private void OnDrop()
    {
        _surface.Ignored = false;

        foreach(var pickable in _surface.Pickables)
        {
            if(!pickable.SkipCollider) pickable.Collider.enabled = true;
        }
    }
}

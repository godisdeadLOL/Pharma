using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrashInteraction : MonoBehaviour
{
    void Awake()
    {
        GetComponent<Pickable>().OnInteracted += OnInteracted;
    }

    private void OnInteracted(Pickable pickable)
    {
        if (!pickable.name.Contains("Paper")) return;

        StateManager._inst.ChangeState<TableController>();
        Destroy(pickable.gameObject);
    }
}

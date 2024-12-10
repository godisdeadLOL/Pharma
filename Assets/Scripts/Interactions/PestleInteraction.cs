using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PestleInteraction : MonoBehaviour
{
    private Pickable _pickable;

    void Awake()
    {
        _pickable = GetComponent<Pickable>();
    }


    void OnEnable()
    {
        _pickable.OnInteract += Interact;
    }

    void OnDisable()
    {
        _pickable.OnInteract -= Interact;
    }

    private void Interact(Pickable pickable)
    {
        if (!pickable.TryGetComponent<MortarVisualData>(out var mortarVisualData)) return;

        MortarController mortarController = StateManager._inst.GetState<MortarController>();
        mortarController.MortarVisual = mortarVisualData;
        mortarController.Pestle = transform;

        StateManager._inst.ChangeState<MortarController>();
    }
}

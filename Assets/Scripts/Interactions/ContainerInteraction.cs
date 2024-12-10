using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContainerInteraction : MonoBehaviour
{
    private Container _container;
    private Pickable _pickable;

    void Awake()
    {
        _container = GetComponent<Container>();
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
        if (!pickable.TryGetComponent<Container>(out var container)) return;

        var transferController = StateManager._inst.GetState<TransferController>();

        transferController.Source = _container;
        transferController.Target = container;

        StateManager._inst.ChangeState<TransferController>();
    }
}

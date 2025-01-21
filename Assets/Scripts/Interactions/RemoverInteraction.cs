using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RemoverInteraction : MonoBehaviour
{
    private Container _container;
    private Pickable _pickable;

    void Awake()
    {
        _container = GetComponent<Container>();
        _pickable = GetComponent<Pickable>();

        _pickable.OnInteract += Interact;
    }

    private void Interact(Pickable pickable)
    {
        if (!pickable.name.Contains("Paper")) return;

        if (!pickable.TryGetComponent<Container>(out var container)) return;


        if (container.GetAmount() > 0)
        {
            var revertTransferController = StateManager._inst.GetState<RevertTransferController>();

            revertTransferController.To = _container;
            revertTransferController.From = container;

            StateManager._inst.ChangeState<RevertTransferController>();
        }
        else
        {
            var transferController = StateManager._inst.GetState<TransferController>();

            transferController.Source = _container;
            transferController.Target = container;

            StateManager._inst.ChangeState<TransferController>();
        }


    }
}

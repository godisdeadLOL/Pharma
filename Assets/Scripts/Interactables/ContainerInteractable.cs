using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContainerInteractable : Interactable
{
    // public override void Interact(GameObject pickable)
    // {
    //     //transform
    //     //pickable.transform

    //     if (pickable.GetComponent<Container>() == null) return;

    //     var transferController = StateManager._inst.GetState<TransferController>();

    //     transferController.pickable = transform.gameObject;
    //     transferController.interactable = pickable;

    //     StateManager._inst.ChangeState<TransferController>();
    // }
}

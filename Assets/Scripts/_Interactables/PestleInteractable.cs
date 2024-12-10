using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PestleInteractable : Interactable
{
    // public override void Interact(GameObject pickable)
    // {
    //     MortarVisualData mortarVisual = pickable.GetComponent<MortarVisualData>();
    //     if (mortarVisual == null) return;

    //     MortarController mortarController = StateManager._inst.GetState<MortarController>();
    //     mortarController.MortarVisual = mortarVisual;

    //     mortarController.Pestle = transform;

    //     StateManager._inst.ChangeState<MortarController>();
    //     // CameraManager._inst.SetPivot(pickable.transform);
    // }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrindAction : Action
{
    public override string Name => "Молоть";

    public override void Activate(GameObject pickable)
    {
        MortarVisualData mortarVisual = pickable.GetComponent<MortarVisualData>();
        if(mortarVisual == null) return;

        MortarController mortarController = StateManager._inst.GetState<MortarController>();
        mortarController.MortarVisual = mortarVisual;

        StateManager._inst.ChangeState<MortarController>();
        CameraManager._inst.SetPivot(pickable.transform);
    }
}

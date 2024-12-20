using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExamineAction : Action
{
    public override string Name => "Осмотреть";

    public override void Activate(GameObject pickable)
    {
        var examineController = StateManager._inst.GetState<ExamineController>();
        examineController.Target = pickable.transform;

        StateManager._inst.ChangeState<ExamineController>();
    }
}

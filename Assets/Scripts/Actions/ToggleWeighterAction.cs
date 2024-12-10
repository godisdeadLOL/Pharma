using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToggleWeighterAction : Action
{
    public override string Name => "Вкл/Выкл";

    public override void Activate(GameObject pickable)
    {
        var weighter = pickable.GetComponent<Weighter>();
        weighter.Toggle();
    }
}
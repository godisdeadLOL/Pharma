using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetWeighterAction : Action
{
    public override string Name => "Сбросить вес";

    public override void Activate(GameObject pickable)
    {
        var weighter = pickable.GetComponent<Weighter>();
        weighter.ResetWeight();
    }
}

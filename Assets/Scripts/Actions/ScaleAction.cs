using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScaleAction : Action
{
    public override string Name => "Увеличить";

    public override void Activate(GameObject pickable)
    {
        pickable.transform.localScale *= 1.1f;
    }
}

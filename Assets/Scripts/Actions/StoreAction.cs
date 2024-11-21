using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoreAction : Action
{
    public override string Name => "Взять";

    public override void Activate(GameObject pickable)
    {
        InventoryManager.main.Store(pickable);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Action : MonoBehaviour
{
    public abstract string Name { get; }

    public abstract void Activate(GameObject pickable);

    public virtual bool CanActivate(GameObject pickable)
    {
        return true;
    }
}

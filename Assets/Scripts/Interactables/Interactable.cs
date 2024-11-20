using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Interactable : MonoBehaviour
{
    public virtual void Interact(GameObject pickable) {}

    public virtual void Hold() { }
    public virtual void Release() { }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public enum InteractionLayer
{
    None, MovePoint, FocusTarget, Pickable, Button, Extra
}

public class Interactable : MonoBehaviour
{
    public UnityAction OnHoverEnter;
    public UnityAction OnHoverExit;

    public UnityEvent OnInteractPrimary;
    public UnityEvent OnInteractSecondary;

    public UnityAction OnInteractionEnabled;
    public UnityAction OnInteractionDisabled;

    [field: SerializeField] public bool CanInteract { get; private set; } = false;

    [field: SerializeField] public InteractionLayer Layer { get; private set; } = InteractionLayer.None;

    void Awake()
    {
        InteractionManager.Inst.OnLayerPushed += LayerPushed;
        InteractionManager.Inst.OnLayerCleared += LayerCleared;

        InteractionManager.Inst.OnInteractablePushed += InteractablePushed;
        InteractionManager.Inst.OnInteractableCleared += OnInteractableCleared;
    }

    private void LayerPushed(InteractionLayer layer)
    {
        if (!CanInteract && layer == Layer && layer != InteractionLayer.None)
        {
            OnInteractionEnabled?.Invoke();
            CanInteract = true;
        }
    }
    private void LayerCleared(InteractionLayer layer)
    {
        if (CanInteract && layer != Layer)
        {
            OnInteractionDisabled?.Invoke();
            CanInteract = false;
        }
    }

    private void InteractablePushed(Interactable interactable)
    {
        if (interactable == this)
        {
            OnInteractionEnabled?.Invoke();
            CanInteract = true;
        }
    }
    private void OnInteractableCleared(Interactable interactable)
    {
        if (interactable == this)
        {
            OnInteractionDisabled?.Invoke();
            CanInteract = false;
        }
    }
}

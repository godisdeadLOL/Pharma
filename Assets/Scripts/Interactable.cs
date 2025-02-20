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

    public UnityAction OnInteractionBlocked;
    public UnityAction OnInteractionUnblocked;

    [field: SerializeField] public bool CanInteract { get; private set; } = false;
    [field: SerializeField] public bool IsBlocked { get; private set; } = false;

    [field: SerializeField] public InteractionLayer Layer { get; private set; } = InteractionLayer.None;

    void Awake()
    {
        InteractionManager.Inst.OnLayerPushed += LayerPushed;
        InteractionManager.Inst.OnLayerCleared += LayerCleared;

        InteractionManager.Inst.OnInteractablePushed += InteractablePushed;
        InteractionManager.Inst.OnInteractableCleared += InteractableCleared;

        InteractionManager.Inst.OnAllBlocked += AllBlocked;
        InteractionManager.Inst.OnAllUnblocked += AllUnblocked;
        InteractionManager.Inst.OnUnblockedDirect += Unblocked;
        InteractionManager.Inst.OnUnblockedFilter += UnblockedFilter;
    }

    void OnDestroy()
    {
        InteractionManager.Inst.OnLayerPushed -= LayerPushed;
        InteractionManager.Inst.OnLayerCleared -= LayerCleared;

        InteractionManager.Inst.OnInteractablePushed -= InteractablePushed;
        InteractionManager.Inst.OnInteractableCleared -= InteractableCleared;

        InteractionManager.Inst.OnAllBlocked -= AllBlocked;
        InteractionManager.Inst.OnAllUnblocked -= AllUnblocked;
        InteractionManager.Inst.OnUnblockedDirect -= Unblocked;
        InteractionManager.Inst.OnUnblockedFilter -= UnblockedFilter;
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
        if (CanInteract && layer == Layer)
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
    private void InteractableCleared(Interactable interactable)
    {
        if (interactable == this)
        {
            OnInteractionDisabled?.Invoke();
            CanInteract = false;
        }
    }

    private void AllBlocked()
    {
        // if (!CanInteract) return;

        if (!IsBlocked)
        {
            IsBlocked = true;
            OnInteractionBlocked?.Invoke();
        }
    }
    private void AllUnblocked()
    {
        if (IsBlocked)
        {
            IsBlocked = false;
            OnInteractionUnblocked?.Invoke();
        }
    }
    private void Unblocked(Interactable interactable)
    {
        // if (!CanInteract) return;

        if (interactable == this && IsBlocked)
        {
            IsBlocked = false;
            OnInteractionUnblocked?.Invoke();
        }
    }

    private void UnblockedFilter(string filter)
    {
        if (gameObject.name.ToLower().Contains(filter.ToLower()) && IsBlocked)
        {
            IsBlocked = false;
            OnInteractionUnblocked?.Invoke();
        }
    }

    public void Block()
    {
        if(!IsBlocked)
        {
            IsBlocked = true;
            OnInteractionBlocked?.Invoke();
        }
    }

    public void Unblock()
    {
        if(IsBlocked)
        {
            IsBlocked = false;
            OnInteractionUnblocked?.Invoke();
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class InteractionManager : MonoBehaviour
{
    public static InteractionManager Inst { get; private set; }

    public UnityAction<Interactable> OnInteractPrimary;

    public UnityAction<InteractionLayer> OnLayerPushed;
    public UnityAction<InteractionLayer> OnLayerCleared;

    public UnityAction<Interactable> OnInteractablePushed;
    public UnityAction<Interactable> OnInteractableCleared;

    public UnityAction OnAllBlocked;
    public UnityAction OnAllUnblocked;
    public UnityAction<Interactable> OnUnblockedDirect;
    public UnityAction<string> OnUnblockedFilter;

    [field: SerializeField] public float InteractionDistance { get; set; } = 1f;
    [field: SerializeField] public bool BlockPrimary { get; set; } = false;
    [field: SerializeField] public bool BlockSecondary { get; set; } = false;
    [field: SerializeField] public bool BlockHover { get; set; } = false;

    [field: SerializeField] public LayerMask RaycastMask { get; set; }

    // private InteractionLayer _layer = InteractionLayer.None;
    // public InteractionLayer Layer
    // {
    //     get => _layer;
    //     set
    //     {
    //         _layer = value;
    //         OnLayerChange?.Invoke(_layer);
    //     }
    // }

    [SerializeField] private List<Interactable> _interactables = new();
    [SerializeField] private List<InteractionLayer> _layers = new();


    // private bool _blockHover = false;
    // public bool BlockHover
    // {
    //     get => _blockHover;
    //     set
    //     {
    //         _blockHover = value;

    //         if (_hovered != null && BlockHover)
    //         {
    //             _hovered.OnHoverExit?.Invoke();
    //             _hovered = null;
    //         }
    //         else if (_hovered == null && !BlockHover)
    //         {
    //             RaycastHover();
    //         }
    //     }
    // }

    private Interactable _hovered;

    void Awake()
    {
        Inst = this;
    }

    void Update()
    {
        // OnHover
        if (!BlockHover) RaycastHover();


        // OnInteract

        if (_hovered != null && !_hovered.IsBlocked)
        {

            if (!BlockPrimary && InputHandler.GetMouseButtonDown(0))
            {
                _hovered.OnInteractPrimary?.Invoke();
                OnInteractPrimary?.Invoke(_hovered);
            }
            else if (!BlockSecondary && InputHandler.GetMouseButtonDown(1))
            {
                _hovered.OnInteractSecondary?.Invoke();
            }

        }
    }

    private void RaycastHover()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out RaycastHit hit, InteractionDistance, RaycastMask)) // OnHoverEnter
        {
            GameObject gameObject = hit.collider.gameObject;

            // if (hit.rigidbody != null) gameObject = hit.rigidbody.gameObject;
            // else gameObject = hit.collider.gameObject;

            if (!gameObject.TryGetComponent<Interactable>(out var interactable))
                interactable = gameObject.GetComponentInParent<Interactable>();

            
            if(_hovered == interactable) return;

            // gameObject.TryGetComponent<Interactable>(out var interactable) && 

            if (interactable != null && interactable.CanInteract)
            {
                if (_hovered != null) //
                {
                    _hovered.OnHoverExit?.Invoke();
                    _hovered = null;
                }

                _hovered = interactable;
                interactable.OnHoverEnter?.Invoke();
                return;
            }
        }


        if (_hovered != null)
        {
            _hovered.OnHoverExit?.Invoke();
            _hovered = null;
        }
    }


    public void PushInteractable(Interactable interactable)
    {
        _interactables.Add(interactable);
        OnInteractablePushed?.Invoke(interactable);
    }
    public void ClearInteractables()
    {
        foreach (var interactable in _interactables)
        {
            OnInteractableCleared?.Invoke(interactable);
        }
        _interactables.Clear();
    }

    public void PushLayer(InteractionLayer layer)
    {
        _layers.Add(layer);
        OnLayerPushed?.Invoke(layer);
    }
    public void ClearLayers()
    {
        foreach (var layer in _layers)
        {
            OnLayerCleared?.Invoke(layer);
        }
        _layers.Clear();
    }

    public void BlockAll()
    {
        OnAllBlocked?.Invoke();
    }

    public void UnblockAll()
    {
        OnAllUnblocked?.Invoke();
    }

    public void Unblock(Interactable interactable)
    {
        OnUnblockedDirect?.Invoke(interactable);
    }

    public void UnblockByFilter(string filter)
    {
        OnUnblockedFilter?.Invoke(filter);
    }

    // public void Register(Interactable interactable)
    // {
    //     _interactables.Add(interactable);
    // }
    // public void Unregister(Interactable interactable)
    // {
    //     _interactables.Remove(interactable);
    // }
}

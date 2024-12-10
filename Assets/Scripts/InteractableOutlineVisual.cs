using UnityEngine;

[RequireComponent(typeof(Outline))]
public class InteractableOutlineVisual : MonoBehaviour
{
    [SerializeField] private float _hoverWidth = 7f;
    [SerializeField] private Color _hoverColor = Color.white;

    private Outline _outline;

    private float _startWidth;
    private Color _startColor;


    private bool _hovered = false;

    private Interactable _interactable;

    void Awake()
    {
        _outline = GetComponent<Outline>();

        _outline.enabled = false;

        _startWidth = _outline.OutlineWidth;
        _startColor = _outline.OutlineColor;

        _interactable = GetComponent<Interactable>();
        if (_interactable == null) _interactable = GetComponentInParent<Interactable>();

        _interactable.OnInteractionEnabled += InteractionEnabled; // () => { _outline.enabled = true; HoverExit(); };
        _interactable.OnInteractionDisabled += InteractionDisabled; // () => { _outline.enabled = false; print("disable"); };

        // interactable.OnHoverEnter += HoverEnter;  // () => { _hovered = true; _outline.enabled = true; };
        // interactable.OnHoverExit += HoverExit; //() => _hovered = false;
    }

    private void InteractionEnabled()
    {
        _outline.enabled = true; 
        HoverExit();

        _interactable.OnHoverEnter += HoverEnter;
        _interactable.OnHoverExit += HoverExit;
    }
    private void InteractionDisabled()
    {
        _outline.enabled = false;

        _interactable.OnHoverEnter -= HoverEnter;
        _interactable.OnHoverExit -= HoverExit;
    }

    private void HoverEnter()
    {
        _hovered = true;

        _outline.OutlineWidth = _hoverWidth;
        _outline.OutlineColor = _hoverColor;

        _outline.enabled = true;
    }

    private void HoverExit()
    {
        _hovered = false;

        _outline.OutlineWidth = _startWidth;
        _outline.OutlineColor = _startColor;

        _outline.enabled = !Mathf.Approximately(_outline.OutlineColor.a, 0f);
    }

    // void Update()
    // {
    //     _outline.OutlineWidth = Lerping.Lerp(_outline.OutlineWidth, _hovered ? _hoverWidth : _startWidth, Lerping.Smooth.VeryFast);
    //     _outline.OutlineColor = Lerping.Lerp(_outline.OutlineColor, _hovered ? _hoverColor : _startColor, Lerping.Smooth.VeryFast);

    //     if (!_hovered && Mathf.Approximately(_outline.OutlineWidth, _startWidth)) _outline.enabled = false;
    // }
}

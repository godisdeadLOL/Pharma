using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableBillboardVisual : MonoBehaviour
{
    [SerializeField] private float _hoverScale = 1f;

    private Vector3 _startScale;

    private bool _hovered = false;

    void Awake()
    {
        var interactable = GetComponentInParent<Interactable>();
        interactable.OnHoverEnter += () => _hovered = true;
        interactable.OnHoverExit += () => _hovered = false;

        var sprite = GetComponent<SpriteRenderer>();
        interactable.OnInteractionBlocked += () => sprite.color = Color.red;
        interactable.OnInteractionUnblocked += () => sprite.color = Color.white;

        interactable.OnInteractionDisabled += () => gameObject.SetActive(false);
        interactable.OnInteractionEnabled += () => gameObject.SetActive(true);
    }

    void Start()
    {
        _startScale = transform.localScale;
    }

    void Update()
    {
        transform.localScale = Lerping.Lerp(transform.localScale, _hovered ? (_startScale * _hoverScale) : _startScale, Lerping.Smooth.VeryFast);
    }
}

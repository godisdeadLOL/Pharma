using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableBillboardVisual : MonoBehaviour
{
    [SerializeField] private float _hoverScale = 1f;

    private Vector3 _startScale;

    private bool _hovered = false;

    void Start()
    {
        _startScale = transform.localScale;

        var interactable = GetComponentInParent<Interactable>();
        interactable.OnHoverEnter += () => _hovered = true;
        interactable.OnHoverExit += () => _hovered = false;
    }

    void Update()
    {
        transform.localScale = Lerping.Lerp(transform.localScale, _hovered ? (_startScale * _hoverScale) : _startScale, Lerping.Smooth.VeryFast);
    }
}

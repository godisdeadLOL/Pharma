using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class FocusTarget : MonoBehaviour
{
    [field: SerializeField] public Transform Pivot { get; private set; }
    [field: SerializeField] public GameObject State { get; private set; }
    [field: SerializeField] public float InteractionDistance { get; private set; } = 3.0f;

    [field: SerializeField] public UnityEvent OnEnter { get; private set; }
    [field: SerializeField] public UnityEvent OnExit { get; private set; }

    [SerializeField] private Collider _focusCollider;
    private Outline _outline;

    void Awake()
    {
        _outline = GetComponent<Outline>();
        // SetActive(false);
    }

    public void SetActive(bool active)
    {
        if (active)
        {
            // _outline.enabled = true;
            _focusCollider.enabled = true;
        }
        else
        {
            // _outline.enabled = false;
            _focusCollider.enabled = false;
        }
    }
}

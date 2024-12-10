using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Actions : MonoBehaviour
{
    public Action[] profile;

    private Interactable _interactable;

    void Awake()
    {
        _interactable = GetComponent<Interactable>();
    }

    void OnEnable()
    {
        _interactable.OnInteractSecondary.AddListener(InteractSecondary);
    }
    void OnDisable()
    {
        _interactable.OnInteractSecondary.RemoveListener(InteractSecondary);
    }

    private void InteractSecondary()
    {
        ActionManager._inst.Display(gameObject, profile);
    }
}

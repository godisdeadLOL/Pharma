using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RagInteractable : Interactable
{
    private GameObject _dustEraser;

    void Awake()
    {
        _dustEraser = GetComponentInChildren<DustEraser>(true).gameObject;
        _dustEraser.SetActive(false);
    }

    public override void Hold()
    {
        var tableController = StateManager._inst.GetState<TableController>();
        tableController.ElevationOffset = -0.35f;
        _dustEraser.SetActive(true);
    }
    public override void Release()
    {
        var tableController = StateManager._inst.GetState<TableController>();
        tableController.ElevationOffset = 0f;
        _dustEraser.SetActive(false);
    }
}

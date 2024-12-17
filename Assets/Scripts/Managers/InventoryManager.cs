using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Inst { get; private set; }

    [SerializeField] private Transform[] _itemBoxes;

    private TableController _tableController;


    void Awake()
    {
        Inst = this;
        _tableController = StateManager._inst.GetState<TableController>();
    }

    private Transform GetEmptyBox()
    {
        foreach(Transform itemBox in _itemBoxes)
        {
            if(itemBox.childCount == 0) return itemBox;
        }

        return null;
    }


    public void Store(GameObject pickable)
    {
        Transform itemBox = GetEmptyBox();
        if(itemBox == null) return;

        // var rb = pickable.GetComponent<Rigidbody>();

        // if (rb != null)
        // {
        //     rb.isKinematic = true;
        //     rb.velocity = Vector3.zero;
        //     rb.interpolation = RigidbodyInterpolation.None;
        // }

        pickable.transform.SetParent(itemBox, false);
        pickable.transform.localPosition = Vector3.zero;
    }

    public void Drop(int slot)
    {
        // float interactionDistance = CameraManager._inst.InteractionDistance;

        if (_itemBoxes[slot].childCount == 0) return;

        // Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        // if (!Physics.Raycast(ray, out RaycastHit hit, interactionDistance)) return;

        // Vector3 target = hit.point;

        GameObject pickable = _itemBoxes[slot].GetChild(0).gameObject;

        pickable.transform.SetParent(null, false);
        pickable.transform.position = _tableController.TargetPosition;

        if (_tableController.TargetAnchor != null) _tableController.TargetAnchor.gameObject.SetActive(false);
    }
}

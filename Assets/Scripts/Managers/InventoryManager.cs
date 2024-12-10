using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager main { get; private set; }

    [SerializeField]
    private Transform[] _itemBoxes;
    void Awake()
    {
        main = this;
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

    public void Drop(int slot, Vector2 mousePosition)
    {
        float interactionDistance = CameraManager._inst.InteractionDistance;

        if (_itemBoxes[slot].childCount == 0) return;

        Ray ray = Camera.main.ScreenPointToRay(mousePosition);
        if (!Physics.Raycast(ray, out RaycastHit hit, interactionDistance)) return;

        Vector3 target = hit.point;// + Vector3.up * 0.2f;

        GameObject pickable = _itemBoxes[slot].GetChild(0).gameObject;
        // var rb = pickable.GetComponent<Rigidbody>();

        // if (rb != null)
        // {
        //     rb.isKinematic = false;
        //     rb.interpolation = RigidbodyInterpolation.Interpolate;
        // }

        pickable.transform.SetParent(null, false);
        pickable.transform.position = target;
    }
}

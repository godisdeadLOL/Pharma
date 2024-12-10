using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class MortarController : MonoBehaviour
{
    [field: SerializeField] public MortarVisualData MortarVisual { get; set; }

    public Transform Pestle { get; set; }

    void Update()
    {
        if (InputHandler.GetMouseButtonDown(1))
        {
            StateManager._inst.ChangeState<TableController>();
            return;
        }

        Vector3 targetOffset = InputHandler.GetMouseButton(0) ? Vector3.forward * (MortarVisual.PestleOffset + MortarVisual.PestleStart) : Vector3.forward * MortarVisual.PestleStart;
        MortarVisual.PestleHandle.localPosition = Vector3.Lerp(MortarVisual.PestleHandle.localPosition, targetOffset, Time.deltaTime * 10f);


        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (!MortarVisual.InteractionPlane.Raycast(ray, out RaycastHit hit, float.PositiveInfinity)) return;

        Vector3 targetTop = hit.point - MortarVisual.MortarBase.position;

        targetTop.y = 0;
        if (targetTop.magnitude > MortarVisual.ClampTopRadius)
            targetTop = targetTop.normalized * MortarVisual.ClampTopRadius;

        targetTop.y = MortarVisual.TopPlane.localPosition.y;
        MortarVisual.PestlePivot.localPosition = targetTop;


        Vector3 targetBottom = targetTop;
        targetBottom.y = 0;
        targetBottom = targetBottom / MortarVisual.TopRadius * MortarVisual.BottomRadius;
        targetBottom.y = MortarVisual.BottomPlane.localPosition.y;

        MortarVisual.PestlePivot.LookAt(targetBottom + MortarVisual.MortarBase.position);
    }

    void OnEnable()
    {
        MortarVisual.InteractionPlane.enabled = true;

        Pestle.SetParent(MortarVisual.PestleHandle, false);
        Pestle.localPosition = Vector3.zero;
        Pestle.localRotation = Quaternion.identity;
    }

    void OnDisable()
    {
        if (MortarVisual == null) return;

        MortarVisual.InteractionPlane.enabled = false;
        Pestle.SetParent(null);
        Pestle.localRotation = Quaternion.identity;
    }
}

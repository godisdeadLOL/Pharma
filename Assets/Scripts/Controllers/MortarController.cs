using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;

public class MortarController : MonoBehaviour
{
    public UnityAction<float> OnGrinded;

    [field: SerializeField] public MortarVisualData MortarVisual { get; set; }

    public Transform Pestle { get; set; }

    private float _intensity;

    [SerializeField] private float _increaseK = 1f;
    [SerializeField] private float _decreaseK = 1f;
    [SerializeField] private float _grindThreshold = 0.8f;

    private Vector3 _location;
    private float _grindTimer;
    private bool _targeted;

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

        // Интенсивность перемолки
        if (!_targeted)
        {
            _targeted = true;
        }
        else if (InputHandler.GetMouseButton(0))
        {
            float delta = (_location - hit.point).magnitude;
            _intensity = Mathf.Clamp01(_intensity + delta * _increaseK - Time.deltaTime * _decreaseK);

            if (_intensity > _grindThreshold)
            {
                _grindTimer += Time.deltaTime;
                if (_grindTimer > 0.1f)
                {
                    OnGrinded?.Invoke(0.1f);
                    _grindTimer -= 0.1f;
                }
            }
        }


        if (InputHandler.GetMouseButtonUp(0))
        {
            if (!Mathf.Approximately(_grindTimer, 0f)) OnGrinded?.Invoke(_grindTimer);

            _intensity = 0f;
            _grindTimer = 0f;
        }

        _location = hit.point;

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

        _targeted = false;
    }

    void OnDisable()
    {
        if (MortarVisual == null) return;

        MortarVisual.InteractionPlane.enabled = false;
        Pestle.SetParent(null);
        Pestle.localRotation = Quaternion.identity;
    }
}

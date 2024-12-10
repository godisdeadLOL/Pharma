using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementController : MonoBehaviour
{
    [SerializeField] private LayerMask _pointLayer;
    [SerializeField] private LayerMask _targetLayer;

    [field: SerializeField] public MovementPoint CurrentPoint { get; private set; }
    public FocusTarget CurrentTarget { get; private set; }

    void Start()
    {
        ApproachPoint(CurrentPoint);
    }

    void OnEnable()
    {
        InteractionManager.Inst.InteractionDistance = float.PositiveInfinity;
        InteractionManager.Inst.BlockSecondary = true;
        InteractionManager.Inst.PushLayer(InteractionLayer.MovePoint);
        InteractionManager.Inst.RaycastMask = LayerMask.GetMask("MovePoint", "FocusTarget");

        InteractionManager.Inst.OnInteractPrimary += OnInteract;
    }
    void OnDisable()
    {
        InteractionManager.Inst.BlockSecondary = false;
        InteractionManager.Inst.ClearLayers();

        InteractionManager.Inst.OnInteractPrimary -= OnInteract;
    }

    private void OnInteract(Interactable interactable)
    {
        if (interactable.Layer == InteractionLayer.MovePoint)
        {
            MovementPoint point = interactable.GetComponent<MovementPoint>();

            LeavePoint();
            ApproachPoint(point);
        }
        else if(interactable.Layer == InteractionLayer.FocusTarget)
        {
            FocusTarget target = interactable.GetComponent<FocusTarget>();
            
            EnterTarget(target);
        }
    }

    private void LeavePoint()
    {
        CurrentPoint.gameObject.SetActive(true);

        InteractionManager.Inst.ClearInteractables();
    }
    private void ApproachPoint(MovementPoint next)
    {
        CurrentPoint = next;
        CurrentPoint.gameObject.SetActive(false);

        CameraManager._inst.CurrentPivot = CurrentPoint.transform;

        foreach (var interactable in CurrentPoint.Interactables)
        {
            InteractionManager.Inst.PushInteractable(interactable);
        }
    }

    private void EnterTarget(FocusTarget next)
    {
        CurrentTarget = next;

        // foreach (var target in CurrentPoint.Targets) target.SetActive(false);

        CameraManager._inst.CanRotate = false;
        CameraManager._inst.CurrentPivot = next.Pivot;
        StateManager._inst.ChangeState(next.State);

        CameraManager._inst.InteractionDistance = next.InteractionDistance;

        next.OnEnter.Invoke();
    }

    public void ExitTarget()
    {
        CurrentTarget.OnExit.Invoke();

        StateManager._inst.ChangeState<MovementController>();
        CameraManager._inst.CanRotate = true;
        ApproachPoint(CurrentPoint);

        CurrentTarget = null;
    }
}

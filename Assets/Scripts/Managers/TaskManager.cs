using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class Task
{
    public enum TaskAction
    {
        WaitForDirectInteract,
        WaitForContainerTransfer
    }

    public UnityAction OnCompleted;

    public TaskAction Action = TaskAction.WaitForDirectInteract;

    public Interactable Target = null;
    public string TargetFilter = "";

    public string Hint;
    public bool Blocking = true;
    public bool Save = false;

    private Container _container;

    private static Interactable _saved;

    public void Activate()
    {
        if (Action == TaskAction.WaitForDirectInteract)
        {

            if (Blocking) InteractionManager.Inst.BlockAll();


            if (Target != null)
            {
                Target.OnInteractPrimary.AddListener(Interact);

                if (Blocking) InteractionManager.Inst.Unblock(Target);
            }
            else
            {
                InteractionManager.Inst.OnInteractPrimary += Interact;

                if (Blocking) InteractionManager.Inst.UnblockByFilter(TargetFilter);
            }

        }
        else if (Action == TaskAction.WaitForContainerTransfer)
        {
            _container = _saved.GetComponent<Container>();
            _container.OnAmountChanged += AmountChanged;
        }
    }

    public void Deactivate()
    {
        if (Action == TaskAction.WaitForDirectInteract)
        {

            InteractionManager.Inst.UnblockAll();

            if (Target != null)
            {
                Target.OnInteractPrimary.RemoveListener(Interact);
            }
            else
            {
                InteractionManager.Inst.OnInteractPrimary -= Interact;
            }

        }
        else if (Action == TaskAction.WaitForContainerTransfer)
        {
            _container.OnAmountChanged -= AmountChanged;
        }
    }

    private void Interact()
    {
        OnCompleted?.Invoke();
    }
    private void Interact(Interactable interactable)
    {
        if (!interactable.gameObject.name.ToLower().Contains(TargetFilter.ToLower())) return;

        if (Save) _saved = interactable;

        OnCompleted?.Invoke();
    }
    private void AmountChanged(int amount)
    {
        if (_container.GetAmount() == 0) OnCompleted?.Invoke();
    }
}

public class TaskManager : MonoBehaviour
{
    public UnityAction<Task> OnTaskChanged;

    [SerializeField] private Task[] _tasks;

    private int _taskIndex = -1;

    void Start()
    {
        NextTask();
    }

    private void NextTask()
    {
        if (_taskIndex >= 0)
        {
            _tasks[_taskIndex].Deactivate();
            _tasks[_taskIndex].OnCompleted -= NextTask;
        }

        _taskIndex++;

        if (_taskIndex == _tasks.Length)
        {
            OnTaskChanged?.Invoke(null);
            return;
        }

        OnTaskChanged?.Invoke(_tasks[_taskIndex]);

        _tasks[_taskIndex].OnCompleted += NextTask;
        _tasks[_taskIndex].Activate();

    }
}

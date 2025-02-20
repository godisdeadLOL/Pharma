using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[Serializable]
public abstract class TaskBase
{
    public UnityAction OnCompleted;
    public UnityAction<string> OnPopupChanged;

    public virtual bool Skipped { get; private set; } = false;

    public abstract void Start();
    public abstract void Stop();

    public abstract string GetPopup();
}

[Serializable]
public class DirectInteractTask : TaskBase
{
    public string Popup;
    public Interactable Interactable;

    public override string GetPopup()
    {
        return Popup;
    }

    public override void Start()
    {
        Interactable.OnInteractPrimary.AddListener(OnInteractPrimary);
    }

    public override void Stop()
    {
        Interactable.OnInteractPrimary.RemoveListener(OnInteractPrimary);
    }

    private void OnInteractPrimary()
    {
        OnCompleted?.Invoke();
    }
}

[Serializable]
public class ToggleInteractionTask : TaskBase
{
    public override bool Skipped => true;

    public Interactable Interactable;

    public override string GetPopup()
    {
        return "";
    }

    public override void Start()
    {
        if (Interactable.IsBlocked) Interactable.Unblock();
        else Interactable.Block();
    }

    public override void Stop()
    {

    }
}


public class TaskManager : MonoBehaviour
{
    public UnityAction<string> OnPopupChanged;

    [SerializeReference, SubclassSelector] private TaskBase[] _tasks;

    private int _taskIndex = -1;

    void Start()
    {
        NextTask();
    }

    private void NextTask()
    {
        // Отписаться от текущей задачи
        if (_taskIndex >= 0)
        {
            _tasks[_taskIndex].Stop();
            _tasks[_taskIndex].OnCompleted -= NextTask;
            _tasks[_taskIndex].OnPopupChanged -= ChangePopup;
        }
        
        // Проверка на выполнение всех задач
        _taskIndex++;
        if (_taskIndex >= _tasks.Length)
        {
            OnPopupChanged?.Invoke("Все задачи выполнены");
            return;
        }

        // Подписаться на следующую задачу
        TaskBase task = _tasks[_taskIndex];

        task.OnCompleted += NextTask;
        task.OnPopupChanged += ChangePopup;
        task.Start();

        task.OnPopupChanged?.Invoke(task.GetPopup());

        if (task.Skipped) NextTask();
    }

    private void ChangePopup(string popup)
    {
        OnPopupChanged?.Invoke(popup);
    }
}

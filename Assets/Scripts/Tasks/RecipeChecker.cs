using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

[Serializable]
public abstract class RecipeSubtaskBase
{
    public UnityAction OnCompleted;
    public UnityAction<string> OnPopupChanged;
    public UnityAction<float> OnProgress;

    public Color TargetColor;

    public abstract void Start(CompleteRecipeTask recipeChecker);
    public abstract void Stop(CompleteRecipeTask recipeChecker);
    public abstract string GetPopup();
}

[Serializable]
public class AddIngredientRecipeSubtask : RecipeSubtaskBase
{
    public IngredientData IngredientData;
    public int TargetAmount;

    private int _added;

    public override string GetPopup()
    {
        return $"Добавьте {_added}/{TargetAmount}u {IngredientData.name}";
    }

    public override void Start(CompleteRecipeTask recipeChecker)
    {
        recipeChecker.Container.OnSolutionAdded += OnSolutionAdded;
        recipeChecker.Container.ApplyFilter = true;
        recipeChecker.Container.Filter = IngredientData;
    }

    public override void Stop(CompleteRecipeTask recipeChecker)
    {
        recipeChecker.Container.OnSolutionAdded -= OnSolutionAdded;
        _added = 0;
        recipeChecker.Container.Filter = null;
    }

    private void OnSolutionAdded(ContainerSolution solution)
    {
        if (solution.ingredientData == IngredientData)
        {
            _added += solution.Amount;
            OnProgress?.Invoke((float)_added / TargetAmount);
            OnPopupChanged?.Invoke(GetPopup());
        }

        if (_added >= TargetAmount) OnCompleted?.Invoke();
    }
}

[Serializable]
public class GrindRecipeSubtask : RecipeSubtaskBase
{
    public float TargetTime;

    private float _grindTime;

    public override string GetPopup()
    {
        return $"Перемалывайте в течении {_grindTime}/{TargetTime} секунд";
    }

    public override void Start(CompleteRecipeTask recipeChecker)
    {
        recipeChecker.MortarController.OnGrinded += OnGrinded;
    }

    public override void Stop(CompleteRecipeTask recipeChecker)
    {
        recipeChecker.MortarController.OnGrinded -= OnGrinded;
    }

    private void OnGrinded(float time)
    {
        _grindTime += time;
        OnProgress?.Invoke(_grindTime / TargetTime);
        OnPopupChanged?.Invoke(GetPopup());

        if (_grindTime >= TargetTime) OnCompleted?.Invoke();
    }
}

[Serializable]
public class CompleteRecipeTask : TaskBase
{
    public Container Container;
    public MortarController MortarController;
    [SerializeReference, SubclassSelector] public RecipeSubtaskBase[] Subtasks;

    private int _subtaskIndex = -1;

    public override string GetPopup()
    {
        if (_subtaskIndex < 0 || _subtaskIndex >= Subtasks.Length) return "";
        return Subtasks[_subtaskIndex].GetPopup();
    }

    public override void Start()
    {
        _subtaskIndex = -1;
        NextSubtask();
    }

    public override void Stop()
    {
    }

    private void NextSubtask()
    {
        // Отписаться от текущей подзадачи
        if (_subtaskIndex >= 0)
        {
            Subtasks[_subtaskIndex].Stop(this);
            Subtasks[_subtaskIndex].OnCompleted -= NextSubtask;
            Subtasks[_subtaskIndex].OnProgress -= OnProgress;
            Subtasks[_subtaskIndex].OnPopupChanged -= ChangePopup;
        }

        // Проверка на выполнение всех подзадач
        _subtaskIndex++;
        if (_subtaskIndex >= Subtasks.Length)
        {
            OnCompleted?.Invoke();
            return;
        }

        // Подписаться на следующую подзадачу
        RecipeSubtaskBase subtask = Subtasks[_subtaskIndex];

        subtask.OnCompleted += NextSubtask;
        subtask.OnProgress += OnProgress;
        subtask.OnPopupChanged += ChangePopup;
        subtask.Start(this);

        subtask.OnPopupChanged?.Invoke(subtask.GetPopup());
    }

    private void ChangePopup(string popup)
    {
        OnPopupChanged?.Invoke(popup);
    }

    private void OnProgress(float t)
    {
        Color prev = Subtasks[_subtaskIndex == 0 ? 0 : _subtaskIndex - 1].TargetColor;
        Color next = Subtasks[_subtaskIndex].TargetColor;

        Container.ColorOverrideLerp = 1f;
        Container.ColorOverride = Color.Lerp(prev, next, t);
        Container.ForceUpdateColor();
    }
}


// public class RecipeChecker
// {
//     // [field: SerializeField] public Recipe Recipe { get; private set; }

//     [field: SerializeField] public Container Container { get; private set; }
//     [field: SerializeField] public MortarController MortarController { get; private set; }

//     [SerializeReference, SubclassSelector] private List<RecipeActionBase> _actions;
//     private int _actionPointer = -1;

//     void Start()
//     {
//         NextAction();
//     }

//     private void NextAction()
//     {
//         if (_actionPointer >= _actions.Count - 1) return;

//         if (_actionPointer >= 0)
//         {
//             _actions[_actionPointer].Stop(this);
//             _actions[_actionPointer].OnCompleted -= OnCompleted;
//             _actions[_actionPointer].OnProgress -= OnHelperUpdated;
//         }

//         _actionPointer += 1;

//         _actions[_actionPointer].Start(this);
//         _actions[_actionPointer].OnCompleted += OnCompleted;
//         _actions[_actionPointer].OnProgress += OnHelperUpdated;

//         _text.text = _actions[_actionPointer].GetHelper();
//     }
//     private void OnCompleted()
//     {
//         NextAction();
//     }
//     private void OnHelperUpdated(float t)
//     {
//         _text.text = _actions[_actionPointer].GetHelper();

//         // смешать цвета

//         Color prev = _actions[_actionPointer == 0 ? 0 : _actionPointer - 1].TargetColor;
//         Color next = _actions[_actionPointer].TargetColor;

//         Container.ColorOverrideLerp = 1f;
//         Container.ColorOverride = Color.Lerp(prev, next, t);
//         Container.ForceUpdateColor();
//     }

// }

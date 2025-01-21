using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UiTaskHint : MonoBehaviour
{
    [SerializeField] private TaskManager _taskManager;
    private TMP_Text _text;

    private void Awake()
    {
        _text = GetComponent<TMP_Text>();
        _taskManager.OnTaskChanged += TaskChanged;
    }

    private void TaskChanged(Task task)
    {
        if(task == null) {
            _text.text = "";
            return;
        }

        _text.text = task.Hint;
    }
}

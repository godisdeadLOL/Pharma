using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class StateManager : MonoBehaviour
{
    public static StateManager _inst;

    public UnityAction<MonoBehaviour> OnStateChange;

    void Awake()
    {
        _inst = this;

        for (int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).gameObject.SetActive(false);
        }
    }

    void Start()
    {
        ChangeState<MovementController>();
    }

    public void ChangeState<T>() where T : MonoBehaviour
    {
        MonoBehaviour state = GetComponentInChildren<T>(true);
        GameObject target = state.gameObject;

        for (int i = 0; i < transform.childCount; i++)
        {
            GameObject gameObject = transform.GetChild(i).gameObject;
            if(gameObject.activeSelf) gameObject.SetActive(false);
        }

        target.SetActive(true);

        OnStateChange?.Invoke(state);
    }

    public void ChangeState(GameObject target)
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            GameObject gameObject = transform.GetChild(i).gameObject;
            if(gameObject.activeSelf) gameObject.SetActive(false);
        }

        target.SetActive(true);

        OnStateChange?.Invoke(target.GetComponent<MonoBehaviour>());
    }

    public T GetState<T>() where T : MonoBehaviour
    {
        return GetComponentInChildren<T>(true);
    }
}

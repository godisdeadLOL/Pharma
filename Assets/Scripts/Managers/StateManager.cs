using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateManager : MonoBehaviour
{
    public static StateManager _inst;

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
        ChangeState<TableController>();
    }

    public void ChangeState<T>() where T : MonoBehaviour
    {
        GameObject target = GetComponentInChildren<T>(true).gameObject;

        for (int i = 0; i < transform.childCount; i++)
        {
            GameObject gameObject = transform.GetChild(i).gameObject;

            gameObject.SetActive(gameObject == target);
        }
    }

    public void ChangeState(GameObject target)
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            GameObject gameObject = transform.GetChild(i).gameObject;

            gameObject.SetActive(gameObject == target);
        }
    }

    public T GetState<T>() where T : MonoBehaviour
    {
        return GetComponentInChildren<T>(true);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UiBackButton : MonoBehaviour
{
    void Awake()
    {
        StateManager._inst.OnStateChange += StateChange;
    }

    private void StateChange(MonoBehaviour state)
    {
        gameObject.SetActive(state is not MovementController);
    }
}

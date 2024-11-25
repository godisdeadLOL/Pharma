using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public static class InputHandler
{

    public static bool GetMouseButtonDown(int mouseButton)
    {
        if (EventSystem.current.IsPointerOverGameObject()) return false;

        return Input.GetMouseButtonDown((int)mouseButton);
    }

    public static bool GetMouseButtonUp(int mouseButton)
    {
        if (EventSystem.current.IsPointerOverGameObject()) return false;

        return Input.GetMouseButtonUp((int)mouseButton);
    }

    public static bool GetMouseButton(int mouseButton)
    {
        if (EventSystem.current.IsPointerOverGameObject()) return false;

        return Input.GetMouseButton((int)mouseButton);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Lerping
{
    public enum Smooth
    {
        Slow = 5,
        Average = 10,
        Fast = 15,
        VeryFast = 20
    }

    public enum Type
    {
        Fixed,
        Screen
    }

    public static float Lerp(float from, float to, Smooth smooth, float epsilon = 0.5f, Type type = Type.Screen)
    {
        float value = Mathf.Lerp(from, to, (type == Type.Screen ? Time.smoothDeltaTime : Time.fixedDeltaTime) * (int)smooth);
        if(Mathf.Abs(value-to) < epsilon) return to;
        else return value;
    }
    public static float Lerp(float from, float to, float smooth, Type type = Type.Screen)
    {
        return Mathf.Lerp(from, to, (type == Type.Screen ? Time.smoothDeltaTime : Time.fixedDeltaTime) * smooth);
    }

    public static Vector3 Lerp(Vector3 from, Vector3 to, Smooth smooth, Type type = Type.Screen)
    {
        return Vector3.Lerp(from, to, (type == Type.Screen ? Time.smoothDeltaTime : Time.fixedDeltaTime) * (int)smooth);
    }
    public static Vector3 Lerp(Vector3 from, Vector3 to, float smooth, Type type = Type.Screen)
    {
        return Vector3.Lerp(from, to, (type == Type.Screen ? Time.smoothDeltaTime : Time.fixedDeltaTime) * smooth);
    }

    public static Vector2 Lerp(Vector2 from, Vector2 to, Smooth smooth, Type type = Type.Screen)
    {
        return Vector2.Lerp(from, to, (type == Type.Screen ? Time.smoothDeltaTime : Time.fixedDeltaTime) * (int)smooth);
    }
    public static Vector2 Lerp(Vector2 from, Vector2 to, float smooth, Type type = Type.Screen)
    {
        return Vector2.Lerp(from, to, (type == Type.Screen ? Time.smoothDeltaTime : Time.fixedDeltaTime) * smooth);
    }

    public static Quaternion Lerp(Quaternion from, Quaternion to, Smooth smooth, Type type = Type.Screen)
    {
        return Quaternion.Lerp(from, to, (type == Type.Screen ? Time.smoothDeltaTime : Time.fixedDeltaTime) * (int)smooth);
    }
    public static Quaternion Lerp(Quaternion from, Quaternion to, float smooth, Type type = Type.Screen)
    {
        return Quaternion.Lerp(from, to, (type == Type.Screen ? Time.smoothDeltaTime : Time.fixedDeltaTime) * smooth);
    }

    public static Color Lerp(Color from, Color to, Smooth smooth, Type type = Type.Screen)
    {
        return Color.Lerp(from, to, (type == Type.Screen ? Time.smoothDeltaTime : Time.fixedDeltaTime) * (int)smooth);
    }
    public static Color Lerp(Color from, Color to, float smooth, Type type = Type.Screen)
    {
        return Color.Lerp(from, to, (type == Type.Screen ? Time.smoothDeltaTime : Time.fixedDeltaTime) * smooth);
    }
}

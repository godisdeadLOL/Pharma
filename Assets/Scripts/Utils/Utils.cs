using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Utils
{
    public static float DistanceToRay(Vector3 point, Ray ray)
    {
        Vector3 r1 = ray.origin;
        Vector3 r2 = ray.origin + ray.direction;
        Vector3 pr1 = (point - r1);
        Vector3 pr2 = (point - r2);

        return Vector3.Cross(pr1, pr2).magnitude / (r1 - r2).magnitude;
    }
}

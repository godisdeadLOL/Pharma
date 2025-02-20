using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SurfaceManager : MonoBehaviour
{
    public static SurfaceManager Instance { get; private set; }

    private Surface _current;
    private List<Surface> _surfaces = new();

    void Awake()
    {
        Instance = this;
    }

    public void Register(Surface surface)
    {
        _surfaces.Add(surface);
    }
    public void Unregister(Surface surface)
    {
        _surfaces.Remove(surface);
    }

    public bool Raycast(Ray ray, out Vector3 point, out Surface surface, float maxDistance = float.PositiveInfinity)
    {
        point = Vector3.zero;
        surface = null;

        float closest = float.PositiveInfinity;
        bool collided = false;
        foreach (var surf in _surfaces)
        {
            if (!surf.Raycast(ray, maxDistance, out var _point, restrict: true)) continue;

            float dist = Vector3.Distance(ray.origin, _point);

            if (dist < closest)
            {
                closest = dist;
                _current = surf;
                surface = surf;
                point = _point;
                collided = true;
            }
        }

        if (collided)
        {
            return true;
        }

        if (_current != null)
        {
            if (!_current.Raycast(ray, maxDistance, out point, clamp: true)) return false;
            surface = _current;
            return true;
        }

        return false;
    }
}

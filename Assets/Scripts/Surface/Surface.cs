using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Surface : MonoBehaviour
{
    private Vector3 _debug;

    [SerializeField] private Vector2 _bounds;
    public bool Ignored { get; set; }

    public List<Pickable> Pickables { get; private set; } = new();

    void OnEnable()
    {
        SurfaceManager.Instance.Register(this);
    }
    void OnDisable()
    {
        SurfaceManager.Instance.Unregister(this);
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.matrix = transform.localToWorldMatrix;
        Gizmos.DrawWireCube(Vector3.zero, new Vector3(_bounds.x * 2f, 0f, _bounds.y * 2f));
    }

    public bool Raycast(Ray ray, float maxDistance, out Vector3 point, bool restrict = false, bool clamp = false)
    {
        if (Ignored)
        {
            point = Vector2.zero;
            return false;
        }

        Plane plane = new(transform.up, transform.position);

        if (plane.Raycast(ray, out float distance) && distance < maxDistance)
        {
            point = ray.origin + ray.direction * distance;
            Vector3 offset = point - transform.position;

            Vector2 pos = new(Vector3.Dot(offset, transform.right), Vector3.Dot(offset, transform.forward));
            Vector2 bounds = _bounds;// new(transform.localScale.x / 2f, transform.localScale.y / 2f);

            if (restrict && (Mathf.Abs(pos.x) > bounds.x || Mathf.Abs(pos.y) > bounds.y))
                return false;


            if (clamp)
            {
                pos.x = Mathf.Clamp(pos.x, -bounds.x, bounds.x);
                pos.y = Mathf.Clamp(pos.y, -bounds.y, bounds.y);

                point = transform.position + transform.right * pos.x + transform.forward * pos.y;
            }


            return true;
        }

        point = Vector3.zero;
        return false;
    }


    public void Attatch(Pickable pickable)
    {
        Pickables.Add(pickable);
    }

    public void Deattatch(Pickable pickable)
    {
        Pickables.Remove(pickable);
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DemoPickableController : MonoBehaviour
{
    public float BaseElevation = 0.3f;

    private LayerMask _pickableColliderMask, _surfaceMask, _pickableMask;


    private DemoPickable _pickable;

    private Vector3 _validPosition;

    void Awake()
    {
        _pickableMask = LayerMask.GetMask("Pickable");
        _pickableColliderMask = LayerMask.GetMask("PickableCollider");
        _surfaceMask = LayerMask.GetMask("Surface");
    }

    void Update()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);


        if (_pickable == null)
        {

            if (InputHandler.GetMouseButtonDown(0) && Physics.Raycast(ray, out RaycastHit hit, float.PositiveInfinity, _pickableMask))
            {
                print(15);

                var pickable = hit.transform.GetComponentInParent<DemoPickable>();

                pickable.Pick();
                _pickable = pickable;
                _validPosition = pickable.transform.position;
            }

        }
        else
        {
            if (InputHandler.GetMouseButtonDown(1))
            {
                _pickable.transform.position = _validPosition;
                _pickable.Drop();
                _pickable = null;
                return;
            }

            if (Physics.Raycast(ray, out RaycastHit hit, float.PositiveInfinity, _surfaceMask))
            {
                _pickable.transform.position = hit.point;


                Collider[] buffer = new Collider[1];
                if (Physics.OverlapSphereNonAlloc(hit.point, _pickable.Radius, buffer, _pickableColliderMask) > 0)
                {
                    Collider collider = buffer[0];
                    DemoPickable pickable = collider.transform.GetComponentInParent<DemoPickable>();
                    _pickable.Elevation = BaseElevation + pickable.Height;
                }
                else
                {
                    _pickable.Elevation = BaseElevation;
                    _validPosition = hit.point;
                }
            }

        }
    }

    private void OnDrawGizmos()
    {
        if (!Application.isPlaying) return;

        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(_validPosition, 0.1f);
    }
}

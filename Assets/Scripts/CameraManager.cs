using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    public static CameraManager _inst;

    [field: SerializeField] public Transform CurrentPivot { get; private set; }
    [field: SerializeField] public Area CurrentArea { get; private set; }

    public bool CanPickArea { get; set; } = true;


    [SerializeField] private float _moveSensitivity = 5.0f;
    [SerializeField] private float _zoomSensitivity = 5.0f;


    [SerializeField] private float _positionLerp = 10f;

    [SerializeField] private float _minZoom;
    [SerializeField] private float _maxZoom;

    private float _rotateX, _rotateY, _zoom;

    private Transform _camera;

    void Awake()
    {
        _inst = this;
        _zoom = (_minZoom + _maxZoom)/2.0f;
        _camera = GetComponentInChildren<Camera>().transform;
    }

    void Update()
    {
        if (Input.GetMouseButton(3))
        {
            _rotateX += Input.GetAxis("Mouse Y") * _moveSensitivity;
            _rotateY -= Input.GetAxis("Mouse X") * _moveSensitivity;

            transform.eulerAngles = new Vector3(_rotateX, _rotateY, 0);
        }
        transform.position = Vector3.Lerp(transform.position, CurrentPivot.position, Time.deltaTime * _positionLerp);

        _zoom -= Input.GetAxis("Mouse ScrollWheel")*_zoomSensitivity;
        _zoom = Mathf.Clamp(_zoom, _minZoom, _maxZoom);
        _camera.transform.localPosition = -Vector3.forward * _zoom;


        // выбор области
        if (CanPickArea && Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (!Physics.Raycast(ray, out RaycastHit hit, float.PositiveInfinity)) return;

            Area area = hit.collider.gameObject.GetComponent<Area>();

            if (area == null) return;

            SetArea(area);
        }
    }

    public void SetArea(Area area)
    {
        CurrentArea.gameObject.SetActive(true);

        CurrentArea = area;
        CurrentPivot = area.Pivot;
        if (area.State != null) StateManager._inst.ChangeState(area.State);

        CurrentArea.gameObject.SetActive(false);
    }

    public void SetPivot(Transform pivot)
    {
        CurrentPivot = pivot;
        // transform.position = pivot.position;
    }

    public void ResetPivot()
    {
        CurrentPivot = CurrentArea.Pivot;
    }
}

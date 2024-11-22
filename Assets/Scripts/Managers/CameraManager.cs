using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    public enum CameraMode
    {
        Orbit, Translate
    }


    public static CameraManager _inst;

    [field: SerializeField] public Transform CurrentPivot { get; private set; }
    [field: SerializeField] public Area CurrentArea { get; private set; }

    public bool CanPickArea { get; set; } = true;


    [SerializeField] private float _moveSensitivity = 5.0f;
    [SerializeField] private float _zoomSensitivity = 5.0f;


    [SerializeField] private float _positionLerp = 10f;
    [SerializeField] private float _rotationLerp = 10f;

    [SerializeField] private float _minZoom;
    [SerializeField] private float _maxZoom;

    private float _cameraX, _cameraY, _zoom;

    [SerializeField] private Transform _camera;

    private Transform _target;
    private CameraMode _cameraMode;


    [SerializeField] private float _transitionLerpMult = 0.5f;
    [SerializeField] private float _transitionTime = 1f;
    
    private float _transitionTimer;

    void Awake()
    {
        _inst = this;
        _zoom = (_minZoom + _maxZoom) / 2.0f;
        _target = transform.GetChild(0);
    }

    void Start()
    {
        SetArea(CurrentArea);
    }

    void Update()
    {
        _transitionTimer -= Time.deltaTime;

        if (Input.GetMouseButton(2))
        {
            _cameraX -= Input.GetAxis("Mouse X") * _moveSensitivity * Time.deltaTime;
            _cameraY += Input.GetAxis("Mouse Y") * _moveSensitivity * Time.deltaTime;
        }

        _zoom -= Input.GetAxis("Mouse ScrollWheel") * _zoomSensitivity * Time.deltaTime;
        _zoom = Mathf.Clamp(_zoom, _minZoom, _maxZoom);

        if (_cameraMode == CameraMode.Orbit)
        {
            _target.localPosition = -Vector3.forward * _zoom;
            transform.localEulerAngles = new Vector3(_cameraY, _cameraX, 0f) * 50f;
        }
        else
        {
            _target.localPosition = -Vector3.forward * _zoom - Vector3.up * _cameraY;
            _target.localEulerAngles = new Vector3(0f, _cameraX, 0f) * 50f;
        }

        // интерполяция
        float positionLerp = _transitionTimer < 0 ? _positionLerp : _positionLerp*_transitionLerpMult;
        float rotationLerp = _transitionTimer < 0 ? _rotationLerp : _rotationLerp*_transitionLerpMult;

        _camera.position = Vector3.Lerp(_camera.position, _target.position, Time.deltaTime * positionLerp);

        // _camera.position = _cameraMode == CameraMode.Orbit ? Vector3.Slerp(_camera.position, _target.position, Time.deltaTime * positionLerp) : Vector3.Lerp(_camera.position, _target.position, Time.deltaTime * positionLerp);
        _camera.rotation = Quaternion.Lerp(_camera.rotation, _target.rotation, Time.deltaTime * rotationLerp);

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
        SetPivot(area.Pivot);
        if (area.State != null) StateManager._inst.ChangeState(area.State);
        _cameraMode = area.CameraMode;

        CurrentArea.gameObject.SetActive(false);

        _cameraX = 0;
        _cameraY = 0;
        _zoom = (_minZoom + _maxZoom) / 2.0f;
        _target.localEulerAngles = Vector3.zero;
        _transitionTimer = _transitionTime;
    }

    public void SetPivot(Transform pivot)
    {
        CurrentPivot = pivot;
        transform.position = pivot.position;
        transform.rotation = pivot.rotation;
    }

    public void ResetPivot()
    {
        CurrentPivot = CurrentArea.Pivot;
    }
}

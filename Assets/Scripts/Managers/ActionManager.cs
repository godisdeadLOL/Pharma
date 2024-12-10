using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ActionManager : MonoBehaviour
{
    [SerializeField]
    private UiContextMenu _contextMenu;

    [SerializeField]
    private LayerMask _pickableLayer;

    public static ActionManager _inst;

    private GameObject _pickable;
    private Action[] _currentProfile;

    private bool _delay;

    void Awake()
    {
        _inst = this;
    }

    void Start()
    {
        _contextMenu.gameObject.SetActive(false);
    }

    void LateUpdate()
    {
        // float interactionDistance = CameraManager._inst.InteractionDistance;

        // if (_currentProfile == null) return;

        // for (int i = 0; i < _currentProfile.Length; i++)
        // {
        //     Action action = _currentProfile[i];

        //     if (Input.GetKeyDown(_keyCodes[i]) && action.CanActivate(_pickable))
        //     {
        //         action.Activate(_pickable);
        //         break;
        //     }
        // }

        // if (InputHandler.GetMouseButtonDown(1))
        // {
        //     Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        //     if (!Physics.Raycast(ray, out RaycastHit hit, interactionDistance, _pickableLayer)) return;

        //     Actions actions = hit.rigidbody.gameObject.GetComponent<Actions>();
        //     if (actions == null) return;

        //     _contextMenu.gameObject.SetActive(true);

        //     var rectTransform = (RectTransform)_contextMenu.transform;
        //     rectTransform.anchoredPosition = Input.mousePosition;

        //     _contextMenu.GenerateButtons(actions.profile, actions.gameObject);
        // }
        // else if (Input.anyKeyDown && !EventSystem.current.IsPointerOverGameObject())
        // {
        //     _contextMenu.gameObject.SetActive(false);
        // }

        if (Input.anyKeyDown && !EventSystem.current.IsPointerOverGameObject() && !_delay) Hide();

        _delay = false;
    }

    public void Display(GameObject pickable, Action[] profile)
    {
        _pickable = pickable;
        _currentProfile = profile;

        _contextMenu.gameObject.SetActive(true);
        var rectTransform = (RectTransform)_contextMenu.transform;
        rectTransform.anchoredPosition = Input.mousePosition;

        _contextMenu.GenerateButtons(profile, pickable);

        _delay = true;
    }

    public void Hide()
    {
        _contextMenu.gameObject.SetActive(false);
        _pickable = null;
        _currentProfile = null;
    }
}

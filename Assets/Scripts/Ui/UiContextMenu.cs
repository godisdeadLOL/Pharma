using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UiContextMenu : MonoBehaviour
{
    [SerializeField]
    private float _padding;

    private RectTransform _buttonTemplate;
    private RectTransform _container;

    void Awake()
    {
        _buttonTemplate = (RectTransform)transform.Find("template");
        _container = (RectTransform)transform;
    }

    private void Clear()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            GameObject child = transform.GetChild(i).gameObject;
            if (child != _buttonTemplate.gameObject) Destroy(child.gameObject);
        }
    }

    public void GenerateButtons(Action[] profile, GameObject pickable)
    {
        Clear();

        float buttonHeight = _buttonTemplate.sizeDelta.y;

        _container.sizeDelta = new Vector3(_container.sizeDelta.x, _padding * 2 + (buttonHeight + _padding / 2) * profile.Length);

        for (int i = 0; i < profile.Length; i++)
        {
            Action action = profile[i];

            RectTransform buttonTransform = Instantiate(_buttonTemplate, transform);
            buttonTransform.gameObject.SetActive(true);

            buttonTransform.anchoredPosition = new Vector2(buttonTransform.anchoredPosition.x, -(_padding + buttonHeight / 2 + (buttonHeight + _padding / 2) * i));

            Button button = buttonTransform.GetComponent<Button>();
            button.GetComponentInChildren<TMP_Text>().text = profile[i].Name;
            button.onClick.AddListener(() => action.Activate(pickable));
        }
    }
}

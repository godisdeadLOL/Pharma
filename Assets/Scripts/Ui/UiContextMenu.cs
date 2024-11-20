using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    void Start()
    {
        int amount = 5;
        float buttonHeight = _buttonTemplate.sizeDelta.y;

        _container.sizeDelta = new Vector3(_container.sizeDelta.x, _padding * 2 + (buttonHeight + _padding / 2) * amount);

        for (int i = 0; i < amount; i++)
        {
            RectTransform buttonTransform = Instantiate(_buttonTemplate, transform);
            buttonTransform.gameObject.SetActive(true);

            buttonTransform.anchoredPosition = new Vector2(buttonTransform.anchoredPosition.x, -(_padding + buttonHeight / 2 + (buttonHeight + _padding / 2) * i) );
        }
    }
}

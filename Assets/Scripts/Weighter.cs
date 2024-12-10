using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Weighter : MonoBehaviour
{
    [SerializeField] private TMP_Text _text;

    private float _total = 0f;
    private float _offset = 0f;

    private bool _on = false;

    private GameObject _weighterTrigger;

    void Awake()
    {
        _weighterTrigger = transform.Find("WeighterTrigger").gameObject;
    }

    void Start()
    {
        UpdateDisplay();

        _on = true;
        Toggle();
    }

    void FixedUpdate()
    {
        _total = 0f;
    }

    void OnTriggerStay(Collider collider)
    {
        if (collider.attachedRigidbody.TryGetComponent<Pickable>(out var pickable))
            _total += CalculatePickableMass(pickable);
    }

    void Update()
    {
        if (_on) UpdateDisplay();
    }

    private float CalculatePickableMass(Pickable pickable)
    {
        if (!pickable.Picked) return 0.2f;
        else return 0f;
    }

    private void UpdateDisplay()
    {
        _text.text = $"{_total - _offset} кг";
    }

    public void ResetWeight()
    {
        if(!_on) return;

        _offset = _total;
    }
    public void Toggle()
    {
        _on = !_on;

        _text.gameObject.SetActive(_on);
        _weighterTrigger.SetActive(_on);
    }
}

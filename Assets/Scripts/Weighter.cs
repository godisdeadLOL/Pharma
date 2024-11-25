using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Weighter : MonoBehaviour
{
    [SerializeField] private TMP_Text _text;

    private float _total = 0f;

    private float _offset = 0f;

    void Start()
    {
        UpdateDisplay();
    }

    void FixedUpdate()
    {
        _total = 0f;
    }

    void OnTriggerStay(Collider collider)
    {
        _total += CalculatePickableMass(collider.attachedRigidbody.gameObject);
    }

    void Update()
    {
        UpdateDisplay();
    }

    private float CalculatePickableMass(GameObject pickable)
    {
        float total = 0.2f;

        var container = pickable.GetComponent<Container>();
        if(container == null) return total;

        total += container.GetMass();

        return total;
    }

    private void UpdateDisplay()
    {
        _text.text = $"{_total-_offset} кг";
    }

    public void ResetWeight()
    {
        _offset = _total;
    }
}

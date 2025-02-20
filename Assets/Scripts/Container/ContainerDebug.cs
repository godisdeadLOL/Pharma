using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(Container))]
public class ContainerDebug : MonoBehaviour
{
    [SerializeField]
    private TMP_Text _text;

    private Container _container;

    void Awake()
    {
        _container = GetComponent<Container>();
    }

    void Update()
    {
        float total = _container.GetAmount();
        string text = $"Всего: {total}/{_container.volume} u\n";

        foreach (ContainerSolution solution in _container.solutions)
        {
            text += $"{solution.ingredientData.name} - {solution.Amount} u\n";
        }

        _text.text = text;
    }
}

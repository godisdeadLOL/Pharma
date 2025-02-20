using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ShelfBottlesGenerator : MonoBehaviour
{
    [SerializeField] private Transform _pivots;
    [SerializeField] private GameObject _bottlePrefab;

    [SerializeField] private IngredientData[] _ingredients;

    void Start()
    {
        for (int i = 0; i < _ingredients.Length; i++)
        {
            Transform pivot = transform.GetChild(transform.childCount - 1 - i);
            IngredientData ingredientData = _ingredients[i];

            GameObject bottle = Instantiate(_bottlePrefab, pivot);
            bottle.SetActive(true);
            bottle.transform.SetLocalPositionAndRotation(Vector3.zero, Quaternion.identity);

            bottle.GetComponent<Container>().AddSolution(new ContainerSolution(ingredientData, 20));
            bottle.transform.Find("Label").GetComponent<TMP_Text>().text = ingredientData.name;
            bottle.transform.Find("Label").GetComponent<CurvedText>().Redraw();

        }
    }
}

using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ShelfTextGenerator : MonoBehaviour
{
    [SerializeField] private string[] _texts;

    void Start()
    {
        var target = transform.Find("to");
        var textPrefab = GetComponentInChildren<TMP_Text>();

        Vector3 from = transform.position;
        Vector3 to = target.position;

        for (int i = 0; i < _texts.Length; i++)
        {
            var text = _texts[i];

            float t = (float)i / (_texts.Length - 1);
            Vector3 pos = Vector3.Lerp(from, to, t);

            Instantiate(textPrefab, pos, Quaternion.identity, transform).GetComponent<TMP_Text>().text = text;
        }

        Destroy(this);
        Destroy(target.gameObject);
        Destroy(textPrefab.gameObject);
    }
}

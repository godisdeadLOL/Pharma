using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CabientAnchorsGenerator : MonoBehaviour
{
    [SerializeField] private int _rowCount = 2;

    void Awake()
    {
        Transform[] _anchors = transform.GetComponentsInChildren<Transform>();

        for (int y = 0; y < _anchors.Length / 2; y++)
        {
            Transform fromAnchor = _anchors[1 + y * 2];
            Transform toAnchor = _anchors[1 + y * 2 + 1];
            
            for (int x = 1; x <= _rowCount - 2; x++)
            {
                float t = (float)x / (_rowCount - 2 + 1);

                GameObject anchor = new GameObject();
                anchor.transform.parent = transform;
                anchor.transform.position = Vector3.Lerp(fromAnchor.position, toAnchor.position, t);
            }

            // Destroy(fromAnchor.gameObject);
            // Destroy(toAnchor.gameObject);
        }

        Destroy(this);
    }
}

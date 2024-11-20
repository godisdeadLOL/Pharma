using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DustEraser : MonoBehaviour
{
    private Rigidbody _rb;

    void Awake()
    {
        _rb = GetComponentInParent<Rigidbody>();
    }

    void OnTriggerStay(Collider collider)
    {
        // 0.04
        float size = collider.transform.localScale.x;
        size -= _rb.velocity.magnitude * 0.05f * Time.deltaTime;

        if (size < 0f) Destroy(collider.gameObject);

        collider.transform.localScale = new Vector3(size, collider.transform.localScale.y, size);
    }
}

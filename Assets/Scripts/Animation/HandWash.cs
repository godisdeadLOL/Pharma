using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class HandWash : MonoBehaviour
    {
    // Start is called before the first frame update
    float movespeed = 15; 
    public GameObject LeftArm;
    public GameObject RightArm;
    bool shouldMove = false;
    float distance = 0.025f;
    float timer = 0;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Transform left = LeftArm.transform;
        Transform right = RightArm.transform;
        // left.position += Vector3.right * Time.deltaTime * movespeed;

        
        if (shouldMove) {
            timer += Time.deltaTime;
            left.transform.localPosition = Vector3.right * Mathf.Sin(timer * movespeed) * distance;
            right.transform.localPosition = Vector3.right * Mathf.Sin(timer * movespeed) * distance*-1;

        }
        if (timer >= 1f)
        {
            shouldMove = false;
            timer = 0;
        }

    }
     void OnMouseDown()
     {
        shouldMove = true;
     }
}

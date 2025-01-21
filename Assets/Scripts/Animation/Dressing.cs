using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class Dressing : MonoBehaviour
{
    public float speed = 100;
    public GameObject PointEnd;
    public GameObject PointTop;
    public GameObject PointStart;
    public GameObject Dress;
    int motion = 0;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float step = speed * Time.deltaTime;
        Transform end = PointEnd.transform;
        Transform start = PointStart.transform;
        Transform top = PointTop.transform;
               
        
        if (motion == 1)
        {
            transform.position = Vector3.MoveTowards(transform.position, top.transform.position, step);
            
        }
        if (transform.position == top.transform.position)
        {
            motion = 2;
            

        }
        if (motion == 2)
        {
            transform.position = Vector3.MoveTowards(transform.position, end.transform.position, step);
        }
        if(transform.position == end.transform.position )
        {
            motion = 3;
            Dress.SetActive(false);
            
        }
        if (motion == 3)
        { 
            transform.position = Vector3.MoveTowards(transform.position, top.transform.position, step);
        }
        if (motion ==3 && transform.position == top.transform.position)
        {
            motion = 4;
        }
        if (motion == 4)
        {
            transform.position = Vector3.MoveTowards(transform.position, start.transform.position, step);
        }
        
        
    }
    void OnMouseDown()
    {
        motion = 1;
        
    }
    void OnTriggerEnter(Collider other)
    {
        

        
    }
}

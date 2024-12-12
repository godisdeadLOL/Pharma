using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class Dressing : MonoBehaviour
{
    public float speed = 1;
    public GameObject PointEnd;
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
        
        
        if (motion == 1)
        {
            transform.position = Vector3.MoveTowards(transform.position, end.transform.position, step);
            
        }
        if(transform.position == end.transform.position )
        {
            motion = 2;
            Dress.SetActive(false);
            
        }
        if (motion == 2)
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

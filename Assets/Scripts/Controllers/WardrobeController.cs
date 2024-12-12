using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WardrobeController : MonoBehaviour
{
    [SerializeField] private Interactable _rag;

    [SerializeField] private float speed = 1;
    [SerializeField] private GameObject PointEnd;
    [SerializeField] private GameObject PointStart;
    [SerializeField] private GameObject Dress;
    int motion = 0;

    void Update()
    {
        float step = speed * Time.deltaTime;
        Transform end = PointEnd.transform;
        Transform start = PointStart.transform;


        if (motion == 1)
        {
            _rag.transform.position = Vector3.MoveTowards(_rag.transform.position, end.transform.position, step);

        }
        if (_rag.transform.position == end.transform.position)
        {
            motion = 2;
            Dress.SetActive(!Dress.activeSelf);

        }
        if (motion == 2)
        {
            _rag.transform.position = Vector3.MoveTowards(_rag.transform.position, start.transform.position, step);
        }


    }

    void OnEnable()
    {
        InteractionManager.Inst.PushInteractable(_rag);
        InteractionManager.Inst.RaycastMask = LayerMask.GetMask("Extra");

        InteractionManager.Inst.OnInteractPrimary += InteractPrimary;
    }

    void OnDisable()
    {
        InteractionManager.Inst.ClearInteractables();

        InteractionManager.Inst.OnInteractPrimary -= InteractPrimary;
    }

    void InteractPrimary(Interactable interactable)
    {
        motion = 1;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SinkController : MonoBehaviour
{
    [SerializeField] private Interactable _vavle;
    [SerializeField] private GameObject LeftArm;
    [SerializeField] private GameObject RightArm;

    [SerializeField] private float movespeed = 15;
    [SerializeField] private float _time = 2f;

    private bool shouldMove = false;
    private float distance = 0.025f;
    private float timer = 0;

    void Update()
    {
        Transform left = LeftArm.transform;
        Transform right = RightArm.transform;
        // left.position += Vector3.right * Time.deltaTime * movespeed;


        if (shouldMove)
        {
            timer += Time.deltaTime;
            left.transform.localPosition = LeftArm.transform.right * Mathf.Sin(timer * movespeed) * distance;
            right.transform.localPosition = LeftArm.transform.right * Mathf.Sin(timer * movespeed) * distance * -1;

        }
        if (timer >= _time)
        {
            shouldMove = false;
            timer = 0;
            LeftArm.SetActive(false);
            RightArm.SetActive(false);
        }

    }

    void OnEnable()
    {
        InteractionManager.Inst.PushInteractable(_vavle);
        InteractionManager.Inst.RaycastMask = LayerMask.GetMask("Extra");

        InteractionManager.Inst.OnInteractPrimary += InteractPrimary;
    }

    void OnDisable()
    {
        InteractionManager.Inst.ClearInteractables();

        InteractionManager.Inst.OnInteractPrimary -= InteractPrimary;
    }

    private void InteractPrimary(Interactable interactable)
    {
        shouldMove = true;
        LeftArm.SetActive(true);
        RightArm.SetActive(true);
    }
}

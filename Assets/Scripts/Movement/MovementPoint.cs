using UnityEngine;
using UnityEngine.Events;

public class MovementPoint : MonoBehaviour
{
    [field: SerializeField] public FocusTarget[] Targets { get; private set; }
}
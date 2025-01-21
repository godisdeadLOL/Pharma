using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class RevertTransferController : MonoBehaviour
{
    /// <summary>
    /// Содержимое забирается из этого контейнера
    /// </summary>
    public Container From { get; set; }

    /// <summary>
    /// Содержимое переносится в этот контейнер
    /// </summary>
    public Container To { get; set; }

    [SerializeField] private float _transferTime = 0.1f;


    [Header("Procedural Animation")]

    [SerializeField] Vector3 _startOffset;
    [SerializeField] float _startRotation = 0;

    [SerializeField] Vector3 _endOffset;
    [SerializeField] float _endRotation = 15;

    [SerializeField] Vector3 _restOffset;
    [SerializeField] float _restRotation = -15;

    [SerializeField] float _restTime = 0.1f;


    private float _transferTimer;
    private float _restTimer;


    void Update()
    {
        if (InputHandler.GetMouseButtonDown(1))
        {
            StateManager._inst.ChangeState<TableController>();
            return;
        }

        bool pressed = InputHandler.GetMouseButton(0);

        // if (pressed) _transferTimer = Mathf.Min(_transferTimer + Time.deltaTime, _transferTimer);
        // else _transferTimer = Mathf.Max(_transferTimer, _transferTimer - Time.deltaTime);

        if (pressed && _transferTimer < _transferTime)
        {
            _transferTimer += Time.deltaTime;
            _restTimer = 0f;

            if (_transferTimer >= _transferTime)
            {
                From.Transfer(To, 1);
                _restTimer = _restTime;
            }
        }
        else if (!pressed)
        {
            _transferTimer = 0f;
            _restTimer -= Time.deltaTime;
        }


        Vector3 targetPosition = From.transform.position + (pressed ? _endOffset : (_restTimer > 0f ? _restOffset : _startOffset));
        Quaternion targetRotation = Quaternion.AngleAxis(pressed ? _endRotation : (_restTimer > 0f ? _restRotation : _startRotation), Vector3.forward);

        To.transform.position = Lerping.Lerp(To.transform.position, targetPosition, Lerping.Smooth.Fast);
        To.transform.rotation = Lerping.Lerp(To.transform.rotation, targetRotation, Lerping.Smooth.Fast);
    }

    void OnEnable()
    {

    }

    void OnDisable()
    {

    }
}

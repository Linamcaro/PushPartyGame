using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MovementType
{
    Rotate,
    XAxis,
    YAxis,
    swing
}
public class MovingObstacle : MonoBehaviour
{

    public MovementType movementType;
    public float speed = 1f;

    [Tooltip("only on X & Y movement")]
    public float movementLenght = 5f;

    private void Awake()
    {
        switch (movementType)
        {
            case MovementType.Rotate:
                transform.DORotate(new Vector3(0, 360, 0), speed * 0.5f, RotateMode.FastBeyond360).SetLoops(-1, LoopType.Restart).SetEase(Ease.Linear);
                break;
            case MovementType.XAxis:
                transform.DOMoveX(movementLenght, speed).SetEase(Ease.InOutQuart).SetLoops(-1, LoopType.Yoyo);
                break;
            case MovementType.YAxis:
                transform.DOMoveX(movementLenght, speed).SetEase(Ease.InOutQuart).SetLoops(-1, LoopType.Yoyo);
                break;
            case MovementType.swing:
                transform.DORotate(new Vector3(0, 90, 0), speed * 0.5f, RotateMode.FastBeyond360).SetLoops(-1, LoopType.Yoyo).SetEase(Ease.Linear);
                break;
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MovementType
{
    Rotate,
    XAxis,
    YAxis,
}
public class MovingObstacle : MonoBehaviour
{

    public MovementType movementType;
    private void Awake()
    {
        switch (movementType)
        {
            case MovementType.Rotate:
                break;
            case MovementType.XAxis:
                break;
            case MovementType.YAxis:
                break;
            default:
                break;
        }
    }
}

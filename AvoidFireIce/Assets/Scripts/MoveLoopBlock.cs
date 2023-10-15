using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MoveLoopBlock : IComparable<MoveLoopBlock>
{
    public float startTime;
    public float playTime;

    public Vector2 moveVector;

    public bool easeIn = false;
    public bool easeOut = false;

    public int CompareTo(MoveLoopBlock other)
    {
        if (other == null)
            return 1;

        return startTime.CompareTo(other.startTime);
    }
}

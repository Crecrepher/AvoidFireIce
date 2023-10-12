using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveLoopBlock : IComparable<MoveLoopBlock>
{
    public float startTime;
    public float playTime;

    public Vector2 startPos;
    public Vector2 endPos;

    
    public int CompareTo(MoveLoopBlock other)
    {
        if (other == null)
            return 1;

        return startTime.CompareTo(other.startTime);
    }
}

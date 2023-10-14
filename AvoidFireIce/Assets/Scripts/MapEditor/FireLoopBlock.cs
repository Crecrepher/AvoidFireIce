using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireLoopBlock : IComparable<RotateLoopBlock>
{
    public float startTime;
    public float playTime;

    public int rate;
    public float speed;
    public int element;

    public int CompareTo(RotateLoopBlock other)
    {
        if (other == null)
            return 1;

        return startTime.CompareTo(other.startTime);
    }
}

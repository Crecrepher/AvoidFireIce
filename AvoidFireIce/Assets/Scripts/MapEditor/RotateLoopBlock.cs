using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateLoopBlock : IComparable<RotateLoopBlock>
{
    public float startTime;
    public float playTime;

    public float rot = 90;

    public bool easeIn = false;
    public bool easeOut = false;

    public int CompareTo(RotateLoopBlock other)
    {
        if (other == null)
            return 1;

        return startTime.CompareTo(other.startTime);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveLoopPlayer : MonoBehaviour
{
    public float loopTime;
    public Vector2 startPos;
    public int currentCount = 0;
    public List<MoveLoopBlock> loopList;
}

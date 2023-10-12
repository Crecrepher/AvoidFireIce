using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveLoopPlayer : MonoBehaviour
{
    public float loopTime;
    public Vector2 startPos;

    public int currentCount = 0;
    public List<MoveLoopBlock> loopList;
    private int loopMaxCount = 0;
    private float timer;

    private Vector2 LerpStart;
    private Vector2 LerpEnd;
    private MoveLoopBlock currMove;

    public void Init()
    {
        currMove = loopList[0];
        currentCount = 0;
        loopMaxCount = loopList.Count;
        timer = 0f;
        LerpStart = currMove.startPos;
        LerpEnd = currMove.endPos;
    }
    private void Update()
    {
        timer += Time.deltaTime;
        if (timer > currMove.startTime && timer < currMove.startTime + currMove.playTime)
        {
            transform.position = Vector2.Lerp(LerpStart, LerpEnd, (timer - currMove.startTime) / currMove.playTime);
        }

        if (timer > currMove.startTime + currMove.playTime)
        {
            if ((currentCount + 1) % loopMaxCount == 0 && timer < loopTime)
            {
                return;
            }
            LerpStart = transform.position;
            currentCount = (currentCount + 1) % loopMaxCount;
            if (currentCount == 0) 
            { 
                timer = 0f; 
                transform.position = startPos;
                LerpStart = transform.position;
            }
            currMove = loopList[currentCount];
            LerpEnd = currMove.endPos;
        }

    }
}

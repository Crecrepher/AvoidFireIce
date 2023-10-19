using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Unity.Collections.AllocatorManager;

public class MoveLoopPlayer : MonoBehaviour
{
    public float loopTime;
    public Vector2 startPos;

    public int currentCount = 0;
    public List<MoveLoopBlock> loopList;
    private int loopMaxCount = 0;
    private float timer;

    private MoveLoopBlock currMove;

    public void Init()
    {
        currMove = loopList[0];
        currentCount = 0;
        loopMaxCount = loopList.Count;
        timer = 0f;
    }
    private void Update()
    {
        timer += Time.deltaTime;

        if (timer > currMove.startTime && timer < currMove.startTime + currMove.playTime)
        {
            float easedSpeed = 1f;
            if (currMove.easeIn && currMove.easeOut)
            {
                easedSpeed = (timer - currMove.startTime) / currMove.playTime < 0.5 ? Mathf.Lerp(0f, 4f, timer - currMove.startTime / currMove.playTime) : easedSpeed = Mathf.Lerp(4f, 0f, timer - currMove.startTime / currMove.playTime);
            }
            else if (currMove.easeIn)
            {
                easedSpeed = Mathf.Lerp(0f, 2f, timer - currMove.startTime / currMove.playTime);
            }
            else if (currMove.easeOut)
            {
                easedSpeed = Mathf.Lerp(2f, 0f, timer - currMove.startTime / currMove.playTime);
            }

            Vector2 moveAmount = currMove.moveVector / currMove.playTime * Time.deltaTime;
            Vector2 easedMove = moveAmount * easedSpeed;
            transform.position = (Vector2)transform.position + easedMove;
        }

        if (timer > currMove.startTime + currMove.playTime)
        {
            if ((currentCount + 1) % loopMaxCount == 0 && timer < loopTime)
            {
                return;
            }
            currentCount = (currentCount + 1) % loopMaxCount;
            if (currentCount == 0) 
            { 
                timer = 0f; 
                transform.position = startPos;
            }
            currMove = loopList[currentCount];
        }

    }
}

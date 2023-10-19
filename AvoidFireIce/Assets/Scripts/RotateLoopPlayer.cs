using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateLoopPlayer : MonoBehaviour
{
    public float loopTime;
    public Quaternion startRot;

    public int currentCount = 0;
    public List<RotateLoopBlock> loopList;
    private int loopMaxCount = 0;
    private float timer;
    private RotateLoopBlock currRot;
    private float rotValue;

    public void Init()
    {
        currRot = loopList[0];
        currentCount = 0;
        loopMaxCount = loopList.Count;
        timer = 0f;
        rotValue = currRot.rot;
    }

    private void Update()
    {
        timer += Time.deltaTime;
        if (timer > currRot.startTime && timer < currRot.startTime + currRot.playTime)
        {
            float easedSpeed = 1f;
            if (currRot.easeIn && currRot.easeOut)
            {
                easedSpeed = (timer - currRot.startTime) / currRot.playTime < 0.5 ? Mathf.Lerp(0f, 4f, timer - currRot.startTime / currRot.playTime) : easedSpeed = Mathf.Lerp(4f, 0f, timer - currRot.startTime / currRot.playTime);
            }
            else if (currRot.easeIn)
            {
                easedSpeed = Mathf.Lerp(0f, 2f, timer - currRot.startTime / currRot.playTime);
            }
            else if (currRot.easeOut)
            {
                easedSpeed = Mathf.Lerp(2f, 0f, timer - currRot.startTime / currRot.playTime);
            }

            transform.Rotate(new Vector3(0, 0, rotValue / currRot.playTime * Time.deltaTime* easedSpeed));
        }

        if (timer > currRot.startTime + currRot.playTime)
        {
            if ((currentCount + 1) % loopMaxCount == 0 && timer < loopTime)
            {
                return;
            }
            currentCount = (currentCount + 1) % loopMaxCount;
            if (currentCount == 0)
            {
                timer = 0f;
                transform.rotation = startRot;
            }
            currRot = loopList[currentCount];
            rotValue = currRot.rot;
        }

    }
}

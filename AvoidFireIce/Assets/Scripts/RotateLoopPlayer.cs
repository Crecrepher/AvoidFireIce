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

    private Quaternion LerpStart;
    private Quaternion LerpEnd;
    private RotateLoopBlock currRot;

    public void Init()
    {
        currRot = loopList[0];
        currentCount = 0;
        loopMaxCount = loopList.Count;
        timer = 0f;
        LerpStart = transform.rotation;
        LerpEnd =  Quaternion.Euler(LerpStart.eulerAngles + new Vector3(0,0, currRot.rot));
    }
    private void Update()
    {
        timer += Time.deltaTime;
        if (timer > currRot.startTime && timer < currRot.startTime + currRot.playTime)
        {
            transform.rotation = Quaternion.Lerp(LerpStart, LerpEnd, (timer - currRot.startTime) / currRot.playTime);
        }

        if (timer > currRot.startTime + currRot.playTime)
        {
            if ((currentCount + 1) % loopMaxCount == 0 && timer < loopTime)
            {
                return;
            }
            LerpStart = transform.rotation;
            currentCount = (currentCount + 1) % loopMaxCount;
            if (currentCount == 0)
            {
                timer = 0f;
                transform.rotation = startRot;
                LerpStart = transform.rotation;
            }
            currRot = loopList[currentCount];
            LerpEnd = Quaternion.Euler(LerpStart.eulerAngles + new Vector3(0, 0, currRot.rot));
        }

    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RayFireLoopPlayer : MonoBehaviour
{
    public float loopTime;
    public int currentCount = 0;
    public List<FireLoopBlock> loopList;
    private int loopMaxCount = 0;
    private float timer;
    private FireLoopBlock currFire;
    private RayTower rayTower;
    private bool isOn= false;

    public void Init()
    {
        rayTower = GetComponent<RayTower>();
        currFire = loopList[0];
        currentCount = 0;
        loopMaxCount = loopList.Count;
        timer = 0f;
        isOn = false;
    }

    private void Update()
    {
        timer += Time.deltaTime;
        if (isOn && timer > currFire.startTime + currFire.playTime)
        {
            rayTower.SetActiveRay(false, currFire.element);
            //isOn = false;
        }
        else if (!isOn && timer > currFire.startTime/* && timer < currFire.startTime + currFire.playTime*/)
        {
            rayTower.SetActiveRay(true, currFire.element);
            isOn = true;
        }
       

        if (timer > currFire.startTime + currFire.playTime)
        {
            if ((currentCount + 1) % loopMaxCount == 0 && timer < loopTime)
            {
                return;
            }
            currentCount = (currentCount + 1) % loopMaxCount;
            if (currentCount == 0)
            {
                timer = 0f;
            }
            currFire = loopList[currentCount];
            isOn = false;
        }

    }
}

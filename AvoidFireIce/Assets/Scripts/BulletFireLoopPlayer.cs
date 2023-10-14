using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletFireLoopPlayer : MonoBehaviour
{
    public float loopTime;
    public int currentCount = 0;
    public List<FireLoopBlock> loopList;
    private int loopMaxCount = 0;
    private float timer;
    private FireLoopBlock currFire;
    private BulletTower bt;
    private float subTimer;
    private float shootDuration;

    public void Init()
    {

        currFire = loopList[0];
        currentCount = 0;
        loopMaxCount = loopList.Count;
        timer = 0f;
        bt = GetComponent<BulletTower>();
        subTimer = 0f;
        shootDuration = currFire.playTime / currFire.rate;
    }

    private void Update()
    {
        timer += Time.deltaTime;
        if (timer > currFire.startTime && timer < currFire.startTime + currFire.playTime && currFire.rate > 0)
        {
            subTimer += Time.deltaTime;
            if (subTimer > shootDuration)
            {
                subTimer = 0f;
                bt.Shoot((Element)currFire.element, currFire.speed);
            }
            
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
            if (currFire.rate > 0)
            {
                bt.Shoot((Element)currFire.element, currFire.speed);
                subTimer = 0f;
                shootDuration = currFire.playTime / currFire.rate;
            }
        }
    }
}

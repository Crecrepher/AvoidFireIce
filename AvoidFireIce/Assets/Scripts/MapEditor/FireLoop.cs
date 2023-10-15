using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class FireLoop
{
    public int initCode = 0;
    public float loopTime = 1f;
    public List<FireLoopBlock> loopList;

    public void CopyTo(FireLoop fl)
    {
        fl = new FireLoop();
        fl.initCode = initCode;
        fl.loopTime = loopTime;
        fl.loopList = loopList.ToList();
    }

    public FireLoop() { }
    public FireLoop(int initCode, float loopTime, List<FireLoopBlock> loopList)
    {
        this.initCode = initCode;
        this.loopTime = loopTime;
        this.loopList = loopList;
    }
}

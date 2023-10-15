using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class RotateLoop
{
    public int initCode = 0;
    public float loopTime = 1f;
    public List<RotateLoopBlock> loopList;

    public void CopyTo(RotateLoop rl)
    {
        rl = new RotateLoop();
        rl.initCode = initCode;
        rl.loopTime = loopTime;
        rl.loopList = loopList.ToList();
    }

    public RotateLoop() { }
    public RotateLoop(int initCode, float loopTime, List<RotateLoopBlock> loopList)
    {
        this.initCode = initCode;
        this.loopTime = loopTime;
        this.loopList = loopList;
    }
}

using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MoveLoop
{
    public int initCode = 0;
    public float loopTime = 1f;
    public List<MoveLoopBlock> loopList;

    public void CopyTo(MoveLoop ml)
    {
        ml = new MoveLoop();
        ml.initCode = initCode;
        ml.loopTime = loopTime;
        ml.loopList = loopList.ToList();
    }
    public MoveLoop() { }
    public MoveLoop(int initCode, float loopTime, List<MoveLoopBlock> loopList)
    {
        this.initCode = initCode;
        this.loopTime = loopTime;
        this.loopList = loopList;
    }
}

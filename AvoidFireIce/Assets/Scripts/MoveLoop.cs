using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveLoop
{
    public int initCode = 0;
    public float loopTime = 1f;
    public List<MoveLoopBlock> loopList;

    public MoveLoop() { }
    public MoveLoop(int initCode, float loopTime, List<MoveLoopBlock> loopList)
    {
        this.initCode = initCode;
        this.loopTime = loopTime;
        this.loopList = loopList;
    }

    private void Awake()
    {
        loopList = new List<MoveLoopBlock>();
    }
}

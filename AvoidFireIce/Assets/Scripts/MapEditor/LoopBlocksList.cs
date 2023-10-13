using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoopBlocksList : MonoBehaviour
{
    public bool isTmeBind = false;
    public List<GameObject> moveLoopBlocks;
    public List<GameObject> rotateLoopBlocks;
    public List<GameObject> shootLoopBlocks;

    private void Awake()
    {
        moveLoopBlocks = new List<GameObject>();
    }
}

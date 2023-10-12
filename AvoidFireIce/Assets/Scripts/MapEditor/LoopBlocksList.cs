using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoopBlocksList : MonoBehaviour
{
    public List<GameObject> moveLoopBlocks;

    private void Awake()
    {
        moveLoopBlocks = new List<GameObject>();
    }
}

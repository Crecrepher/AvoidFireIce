using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalData : MonoBehaviour
{
    public static GlobalData instance
    {
        get
        {
            if (globalData == null)
            {
                globalData = FindObjectOfType<GlobalData>();
            }
            return globalData;
        }
    }

    private static GlobalData globalData;

    public bool isBGOn = true;
}

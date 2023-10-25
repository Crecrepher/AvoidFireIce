using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveDataListInfo : MonoBehaviour
{
    public int num = 0;
    public GameObject VerifyIcon;
    public void IsVerified(bool on)
    {
		VerifyIcon.SetActive(on);
	}
}

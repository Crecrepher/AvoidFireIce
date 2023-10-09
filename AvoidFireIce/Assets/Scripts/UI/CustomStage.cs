using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomStage : MonoBehaviour
{
    private List<string> files;

    public GameObject FileFreefab;
    public GameObject ListArea;
    public void Showlists()
    {
        var lists = GameObject.FindGameObjectsWithTag("CustomFileList");
        foreach (var list in lists) 
        { 
            Destroy(list);
        }
    }

    public void CreateNew()
    {

    }

    public void Open()
    {

    }

    public void Delete()
    {

    }
}

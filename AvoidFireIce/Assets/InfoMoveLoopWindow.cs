using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;
using UnityEngine.UI;
using static Unity.Collections.AllocatorManager;
using static UnityEngine.CullingGroup;


public class InfoMoveLoopWindow : MonoBehaviour
{
    public GameObject Window;
    public List<TMP_InputField> InputBoxes;
    public GameObject MoveMarker;
    public GameObject MoveLoopLine;
    public GameObject MapLeftBottom;

    private GameObject currentObject;
    private MoveLoopData ml;
    private GameObject button;
    private int index;

    private void Start()
    {
        InputBoxes[0].onEndEdit.AddListener(StartTimeChanged);
        InputBoxes[1].onEndEdit.AddListener(PlayTimeChanged);
        InputBoxes[2].onEndEdit.AddListener(PosXChanged);
        InputBoxes[3].onEndEdit.AddListener(PosYChanged);
        InputBoxes[4].onEndEdit.AddListener(VecXChanged);
        InputBoxes[5].onEndEdit.AddListener(VecYChanged);
    }

    public void OpenWindow(GameObject button, GameObject currentObject, int index)
    {
        ml = currentObject.GetComponent<MoveLoopData>();
        this.currentObject = currentObject;
        this.button = button;
        this.index = index;
        Window.SetActive(true);
        MoveMarker.SetActive(true);
        InputBoxes[0].text = ml.ml.loopList[index].startTime.ToString();
        InputBoxes[1].text = ml.ml.loopList[index].playTime.ToString();
        InputBoxes[2].text = (ml.ml.loopList[index].endPos.x - MapLeftBottom.transform.position.x).ToString();
        InputBoxes[3].text = (ml.ml.loopList[index].endPos.y - MapLeftBottom.transform.position.y).ToString();
        InputBoxes[4].text = (ml.ml.loopList[index].endPos.x - currentObject.transform.position.x).ToString();
        InputBoxes[5].text = (ml.ml.loopList[index].endPos.y - currentObject.transform.position.y).ToString();
    }

    public void CloseWindow()
    {
        Window.SetActive(false);
        MoveMarker.SetActive(false);
    }

    public void StartTimeChanged(string newValue)
    {
        if (ml.ml.loopList[index] != null)
        {
            float value;
            if (float.TryParse(newValue, out value))
            {
                float minTime = index == 0 ? 0f : ml.ml.loopList[index - 1].startTime + ml.ml.loopList[index - 1].playTime;
                if (value < minTime)
                {
                    value = minTime;
                }

                float maxTime = index < ml.ml.loopList.Count - 1 ? ml.ml.loopList[index + 1].startTime : ml.ml.loopTime;
                if (value + ml.ml.loopList[index].playTime > maxTime)
                {
                    // value = maxTime - mlb.playTime;
                }
                mlb.startTime = value;
                RectTransform rect = button.GetComponent<RectTransform>();
                rect.position = new Vector2(MoveLoopLine.transform.position.x + mlb.startTime * 50, button.transform.position.y);
            }
        }
    }

    public void PlayTimeChanged(string newValue)
    {
        if (mlb != null)
        {
            float value;
            if (float.TryParse(newValue, out value))
            {
                if (value < 0)
                {
                    value = 0;
                }
                if (value + mlb.startTime > maxTime)
                {
                    value = maxTime - mlb.startTime;
                }
                mlb.startTime = value;
                RectTransform rect = button.GetComponent<RectTransform>();
                rect.sizeDelta = new Vector2(mlb.playTime * 50, rect.rect.height);
            }
        }
    }

    public void PosXChanged(string newValue)
    {
        if (mlb != null)
        {
            float value;
            if (float.TryParse(newValue, out value))
            {
                mlb.endPos.x = value + MapLeftBottom.transform.position.x;
                InputBoxes[4].text = (mlb.endPos.x - startPos.x).ToString();
            }
        }
    }

    public void PosYChanged(string newValue)
    {
        if (mlb != null)
        {
            float value;
            if (float.TryParse(newValue, out value))
            {
                mlb.endPos.y = value + MapLeftBottom.transform.position.y;
                InputBoxes[5].text = (mlb.endPos.y - startPos.y).ToString();
            }
        }
    }

    public void VecXChanged(string newValue)
    {
        if (mlb != null)
        {
            float value;
            if (float.TryParse(newValue, out value))
            {
                mlb.endPos.x = value + startPos.x;
                InputBoxes[2].text = (mlb.endPos.x - mlb.startPos.x).ToString();
            }
        }
    }

    public void VecYChanged(string newValue)
    {
        if (mlb != null)
        {
            float value;
            if (float.TryParse(newValue, out value))
            {
                mlb.endPos.y = value + startPos.y;
                InputBoxes[3].text = (mlb.endPos.y - mlb.startPos.y).ToString();
            }
        }
    }
}

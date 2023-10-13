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
    public GameObject MainLoopInfo;

    private GameObject currentObject;
    private MoveLoopData ml;
    private GameObject button;
    private Color pastColor;
    public int index;

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
        if (this.button != null)
        {
            this.button.GetComponent<Image>().color = pastColor;
        }
        ml = currentObject.GetComponent<MoveLoopData>();
        this.currentObject = currentObject;
        this.button = button;
        pastColor = this.button.GetComponent<Image>().color;
        this.button.GetComponent<Image>().color = Color.yellow;
        this.index = index;
        Window.SetActive(true);
        MoveMarker.SetActive(true);
        InputBoxes[0].text = ml.ml.loopList[index].startTime.ToString();
        InputBoxes[1].text = ml.ml.loopList[index].playTime.ToString();
        InputBoxes[2].text = (ml.ml.loopList[index].endPos.x - MapLeftBottom.transform.position.x).ToString();
        InputBoxes[3].text = (ml.ml.loopList[index].endPos.y - MapLeftBottom.transform.position.y).ToString();
        Vector2 startPos = index == 0 ? currentObject.transform.position : ml.ml.loopList[index - 1].endPos;
        InputBoxes[4].text = (ml.ml.loopList[index].endPos.x - startPos.x).ToString();
        InputBoxes[5].text = (ml.ml.loopList[index].endPos.y - startPos.y).ToString();
        MainLoopInfo.SetActive(false);
    }

    public void CloseWindow()
    {
        Window.SetActive(false);
        MoveMarker.SetActive(false);
    }
    public void Release()
    {
        if (button != null)
        {
            button.GetComponent<Image>().color = pastColor;
        }
        button = null;
        index = -1;
        Window.SetActive(false);
        MoveMarker.SetActive(false);
        MainLoopInfo.SetActive(false);
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
                    value = maxTime - ml.ml.loopList[index].playTime;
                }
                ml.ml.loopList[index].startTime = value;
                RectTransform rect = button.GetComponent<RectTransform>();
                rect.position = new Vector2(MoveLoopLine.transform.position.x + ml.ml.loopList[index].startTime * 50f, button.transform.position.y);
                InputBoxes[0].text = ml.ml.loopList[index].startTime.ToString();
            }
        }
    }

    public void PlayTimeChanged(string newValue)
    {
        if (ml.ml.loopList[index] != null)
        {
            float value;
            if (float.TryParse(newValue, out value))
            {
                if (value < 0)
                {
                    value = 0;
                }

                float maxTime = index < ml.ml.loopList.Count - 1 ? ml.ml.loopList[index + 1].startTime : ml.ml.loopTime;
                if (value + ml.ml.loopList[index].startTime > maxTime)
                {
                    value = maxTime - ml.ml.loopList[index].startTime;
                }
                ml.ml.loopList[index].playTime = value;
                RectTransform rect = button.GetComponent<RectTransform>();
                rect.sizeDelta = new Vector2(ml.ml.loopList[index].playTime * 50, rect.rect.height);
                button.GetComponent<RectTransform>().sizeDelta = rect.sizeDelta;
                InputBoxes[1].text = ml.ml.loopList[index].playTime.ToString();
            }
        }
    }

    public void PosXChanged(string newValue)
    {
        if (ml.ml.loopList[index] != null)
        {
            float value;
            Vector2 startPos = index == 0 ? currentObject.transform.position : ml.ml.loopList[index - 1].endPos;
            if (float.TryParse(newValue, out value))
            {
                ml.ml.loopList[index].endPos.x = value + MapLeftBottom.transform.position.x;
                InputBoxes[4].text = (ml.ml.loopList[index].endPos.x - (value + MapLeftBottom.transform.position.x)).ToString();
                InputBoxes[2].text = (ml.ml.loopList[index].endPos.x - MapLeftBottom.transform.position.x).ToString();
            }
        }
    }

    public void PosYChanged(string newValue)
    {
        if (ml.ml.loopList[index] != null)
        {
            float value;
            Vector2 startPos = index == 0 ? currentObject.transform.position : ml.ml.loopList[index - 1].endPos;
            if (float.TryParse(newValue, out value))
            {
                ml.ml.loopList[index].endPos.y = value + MapLeftBottom.transform.position.y;
                InputBoxes[5].text = (ml.ml.loopList[index].endPos.y - startPos.y).ToString();
                InputBoxes[3].text = (ml.ml.loopList[index].endPos.y - MapLeftBottom.transform.position.y).ToString();
            }
        }
    }

    public void VecXChanged(string newValue)
    {
        if (ml.ml.loopList[index] != null)
        {
            float value;
            Vector2 startPos = index == 0 ? currentObject.transform.position : ml.ml.loopList[index - 1].endPos;
            if (float.TryParse(newValue, out value))
            {
                ml.ml.loopList[index].endPos.x = value + startPos.x;
                InputBoxes[2].text = (ml.ml.loopList[index].endPos.x - MapLeftBottom.transform.position.x).ToString();
                InputBoxes[4].text = (ml.ml.loopList[index].endPos.x - startPos.x).ToString();
            }
        }
    }

    public void VecYChanged(string newValue)
    {
        if (ml.ml.loopList[index] != null)
        {
            float value;
            Vector2 startPos = index == 0 ? currentObject.transform.position : ml.ml.loopList[index - 1].endPos;
            if (float.TryParse(newValue, out value))
            {
                ml.ml.loopList[index].endPos.y = value + startPos.y;
                InputBoxes[3].text = (ml.ml.loopList[index].endPos.y - MapLeftBottom.transform.position.y).ToString();
                InputBoxes[5].text = (ml.ml.loopList[index].endPos.y - startPos.y).ToString();
            }
        }
    }
}

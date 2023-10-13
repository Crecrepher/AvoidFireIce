using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements.Experimental;

public class InfoRotateLoopWindow : MonoBehaviour
{
    public GameObject Window;
    public List<TMP_InputField> InputBoxes;
    public Toggle EaseIn;
    public Toggle EaseOut;
    public GameObject RotateMarker;
    public GameObject RotateLoopLine;
    public GameObject MapLeftBottom;
    public GameObject MainLoopInfo;

    private GameObject currentObject;
    private RotateLoopData rl;
    private GameObject button;
    private Color pastColor;
    public int index;

    private void Start()
    {
        InputBoxes[0].onEndEdit.AddListener(StartTimeChanged);
        InputBoxes[1].onEndEdit.AddListener(PlayTimeChanged);
        InputBoxes[2].onValueChanged.AddListener(RotChanged);
    }

    public void OpenWindow(GameObject button, GameObject currentObject, int index)
    {
        if (this.button != null)
        {
            this.button.GetComponent<Image>().color = pastColor;
        }
        rl = currentObject.GetComponent<RotateLoopData>();
        this.currentObject = currentObject;
        this.button = button;
        pastColor = this.button.GetComponent<Image>().color;
        this.button.GetComponent<Image>().color = Color.yellow;
        this.index = index;
        Window.SetActive(true);
        RotateMarker.SetActive(true);
        InputBoxes[0].text = rl.rl.loopList[index].startTime.ToString();
        InputBoxes[1].text = rl.rl.loopList[index].playTime.ToString();
        InputBoxes[2].text = rl.rl.loopList[index].rot.ToString();
        EaseIn.isOn = rl.rl.loopList[index].easeIn;
        EaseOut.isOn = rl.rl.loopList[index].easeOut;
        MainLoopInfo.SetActive(false);
    }

    public void CloseWindow()
    {
        Window.SetActive(false);
        RotateMarker.SetActive(false);
    }

    public void Release()
    {
        if (this.button != null)
        {
            this.button.GetComponent<Image>().color = pastColor;
        }
        button = null;
        index = -1;
        Window.SetActive(false);
        RotateMarker.SetActive(false);
        MainLoopInfo.SetActive(false);
    }

    public void StartTimeChanged(string newValue)
    {
        if (rl.rl.loopList[index] != null)
        {
            float value;
            if (float.TryParse(newValue, out value))
            {
                float minTime = index == 0 ? 0f : rl.rl.loopList[index - 1].startTime + rl.rl.loopList[index - 1].playTime;
                if (value < minTime)
                {
                    value = minTime;
                }

                float maxTime = index < rl.rl.loopList.Count - 1 ? rl.rl.loopList[index + 1].startTime : rl.rl.loopTime;
                if (value + rl.rl.loopList[index].playTime > maxTime)
                {
                    value = maxTime - rl.rl.loopList[index].playTime;
                }
                rl.rl.loopList[index].startTime = value;
                RectTransform rect = button.GetComponent<RectTransform>();
                rect.position = new Vector2(RotateLoopLine.transform.position.x + rl.rl.loopList[index].startTime * 50f, button.transform.position.y);
                InputBoxes[0].text = rl.rl.loopList[index].startTime.ToString();
            }
        }
    }

    public void PlayTimeChanged(string newValue)
    {
        if (rl.rl.loopList[index] != null)
        {
            float value;
            if (float.TryParse(newValue, out value))
            {
                if (value < 0)
                {
                    value = 0;
                }

                float maxTime = index < rl.rl.loopList.Count - 1 ? rl.rl.loopList[index + 1].startTime : rl.rl.loopTime;
                if (value + rl.rl.loopList[index].startTime > maxTime)
                {
                    value = maxTime - rl.rl.loopList[index].startTime;
                }
                rl.rl.loopList[index].playTime = value;
                RectTransform rect = button.GetComponent<RectTransform>();
                rect.sizeDelta = new Vector2(rl.rl.loopList[index].playTime * 50, rect.rect.height);
                button.GetComponent<RectTransform>().sizeDelta = rect.sizeDelta;
                InputBoxes[1].text = rl.rl.loopList[index].playTime.ToString();
            }
        }
    }

    public void RotChanged(string newValue)
    {
        if (rl.rl.loopList[index] != null)
        {
            float value;
            if (float.TryParse(newValue, out value))
            {
                rl.rl.loopList[index].rot = value;
            }
        }
    }

    public void SwitchEaseIn()
    {
        rl.rl.loopList[index].easeIn = EaseIn.isOn;
    }

    public void SwitchEaseOut()
    {
        rl.rl.loopList[index].easeOut = EaseOut.isOn;
    }
}
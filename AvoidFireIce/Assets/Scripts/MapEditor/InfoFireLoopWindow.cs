using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class InfoFireLoopWindow : MonoBehaviour
{
    public GameObject Window;
    public List<TMP_InputField> InputBoxes;
    public List<Toggle> ElementsToggle;
    public GameObject FireLoopLine;
    public GameObject MapLeftBottom;
    public GameObject MainLoopInfo;

    private GameObject currentObject;
    private FireLoopData fl;
    private GameObject button;
    private Color pastColor;
    public int index;

    private void Start()
    {
        InputBoxes[0].onEndEdit.AddListener(StartTimeChanged);
        InputBoxes[1].onEndEdit.AddListener(PlayTimeChanged);
        InputBoxes[2].onValueChanged.AddListener(RateChanged);
        InputBoxes[3].onValueChanged.AddListener(SpeedChanged);
    }

    public void OpenWindow(GameObject button, GameObject currentObject, int index)
    {
        if (this.button != null)
        {
            this.button.GetComponent<Image>().color = pastColor;
        }
        fl = currentObject.GetComponent<FireLoopData>();
        this.currentObject = currentObject;
        this.button = button;
        pastColor = this.button.GetComponent<Image>().color;
        this.button.GetComponent<Image>().color = Color.yellow;
        this.index = index;
        Window.SetActive(true);
        InputBoxes[0].text = fl.fl.loopList[index].startTime.ToString();
        InputBoxes[1].text = fl.fl.loopList[index].playTime.ToString();
        if (currentObject.GetComponent<MarkerInfo>().ObjectType == (int)ObjectType.BulletTower)
        {
            InputBoxes[2].gameObject.SetActive(true);
            InputBoxes[3].gameObject.SetActive(true);
            InputBoxes[2].text = fl.fl.loopList[index].rate.ToString();
            InputBoxes[3].text = fl.fl.loopList[index].speed.ToString();
        }
        else
        {
            InputBoxes[2].gameObject.SetActive(false);
            InputBoxes[3].gameObject.SetActive(false);
        }
        ElementsToggle[fl.fl.loopList[index].element].isOn = true;
        MainLoopInfo.SetActive(false);
    }

    public void CloseWindow()
    {
        Window.SetActive(false);
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
        MainLoopInfo.SetActive(false);
    }

    public void StartTimeChanged(string newValue)
    {
        if (fl.fl.loopList[index] != null)
        {
            float value;
            if (float.TryParse(newValue, out value))
            {
                float minTime = index == 0 ? 0f : fl.fl.loopList[index - 1].startTime + fl.fl.loopList[index - 1].playTime;
                if (value < minTime)
                {
                    value = minTime;
                }

                float maxTime = index < fl.fl.loopList.Count - 1 ? fl.fl.loopList[index + 1].startTime : fl.fl.loopTime;
                if (value + fl.fl.loopList[index].playTime > maxTime)
                {
                    value = maxTime - fl.fl.loopList[index].playTime;
                }
                fl.fl.loopList[index].startTime = value;
                RectTransform rect = button.GetComponent<RectTransform>();
                rect.position = new Vector2(FireLoopLine.transform.position.x + fl.fl.loopList[index].startTime * 50f, button.transform.position.y);
                InputBoxes[0].text = fl.fl.loopList[index].startTime.ToString();
            }
        }
    }

    public void PlayTimeChanged(string newValue)
    {
        if (fl.fl.loopList[index] != null)
        {
            float value;
            if (float.TryParse(newValue, out value))
            {
                if (value < 0)
                {
                    value = 0;
                }

                float maxTime = index < fl.fl.loopList.Count - 1 ? fl.fl.loopList[index + 1].startTime : fl.fl.loopTime;
                if (value + fl.fl.loopList[index].startTime > maxTime)
                {
                    value = maxTime - fl.fl.loopList[index].startTime;
                }
                fl.fl.loopList[index].playTime = value;
                RectTransform rect = button.GetComponent<RectTransform>();
                rect.sizeDelta = new Vector2(fl.fl.loopList[index].playTime * 50, rect.rect.height);
                button.GetComponent<RectTransform>().sizeDelta = rect.sizeDelta;
                InputBoxes[1].text = fl.fl.loopList[index].playTime.ToString();
            }
        }
    }

    public void RateChanged(string newValue)
    {
        if (fl.fl.loopList[index] != null)
        {
            int value;
            if (int.TryParse(newValue, out value))
            {
                fl.fl.loopList[index].rate = value;
            }
        }
    }

    public void SpeedChanged(string newValue)
    {
        if (fl.fl.loopList[index] != null)
        {
            float value;
            if (float.TryParse(newValue, out value))
            {
                fl.fl.loopList[index].speed = value;
            }
        }
    }

    public void SetElement(int elementCode)
    {
        fl.fl.loopList[index].element = elementCode;
    }
}

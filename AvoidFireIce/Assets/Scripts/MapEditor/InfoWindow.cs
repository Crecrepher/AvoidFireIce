using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.U2D;
using UnityEngine.UI;

public class InfoWindow : MonoBehaviour
{
    public Image sprite;
    public GameObject InfoWindowObject;
    public GameObject InfoWindowElement;
    public GameObject MapLeftBottom;
    public TMP_InputField PosX;
    public TMP_InputField PosY;
    public TMP_InputField Rotate;
    public List<Toggle> ElementsToggle;

    private GameObject selectedObject;

    private void Start()
    {
        PosX.onValueChanged.AddListener(PosXChanged);
        PosY.onValueChanged.AddListener(PosYChanged);
        Rotate.onValueChanged.AddListener(RotateChanged);
    }

    public void OpenWindow(GameObject selected)
    {
        if (selected == null) { return; }
        selectedObject = selected;
        InfoWindowObject.SetActive(true);
        MarkerInfo mi = selectedObject.GetComponent<MarkerInfo>();
        if (mi != null && Defines.instance.isHaveElement(mi.ObjectType))
        {
            InfoWindowElement.SetActive(true);
            ElementsToggle[(int)selectedObject.GetComponent<DangerObject>().element].isOn = true;
        }
        else
        {
            InfoWindowElement.SetActive(false);
        }
        SpriteRenderer spriteInfo = selected.GetComponent<SpriteRenderer>();
        if (spriteInfo != null)
        {
            sprite.sprite = spriteInfo.sprite;
            sprite.color = Color.white;
        }
        PosX.text = (selectedObject.transform.position.x - MapLeftBottom.transform.position.x).ToString();
        PosY.text = (selectedObject.transform.position.y - MapLeftBottom.transform.position.y).ToString();
        Rotate.text = (selectedObject.transform.rotation.eulerAngles.z).ToString();
    }

    public void CloseWindow() 
    {
        InfoWindowObject.SetActive(false);
    }

    public void PosXChanged(string newValue)
    {
        if (selectedObject != null)
        {
            float value;
            if (float.TryParse(newValue, out value))
            {
                Vector3 newPosition = selectedObject.transform.position;
                newPosition.x = value + MapLeftBottom.transform.position.x;
                selectedObject.transform.position = newPosition;
            }
        }
    }

    public void PosYChanged(string newValue)
    {
        if (selectedObject != null)
        {
            float value;
            if (float.TryParse(newValue, out value))
            {
                Vector3 newPosition = selectedObject.transform.position;
                newPosition.y = value + MapLeftBottom.transform.position.y;
                selectedObject.transform.position = newPosition;
            }
        }
    }

    public void RotateChanged(string newValue)
    {
        if (selectedObject != null)
        {
            float value;
            if (float.TryParse(newValue, out value))
            {
                //Quaternion newRotation = selectedObject.transform.rotation;
                Quaternion newRotation = Quaternion.Euler(0f,0f,value);
                selectedObject.transform.rotation = newRotation;
            }
        }
    }

    public void SetElement(int code)
    {
        DangerObject target = selectedObject.GetComponent<DangerObject>();
        target.element = (Element)code;
        target.SetColor();
    }
}

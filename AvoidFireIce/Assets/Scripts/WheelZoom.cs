using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WheelZoom : MonoBehaviour
{
    public float zoomSpeed = 1.0f;
    public float minOrthoSize = 1.0f;
    public float maxOrthoSize = 10.0f;

    void Update()
    {
        float scrollDelta = Input.GetAxis("Mouse ScrollWheel");
        if (scrollDelta != 0.0f)
        {
            float newSize = Mathf.Clamp(Camera.main.orthographicSize - scrollDelta * zoomSpeed, minOrthoSize, maxOrthoSize);
            Camera.main.orthographicSize = newSize;
        }
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WheelZoom : MonoBehaviour
{
	public float zoomSpeed = 1.0f;
	public float minOrthoSize = 1.0f;
	public float maxOrthoSize = 10.0f;

	public bool IsTouching { get; private set; }
	public float ZoomNormal { get; private set; }

	private List<int> fingerIdList = new List<int>();

	void Update()
	{
//#if UNITY_EDITOR || UNITY_STANDALONE
		float scrollDelta = Input.GetAxis("Mouse ScrollWheel");
		if (scrollDelta != 0.0f)
		{
			float newSize = Mathf.Clamp(Camera.main.orthographicSize - scrollDelta * zoomSpeed, minOrthoSize, maxOrthoSize);
			Camera.main.orthographicSize = newSize;
		}

		//#elif UNITY_EDITOR || UNITY_STANDALONE
		foreach (var t in Input.touches)
		{
			switch (t.phase)
			{
				case TouchPhase.Began:
					fingerIdList.Add(t.fingerId);
					break;
				case TouchPhase.Moved:
				case TouchPhase.Stationary:
					break;
				case TouchPhase.Ended:
				case TouchPhase.Canceled:
					fingerIdList.Remove(t.fingerId);
					break;
			}
		}
		if (Input.touchCount >=2)
        {
			UpdatePinchZoom();
		}
    }
	public void UpdatePinchZoom()
	{
		if (fingerIdList.Count >= 2)
		{
			Vector2[] prevFirstTouchPos = new Vector2[2];
			Vector2[] currentTouchPos = new Vector2[2];
			for (int i = 0; i < 2; i++)
			{
				var touch = Array.Find(Input.touches,
					x => x.fingerId == fingerIdList[i]);
				currentTouchPos[i] = touch.position;
				prevFirstTouchPos[i] = touch.position - touch.deltaPosition;
			}
			//prev distance
			var prevFrameDist = Vector2.Distance(prevFirstTouchPos[0], prevFirstTouchPos[1]);

			//CurrFrame Distance
			var currFrameDist = Vector2.Distance(currentTouchPos[0], currentTouchPos[1]);

			float newSize = Mathf.Clamp(Camera.main.orthographicSize - (currFrameDist - prevFrameDist)*0.01f, minOrthoSize, maxOrthoSize);
			Camera.main.orthographicSize = newSize;
		}
	}
//#endif
}


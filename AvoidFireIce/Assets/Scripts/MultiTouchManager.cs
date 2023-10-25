using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MultiTouchManager : MonoBehaviour
{
	public bool IsTouching { get; private set; }

	public float minZoomInch = 0.2f;
	public float maxZoomInch = 0.5f;

	public float minZoomPixel;
	public float maxZoomPixel;

	public float ZoomNormal { get; private set; }

	private List<int> fingerIdList = new List<int>();

	private void Awake()
	{
		minZoomPixel = minZoomInch * Screen.dpi;
		maxZoomPixel = maxZoomInch * Screen.dpi;
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

			var distancePixel = prevFrameDist - currFrameDist;
			//var distanceInch = distancePixel * Screen.dpi;
			//Debug.Log(distanceInch);
		}
	}
	public void Update()
	{
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
	}
}

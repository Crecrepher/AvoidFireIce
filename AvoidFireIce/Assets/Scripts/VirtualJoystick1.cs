using Newtonsoft.Json.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


public class VirtualJoystick2 : MonoBehaviour, IDragHandler, IPointerDownHandler, IPointerUpHandler
{ 
	public Vector2 value { get; private set; }

	private int pointerId;
	private bool isDragging;

	public void OnDrag(PointerEventData eventData)
	{
		throw new System.NotImplementedException();
	}

	public void OnPointerDown(PointerEventData eventData)
	{
		value = Vector2.zero;
	}

	public void OnPointerUp(PointerEventData eventData)
	{
		if (isDragging) { }
		throw new System.NotImplementedException();
	}
}

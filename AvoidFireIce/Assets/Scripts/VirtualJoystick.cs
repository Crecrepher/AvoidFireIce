using Newtonsoft.Json.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


public class VirtualJoystick : MonoBehaviour, IDragHandler, IPointerDownHandler, IPointerUpHandler
{
    public enum Axis
    {
        Horizontal,
        Vertical,
    }

    public Image stick;
    public float radius;

    private Vector3 originalPoint;
	private RectTransform rectTr;

	private Vector3 value;

	private int pointerId;
	private bool isDragging = false;

	public void Start()
	{
		rectTr = GetComponent<RectTransform>();
		originalPoint = stick.rectTransform.position;
	}
	public float GetAxis(Axis axis)
    {
		switch(axis)
		{
			case Axis.Horizontal:
				return value.x;
				case Axis.Vertical: 
				return value.y;
		}
        return 0f;
    }
	public void UpdateStickPos(Vector3 screenPos)
	{
		RectTransformUtility.ScreenPointToLocalPointInRectangle(
			rectTr, screenPos, null, out Vector2 newPoint);
		var delta = Vector3.ClampMagnitude((Vector3)newPoint - originalPoint, radius);
		value = delta.normalized;
		stick.rectTransform.position = originalPoint + delta;
	}
	public void OnDrag(PointerEventData eventData)
	{
		if (pointerId != eventData.pointerId)
			return;

		UpdateStickPos(eventData.position);
	}
	public void OnPointerDown(PointerEventData eventData)
	{
		if (isDragging) { return; }
		isDragging = true;
		pointerId = eventData.pointerId;
		UpdateStickPos(eventData.position);
	}

	public void OnPointerUp(PointerEventData eventData)
	{
		if (pointerId != eventData.pointerId) { return; }
		isDragging = false;
		stick.rectTransform.position = originalPoint;
		value = Vector2.zero;
	}
}

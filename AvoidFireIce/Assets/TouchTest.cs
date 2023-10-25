using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TouchTest : MonoBehaviour
{
	public TextMeshProUGUI text;
	private void Update()
	{
		var message = string.Empty;
		foreach (var t in Input.touches)
		{
			message += "Touch ID: " + t.fingerId + "\tPhase" + t.phase + "\n";
			message += "\tPosition" + t.position;
			message += "\tDelta Pos" + t.deltaPosition + "\n";
		}
		message += "\n";
		text.text = message;
	}
}

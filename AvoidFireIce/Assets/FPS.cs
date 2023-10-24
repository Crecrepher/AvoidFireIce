using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class FpsCheck : MonoBehaviour
{
	float deltaTime = 0.0f;
	TMP_Text tx;
	void Update()
	{
		deltaTime += (Time.unscaledDeltaTime - deltaTime) * 0.1f;
		float fps = 1.0f / deltaTime;
		tx.text = $"{(int)fps} fps)";
	}

	void Awake()
	{
		tx = GetComponent<TMP_Text>();
	}
}

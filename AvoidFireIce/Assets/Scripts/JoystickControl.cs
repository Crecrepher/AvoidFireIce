using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JoystickControl : MonoBehaviour
{
	[SerializeField] private bl_Joystick Joystick;
	public float speed = 1f;
	private Player player;
	private Vector2 direction = Vector2.zero;
	private Rigidbody2D rb;

	private void Awake()
	{
		player = GetComponent<Player>();
		rb = GetComponent<Rigidbody2D>();
		Joystick = GameObject.FindGameObjectWithTag("Joy").GetComponent<bl_Joystick>();
	}
	private void FixedUpdate()
	{
		Vector2 position = rb.position;
		position += direction * speed * Time.fixedDeltaTime;
		rb.MovePosition(position);
	}

	void Update()
	{
		float v = Mathf.Min(Joystick.Vertical, 1);
		float h = Mathf.Min(Joystick.Horizontal, 1);

		direction = new Vector3(h, v);
		if (direction.magnitude > 1)
		{
			direction.Normalize();
		}
		//if (!player.isWall)
		//{
		//	float directionMag = direction.magnitude;
		//	if (directionMag > 1)
		//	{
		//		direction.Normalize();
		//	}
		//}
	}
}

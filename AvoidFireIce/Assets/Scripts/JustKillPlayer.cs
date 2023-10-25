using System.Collections;
using System.Collections.Generic;
using System.Xml.Linq;
using UnityEngine;

public class JustKillPlayer : MonoBehaviour
{
	private void OnCollisionEnter2D(Collision2D collision)
	{
		if (collision.gameObject.CompareTag("Player"))
		{
			Player player = collision.gameObject.GetComponent<Player>();
			player.Ouch();
		}
	}
	private void OnCollisionStay2D(Collision2D collision)
	{
		if (collision.gameObject.CompareTag("Player"))
		{
			Player player = collision.gameObject.GetComponent<Player>();
			player.Ouch();
		}
	}
}

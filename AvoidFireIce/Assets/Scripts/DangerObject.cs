using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DangerObject : MonoBehaviour
{
    public Element element = Element.None;
    private SpriteRenderer spr;

    private void Awake()
    { 
        spr = GetComponent<SpriteRenderer>();
        switch (element)
        {
            case Element.Fire:
                spr.color = GameManager.instance.FireColor;
                break;
            case Element.Ice:
                spr.color = GameManager.instance.IceColor;
                break;
        }
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
         Player player = collision.gameObject.GetComponent<Player>();
        if (collision.gameObject.CompareTag("Player") && player.CurrentElemental != element)
        {
            player.Ouch();
        }
    }
}

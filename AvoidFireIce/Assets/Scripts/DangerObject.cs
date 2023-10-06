using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DangerObject : MonoBehaviour
{
    public Element element = Element.Fire;
    private SpriteRenderer spr;

    private void Awake()
    { 
        spr = GetComponent<SpriteRenderer>();
        SetColor();
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
         Player player = collision.gameObject.GetComponent<Player>();
        if (collision.gameObject.CompareTag("Player") && player.CurrentElemental != element)
        {
            player.Ouch();
        }
    }

    public void SetColor()
    {
        switch (element)
        {
            case Element.Fire:
                spr.color = Defines.instance.FireColor;
                break;
            case Element.Ice:
                spr.color = Defines.instance.IceColor;
                break;
            case Element.None:
                spr.color = Defines.instance.DangerColor;
                break;
        }
    }
}

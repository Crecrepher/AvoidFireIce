using System.Collections;
using System.Collections.Generic;
using System.Xml.Linq;
using UnityEngine;

public class DangerObject : MonoBehaviour
{
    public Element element = Element.Fire;
    public bool isRay = false;
    protected SpriteRenderer spr;

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

    virtual public void SetColor()
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
        if (isRay)
        {
            Color alp = spr.color;
            alp.a = 0.2f;
            spr.color = alp;
        }
    }

    public void SetElement(Element val)
    {
        element = val;
        SetColor();
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public Element CurrentElemental = Element.Fire;


    public float speed = 1f;

    private Vector2 direction = Vector2.zero;


    private Rigidbody2D rb;
    private SpriteRenderer spr;



    private void OnEnable()
    {
        rb = GetComponent<Rigidbody2D>();
        spr = GetComponent<SpriteRenderer>();
        SetPlayerElementColor();
    }
    private void FixedUpdate()
    {
        Vector2 position = rb.position;
        position += direction * speed * Time.fixedDeltaTime;
        rb.MovePosition(position);

    }

    private void Update()
    {
        var h = Input.GetAxisRaw("Horizontal");
        var v = Input.GetAxisRaw("Vertical");

        direction = new Vector3(h, v);
        var directionMag = direction.magnitude;
        if (directionMag > 1)
        {
            direction.Normalize();
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            ChangePlayerElement();
        }
    }

    public void ChangePlayerElement()
    {
        CurrentElemental = (Element)(((int)CurrentElemental + 1) % 2);
        SetPlayerElementColor();
    }

    public void SetPlayerElementColor()
    {
        switch (CurrentElemental)
        {
            case Element.Fire:
                spr.color = Defines.instance.FireColor;
                break;
            case Element.Ice:
                spr.color = Defines.instance.IceColor;
                break;
        }
    }

    public void Ouch()
    {
        GameManager.instance.AutoRestart();
        Destroy(gameObject);
    }
}

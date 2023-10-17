using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wall : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Bullet"))
        {
            Bullet b = collision.gameObject.GetComponent<Bullet>();
            if (b != null) { b.Poof(); }
        }
    }
}

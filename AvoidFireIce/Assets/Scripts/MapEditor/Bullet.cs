using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private Rigidbody2D rb2d;

    public bool willDestroyed = true;
    private void Awake()
    {
        rb2d = GetComponent<Rigidbody2D>();
        if (willDestroyed) { Destroy(gameObject, 30f); }
    }

    public void Launch(Vector2 direction, float force)
    {
        rb2d.AddForce(direction * force, ForceMode2D.Impulse);
    }
}

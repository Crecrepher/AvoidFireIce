using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveBounce : MonoBehaviour
{
    public float areaMax = 8f;
    public float speed = 60f;
    public Vector2 dir = new Vector2(1f,0f);

    private Rigidbody2D rb2d;

    private void Awake()
    {
        rb2d = GetComponent<Rigidbody2D>();
    }
    private void Update()
    {
        Vector2 position = transform.position;
        position += dir * speed * Time.deltaTime;
        if ((position.x < -areaMax && dir.x < 0f) || (position.x > areaMax && dir.x > 0f))
        {
            dir.x *= -1f;
        }
        transform.position = position;
    }
}

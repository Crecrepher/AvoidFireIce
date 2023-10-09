using Newtonsoft.Json.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Swing : MonoBehaviour
{
    private float lasttime;
    private float maxtime = 1f;
    private float dir = 1f;
    private float speed = 60f;

    private void Awake()
    {
        lasttime = Time.time;
    }
    private void Update()
    {
        transform.Rotate(new Vector3(0f, 0f, dir * Time.deltaTime * speed));
        if (maxtime < Time.time - lasttime)
        {
            dir *= -1f;
            lasttime = Time.time;
        }
    }
}

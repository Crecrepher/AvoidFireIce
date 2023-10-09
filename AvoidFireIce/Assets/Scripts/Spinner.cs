using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spinner : MonoBehaviour
{
    public float speed = 60f;
    public float dir = 1f;
    private void Update()
    {
        transform.Rotate(new Vector3(0f, 0f, dir * Time.deltaTime * speed));
    }
}

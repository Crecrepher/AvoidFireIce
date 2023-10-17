using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShortParticle : MonoBehaviour
{
    public float DestroyTime = 3f;
    private void Awake()
    {
        Destroy(gameObject, DestroyTime);
    }
}

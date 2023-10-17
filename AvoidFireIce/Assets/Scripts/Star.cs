using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Star : MonoBehaviour
{
    public GameObject Poof;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            GameManager.instance.CheckWin(gameObject);
            Instantiate(Poof, transform.position, Quaternion.identity);
        }
    }


}

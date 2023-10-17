using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private Rigidbody2D rb2d;
    private float speed;
    public List<GameObject> ElementFire;
    public List<GameObject> ElementIce;
    public GameObject PoofFire;
    public GameObject PoofIce;

    private DangerObject element;
    public bool willDestroyed = true;
    private void Awake()
    {
        rb2d = GetComponent<Rigidbody2D>();
        element= GetComponent<DangerObject>();
        if (willDestroyed) { Destroy(gameObject, 30f); }

    }

    public void Launch(Vector2 direction, float force)
    {
        speed = force;
        rb2d.AddForce(direction * force, ForceMode2D.Impulse);
    }

    public void SetEffect(Element element)
    {
        switch (element)
        {
            case Element.Fire:
                foreach (var item in ElementFire)
                {
                    item.SetActive(true);
                }
                break;
            case Element.Ice:
                foreach (var item in ElementIce)
                {
                    item.SetActive(true);
                }
                break;
            default:
                foreach (var item in ElementFire)
                {
                    item.SetActive(true);
                }
                foreach (var item in ElementIce)
                {
                    item.SetActive(true);
                }
                break;
        }
    }

    public void Poof()
    {
        if (element.element == Element.Ice) 
        {
            Instantiate(PoofIce, transform.position, Quaternion.identity);
        }
        else 
        {
            Instantiate(PoofFire, transform.position, Quaternion.identity);
        }
        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Glass"))
        {
            Rigidbody2D glassRigidbody = collision.GetComponent<Rigidbody2D>();
            if (glassRigidbody != null)
            {
                Vector2 hitPoint = collision.ClosestPoint(transform.position);
                // ����ü�� ����
                Vector2 projectileDirection = rb2d.velocity.normalized;
                // ǥ���� ���� ����
                Vector2 contactNormal = (hitPoint - (Vector2)transform.position).normalized;
                // �ݻ� ���� ���
                Vector2 reflectDirection = Vector2.Reflect(projectileDirection, contactNormal);
                // ����� �ӵ� ������Ʈ
                rb2d.velocity = reflectDirection * speed;

            }
        }
    }
}

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
                // 투사체의 방향
                Vector2 projectileDirection = rb2d.velocity.normalized;
                // 표면의 법선 벡터
                Vector2 contactNormal = (hitPoint - (Vector2)transform.position).normalized;
                // 반사 방향 계산
                Vector2 reflectDirection = Vector2.Reflect(projectileDirection, contactNormal);
                // 방향과 속도 업데이트
                float angle = Mathf.Atan2(reflectDirection.y, reflectDirection.x) * Mathf.Rad2Deg;
                transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
                rb2d.velocity = reflectDirection * speed;

            }
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BulletTower : MonoBehaviour
{
    public Bullet Bullet;
    public float fireRate;
    public float bulletSpeed;

    private float lastTime;
    private Rigidbody2D rb;
    public float ShootGap = 1f;

    public int Element = 0;
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        lastTime = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        if (fireRate < Time.time - lastTime)
        {
            Shoot(Element);
            Element = (Element + 1) % 2;
            lastTime = Time.time;
        }
    }

    private void Shoot(int element)
    {
        Vector3 rotationEulerAngles = transform.rotation.eulerAngles;
        Vector3 direction = Quaternion.Euler(rotationEulerAngles) * Vector3.up;

        var pos = rb.position;
        pos += (Vector2)direction.normalized * ShootGap;

        var bullet = Instantiate(Bullet, pos, Quaternion.identity);
        bullet.Launch(direction.normalized, bulletSpeed);
        bullet.gameObject.GetComponent<DangerObject>().SetElement((Element)element);
    }
}

using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BulletTowerSwitcher : MonoBehaviour
{
    public Bullet Bullet;
    public float fireRate;
    public float bulletSpeed;

    private float lastTime;
    private Rigidbody2D rb;
    private DangerObject dangerObject;
    public float ShootGap = 1f;
    private int counter = 2;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        dangerObject = GetComponent<DangerObject>();
        lastTime = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        if (fireRate < Time.time - lastTime)
        {
            Shoot(dangerObject.element);
            counter = (counter + 1) % 3;
            if (counter == 2)
            {
                dangerObject.element = (Element)(((int)dangerObject.element + 1) % 2);
            }
            lastTime = Time.time;
        }
    }

    private void Shoot(Element element)
    {
        Vector3 rotationEulerAngles = transform.rotation.eulerAngles;
        Vector3 direction = Quaternion.Euler(rotationEulerAngles) * Vector3.up;

        var pos = rb.position;
        pos += (Vector2)direction.normalized * ShootGap;

        var bullet = Instantiate(Bullet, pos, Quaternion.identity);
        bullet.Launch(direction.normalized, bulletSpeed);
        bullet.gameObject.GetComponent<DangerObject>().SetElement(element);
    }
}
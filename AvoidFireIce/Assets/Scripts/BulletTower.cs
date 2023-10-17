using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BulletTower : MonoBehaviour
{
    public Bullet Bullet;
    public float fireRate;
    public float bulletSpeed;

    public ParticleSystem ShootFlareFire;
    public ParticleSystem ShootFlareIce;

    private float lastTime;
    private Rigidbody2D rb;
    private DangerObject dangerObject;
    public float ShootGap = 1f;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        dangerObject = GetComponent<DangerObject>();
        lastTime = Time.time;
    }

    void Update()
    {
        //if (fireRate < Time.time - lastTime)
        //{
        //    Shoot(dangerObject.element);
        //    lastTime = Time.time;
        //}
    }

    public void Shoot(Element element,float speed)
    {
        bulletSpeed = speed;
        Vector3 rotationEulerAngles = transform.rotation.eulerAngles;
        Vector3 direction = Quaternion.Euler(rotationEulerAngles) * Vector3.up;

        var pos = rb.position;
        pos += (Vector2)direction.normalized * ShootGap;


        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        Quaternion targetRotation = Quaternion.Euler(new Vector3(0, 0, angle));

        var bullet = Instantiate(Bullet, pos, targetRotation);
        bullet.Launch(direction.normalized, bulletSpeed);
        bullet.gameObject.GetComponent<DangerObject>().SetElement(element);
        bullet.gameObject.GetComponent<Bullet>().SetEffect(element);

        if (element == Element.Ice)
        {
            ShootFlareIce.Play();
        }
        else
        {
            ShootFlareFire.Play();
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;



public class RayTower : MonoBehaviour
{
    public LayerMask WallLayer;
    public float ShootGap = 0.4f;
    public int MaxReflections = 10;
    public Transform ShootPos;

    private Rigidbody2D rb;
    private LineRenderer RayLineRenderer;
    private DangerObject dangerObject;

    public bool Power = true;
    
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        RayLineRenderer = gameObject.GetComponentInChildren<LineRenderer>();
        dangerObject = GetComponent<DangerObject>();
        RayLineRenderer.positionCount = 10;
        RayLineRenderer.enabled = true;
    }

    void Update()
    {
        Vector3 rotationEulerAngles = transform.rotation.eulerAngles;
        Vector3 direction = Quaternion.Euler(rotationEulerAngles) * Vector3.up;
        Vector2 hitPosition = transform.position;

        int reflectionCount = 0;
        for (int i = 0; i < MaxReflections; i++)
        {
            RaycastHit2D hit = Physics2D.Raycast(hitPosition, direction.normalized, 100f, WallLayer);
            if (hit.collider != null)
            {
                if (hit.collider.CompareTag("Player"))
                {
                    Player player = hit.collider.gameObject.GetComponent<Player>();
                    hitPosition = hit.point;

                    if (player.CurrentElemental != dangerObject.element)
                    {
                        player.Ouch();
                    }
                }
                else if (hit.collider.CompareTag("Wall"))
                {
                    hitPosition = hit.point;
                }
                else if (hit.collider.CompareTag("Glass"))
                {
                    direction = Vector2.Reflect(direction, hit.normal);
                    hitPosition = hit.point;
                }
            }

            RayLineRenderer.positionCount = reflectionCount + 2;
            RayLineRenderer.SetPosition(reflectionCount + 1, hitPosition);
            reflectionCount++;
            hitPosition = hitPosition + (Vector2)direction* 0.1f;

            if (hit.collider == null || !hit.collider.CompareTag("Glass"))
                break;
        }
        RayLineRenderer.SetPosition(0, ShootPos.position);
    }



    public void SetRayColor()
    {
        Color color = Color.red;
        if (dangerObject == null)
        {
            dangerObject = GetComponent<DangerObject>();
        }
        switch (dangerObject.element)
        {
            case Element.Fire:
                color = Defines.instance.FireColor; 
                break;
            case Element.Ice:
                color = Defines.instance.IceColor; 
                break;
        }
        RayLineRenderer.startColor = color;
        RayLineRenderer.endColor = color;
    }
}

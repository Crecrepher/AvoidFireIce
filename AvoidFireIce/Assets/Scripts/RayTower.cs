using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;



public class RayTower : MonoBehaviour
{
    public LayerMask WallLayer;
    public float ShootGap = 0.4f;

    private Rigidbody2D rb;
    private LineRenderer RayLineRenderer;
    private DangerObject dangerObject;

    public bool Power = true;
    
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        RayLineRenderer = GetComponent<LineRenderer>();
        dangerObject = GetComponent<DangerObject>();
        RayLineRenderer.positionCount = 2;
        RayLineRenderer.enabled = true;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 rotationEulerAngles = transform.rotation.eulerAngles;
        Vector3 direction = Quaternion.Euler(rotationEulerAngles) * Vector3.up;

        Vector2 hitPosition = new Vector2();
        RaycastHit2D hit = Physics2D.Raycast(transform.position, direction.normalized,100f, WallLayer);
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
            if (hit.collider.CompareTag("Wall"))
            {
                hitPosition = hit.point;
            }
            //else if (hit.collider.CompareTag("GlassWall"))
            //{
            //    // 유리 벽을 만났을 때 처리
            //}
        }
        var pos = rb.position;
        pos += (Vector2)direction.normalized * ShootGap;
        RayLineRenderer.SetPosition(0, pos);
        RayLineRenderer.SetPosition(1, hitPosition);
    }

    public void SetRayColor()
    {
        Color color = Color.red;
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

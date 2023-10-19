using System.Collections;
using System.Collections.Generic;
using System.Xml.Linq;
using Unity.Burst.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;



public class RayTower : MonoBehaviour
{
    public LayerMask WallLayer;
    public int MaxReflections = 10;
    public Transform ShootPos;

    public List<GameObject> FireElements;
    public List<GameObject> IceElements;
    public Material FireRay;
    public Material IceRay;
    public Material DeathRay;

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
        Power = true;
    }

    void Update()
    {
        if (Power)
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
                    else if (hit.collider.CompareTag("Orb"))
                    {
                        hitPosition = hit.point;
                    }
                }

                RayLineRenderer.positionCount = reflectionCount + 2;
                RayLineRenderer.SetPosition(reflectionCount + 1, hitPosition);
                reflectionCount++;
                hitPosition = hitPosition + (Vector2)direction * 0.1f;

                if (hit.collider == null || !hit.collider.CompareTag("Glass"))
                    break;
            }
            RayLineRenderer.SetPosition(0, ShootPos.position);
            Vector2 pos = RayLineRenderer.GetPosition(RayLineRenderer.positionCount - 1);
            FireElements[1].transform.position = pos;
            IceElements[1].transform.position = pos;
        }
    }

    public void SetActiveRay(bool on,int element)
    {
        RayLineRenderer.enabled = on;
        Power = on;
        if (on)
        {
            if (dangerObject == null)
            {
                dangerObject = GetComponent<DangerObject>();
            }
            dangerObject.element = (Element)element;
            SetRayColor();
        }
        else
        {
            foreach (var item in FireElements)
            {
                item.SetActive(false);
            }
            foreach (var item in IceElements)
            {
                item.SetActive(false);
            }
        }
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
                RayLineRenderer.material = FireRay;
                foreach (var item in FireElements)
                {
                    item.SetActive(true);
                }
                foreach (var item in IceElements)
                {
                    item.SetActive(false);
                }
                break;
            case Element.Ice:
                RayLineRenderer.material = IceRay;
                foreach (var item in FireElements)
                {
                    item.SetActive(false);
                }
                foreach (var item in IceElements)
                {
                    item.SetActive(true);
                }
                break;
            default:
                RayLineRenderer.material = DeathRay;
                foreach (var item in FireElements)
                {
                    item.SetActive(true);
                }
                foreach (var item in IceElements)
                {
                    item.SetActive(true);
                }
                break;
        }
    }
}

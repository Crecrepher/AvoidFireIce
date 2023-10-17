using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WallBindRenderer : MonoBehaviour
{
    private CompositeCollider2D compositeCollider;
    private LineRenderer lineRenderer;
    private bool update = true;
    public void Update()
    {
        if (update)
        {
            update= false;
            WallBind();
        }
    }
    public void WallBind()
    {
        compositeCollider = GetComponent<CompositeCollider2D>();
        lineRenderer = GetComponent<LineRenderer>();

        lineRenderer.positionCount = compositeCollider.pointCount;
        lineRenderer.useWorldSpace = false;

        Vector2[] points = new Vector2[compositeCollider.pointCount];
        compositeCollider.GetPath(0, points);
        Vector3[] points3D = new Vector3[compositeCollider.pointCount];

        for (int i = 0; i < compositeCollider.pointCount; i++)
        {
            Vector2 point2D = points[i];
            points3D[i] = new Vector3(point2D.x, point2D.y, 0f);  // Set desired Z coordinate
        }

        lineRenderer.SetPositions(points3D);
    }
    void Start()
    {
        compositeCollider = GetComponent<CompositeCollider2D>();
        lineRenderer = GetComponent<LineRenderer>();
    }
}

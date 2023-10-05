using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineGrid : MonoBehaviour
{
    LineRenderer lr;
    public float sr, sc;
    public int rowCount, colCount;
    public float gridSize;

    public void OnEnable()
    {
        lr = GetComponent<LineRenderer>();
        initLineRenderer(lr);
        makeGrid(lr, sr, sc, rowCount, colCount);
    }

    void initLineRenderer(LineRenderer lr)
    {
        lr.startWidth = lr.endWidth = 0.1f;
    }

    void makeGrid(LineRenderer lr, float sr, float sc, int rowCount, int colCount)
    {
        List<Vector3> gridPos = new List<Vector3>();

        float ec = sc + colCount * gridSize;

        gridPos.Add(new Vector3(sr, sc, transform.position.z));
        gridPos.Add(new Vector3(sr, ec, transform.position.z));

        int toggle = -1;
        Vector3 currentPos = new Vector3(sr, ec, transform.position.z);
        for (int i = 0; i < rowCount; i++)
        {
            Vector3 nextPos = currentPos;

            nextPos.x += gridSize;
            gridPos.Add(nextPos);

            nextPos.y += (colCount * toggle * gridSize);
            gridPos.Add(nextPos);

            currentPos = nextPos;
            toggle *= -1;
        }

        currentPos.x = sr;
        gridPos.Add(currentPos);

        int colToggle = toggle = 1;
        if (currentPos.y == ec) colToggle = -1;

        for (int i = 0; i < colCount; i++)
        {
            Vector3 nextPos = currentPos;

            nextPos.y += (colToggle * gridSize);
            gridPos.Add(nextPos);

            nextPos.x += (rowCount * toggle * gridSize);
            gridPos.Add(nextPos);

            currentPos = nextPos;
            toggle *= -1;
        }

        lr.positionCount = gridPos.Count;
        lr.SetPositions(gridPos.ToArray());
    }

  
}
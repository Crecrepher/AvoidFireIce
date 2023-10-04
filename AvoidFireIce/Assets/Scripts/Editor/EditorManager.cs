using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class EditorManager : MonoBehaviour
{
    public Tilemap tilemap;
    public static EditorManager instance
    {
        get
        {
            if (editorManager == null)
            {
                editorManager = FindObjectOfType<EditorManager>();
            }
            return editorManager;
        }
    }

    private static EditorManager editorManager;

    private void OnEnable()
    {
        AdjustCameraOrthographicSize();
    }

    private void AdjustCameraOrthographicSize()
    {
        float mapWidth = tilemap.cellBounds.size.x * tilemap.cellSize.x;
        float mapHeight = tilemap.cellBounds.size.y * tilemap.cellSize.y;
        float targetOrthographicSize = Mathf.Max(mapWidth, mapHeight) * 0.5f;
        Camera.main.orthographicSize = targetOrthographicSize*1.2f;

        Camera.main.transform.position = new Vector3(0, -3, Camera.main.transform.position.z);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;


public class GameManager : MonoBehaviour
{
    public static GameManager instance
    {
        get
        {
            if (gameManager == null)
            {
                gameManager = FindObjectOfType<GameManager>();
            }
            return gameManager;
        }
    }

    private static GameManager gameManager;

    public Tilemap tilemap;
    public string StageName;
    public bool isTestMode = false;

    private void Awake()
    {
        Defines.instance.DefineColor();
        if (PlayerPrefs.GetString("StageName") != null)
        {
            StageName = PlayerPrefs.GetString("StageName");
            StageManager.instance.Load(StageName);
        }
        AdjustCameraOrthographicSize();
    }

    private void AdjustCameraOrthographicSize()
    {
        float mapWidth = tilemap.cellBounds.size.x * tilemap.cellSize.x;
        float mapHeight = tilemap.cellBounds.size.y * tilemap.cellSize.y;
        float targetOrthographicSize = Mathf.Max(mapWidth, mapHeight) * 0.5f;
        Camera.main.orthographicSize = targetOrthographicSize;

        Camera.main.transform.position = new Vector3(0, 0, Camera.main.transform.position.z);
    }
}

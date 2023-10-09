using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.SceneManagement;
using System.Security.Cryptography.X509Certificates;

public class EditorManager : MonoBehaviour
{
    public Palate palate;
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

    private void Awake()
    {
        Defines.instance.DefineColor();
        string a = PlayerPrefs.GetString("StageName");
        if (PlayerPrefs.GetString("StageName") != "")
        {
            StageSaveLoader.instance.Load(PlayerPrefs.GetString("StageName"));
        }

    }

    private void OnEnable()
    {
        AdjustCameraOrthographicSize();
    }

    private void AdjustCameraOrthographicSize()
    {
        float mapWidth = tilemap.cellBounds.size.x * tilemap.cellSize.x;
        float mapHeight = tilemap.cellBounds.size.y * tilemap.cellSize.y;
        float targetOrthographicSize = Mathf.Max(mapWidth, mapHeight) * 0.5f;
        Camera.main.orthographicSize = targetOrthographicSize * 1.2f;

        Camera.main.transform.position = new Vector3(0, -3, Camera.main.transform.position.z);
    }

    public void StartTesting()
    {
        if (GameObject.FindGameObjectWithTag("PlayerStart") == null)
        {
            Debug.LogWarning("Need PlayerStartPos!");
            return;
        }

        if (GameObject.FindGameObjectWithTag("Star") == null)
        {
            Debug.LogWarning("Need Star!");
            return;
        }
        StageSaveLoader.instance.Save("TeSt6212");
        PlayerPrefs.SetString("StageName", "TeSt6212");
        SceneManager.LoadScene("GameScene");
    }

}

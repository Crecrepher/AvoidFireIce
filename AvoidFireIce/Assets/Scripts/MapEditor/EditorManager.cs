using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.SceneManagement;
using System.Security.Cryptography.X509Certificates;
using System.IO;

public class EditorManager : MonoBehaviour
{
    public Palate palate;
    public Tilemap tilemap;
    public float Zoom = 0.3f;
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
        Debug.Log(PlayerPrefs.GetString("StageName"));
    }

    private void OnEnable()
    {
        AdjustCameraOrthographicSize();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            StageSaveLoader.instance.Save(PlayerPrefs.GetString("StageName"));
            SceneManager.LoadScene("TitleScene");
        }
    }

    private void AdjustCameraOrthographicSize()
    {
        float mapWidth = tilemap.cellBounds.size.x * tilemap.cellSize.x;
        float mapHeight = tilemap.cellBounds.size.y * tilemap.cellSize.y;
        float targetOrthographicSize = Mathf.Max(mapWidth, mapHeight) * Zoom;
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
        Debug.Log(Application.persistentDataPath);
        StageSaveLoader.instance.Save(PlayerPrefs.GetString("StageName"));
        StageSaveLoader.instance.Save("sys/TeSt6212");
        PlayerPrefs.SetString("TestStageName", Application.persistentDataPath + "\\sys\\TeSt6212");
        PlayerPrefs.SetInt("StageType", (int)StageType.Editing);
        SceneManager.LoadScene("GameScene");
    }

}

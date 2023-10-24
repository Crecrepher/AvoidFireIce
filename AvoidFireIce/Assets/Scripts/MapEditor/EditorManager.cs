using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.SceneManagement;
using System.Security.Cryptography.X509Certificates;
using System.IO;
using UnityEngine.UI;

public class EditorManager : MonoBehaviour
{
    public Palate palate;
    public Tilemap tilemap;
    public float Zoom = 0.3f;
    public CanvasScaler scaler;

    public GameObject SFX;
    public GameObject BGM;
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
            StageSaveLoader.instance.Load("CustomLevel/" + PlayerPrefs.GetString("StageName"));
        }
        PlayerPrefs.SetInt("StageType", (int)StageType.Editing);
        if (GameObject.FindGameObjectWithTag("Unloaded") == null)
        {
            Instantiate(BGM);
            Instantiate(SFX);
        }
        GlobalData.instance.StopMusic();
		if (Screen.width / Screen.height < 1.5f)
		{
            scaler.matchWidthOrHeight = 0f;
		}
        else
        {
			scaler.matchWidthOrHeight = 1f;
		}
	}

    private void OnEnable()
    {
        AdjustCameraOrthographicSize();
    }

    private void Update()
    {
        //if (Input.GetKeyDown(KeyCode.Escape))
        //{
        //    StageSaveLoader.instance.Save("CustomLevel/" + PlayerPrefs.GetString("StageName"));
        //    SceneManager.LoadScene("TitleScene");
        //}
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
		//StageSaveLoader.instance.Save("CustomLevel/" + PlayerPrefs.GetString("StageName"));
		StageSaveLoader.instance.Save(Path.Combine("CustomLevel", PlayerPrefs.GetString("StageName")));
		//StageSaveLoader.instance.Save("sys/TeSt6212");
		StageSaveLoader.instance.Save(Path.Combine("sys", "TeSt6212"));
		//PlayerPrefs.SetString("TestStageName", Application.persistentDataPath + "\\sys\\TeSt6212");
		PlayerPrefs.SetString("TestStageName", Path.Combine(Application.persistentDataPath, "sys", "TeSt6212"));
		PlayerPrefs.SetInt("StageType", (int)StageType.Editing);
        SceneManager.LoadScene("GameScene");
    }

}

using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
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
    public float RestartDelay = 1f;
    public float Zoom = 0.5f;
    private bool isWin = false;

    public GameObject WinWindow;
    public List<GameObject> SpecialObjsStage1;
    public List<GameObject> SpecialObjsStage2;

    public TMP_Text DeathCounter;
    public GameObject WallColiderBinder;
    public GameObject GlassColiderBinder;
    public GameObject SmallWallColiderBinder;

    private void Awake()
    {
        isWin = false;
        Defines.instance.DefineColor();
        if ((StageType)PlayerPrefs.GetInt("StageType") == StageType.Editing && PlayerPrefs.GetString("TestStageName") != null)
        {
            StageName = PlayerPrefs.GetString("TestStageName");
            StageManager.instance.Load(StageName);
        }
        else if (PlayerPrefs.GetString("StageName") != null)
        {
            StageName = PlayerPrefs.GetString("StageName");
            StageManager.instance.LoadStage(StageName);
            switch(StageName[0])
            {
                case '1':
                    Instantiate(SpecialObjsStage1[int.Parse(StageName[StageName.Length - 1].ToString())]);
                    break;
                case '2':
                    Instantiate(SpecialObjsStage2[int.Parse(StageName[StageName.Length - 1].ToString())]);
                    break;
            }
           
        }
        WallBind();
        GlassBind();
        SmallWallBind();
        AdjustCameraOrthographicSize();
        DeathCounter.text = $"Death: {PlayerPrefs.GetInt("DeathCount")}";
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)) 
        {
            switch ((StageType)PlayerPrefs.GetInt("StageType"))
            {
                case StageType.Official:
                case StageType.Custom:
                    SceneManager.LoadScene("TitleScene");
                    break;
                case StageType.Editing:
                    SceneManager.LoadScene("EditorScene");
                    break;
            }
        }
        if (Input.GetKeyDown(KeyCode.BackQuote))
        {
            NextLevel();
        }
    }

    private void AdjustCameraOrthographicSize()
    {
        float mapWidth = tilemap.cellBounds.size.x * tilemap.cellSize.x;
        float mapHeight = tilemap.cellBounds.size.y * tilemap.cellSize.y;
        float targetOrthographicSize = Mathf.Max(mapWidth, mapHeight) * Zoom;
        Camera.main.orthographicSize = targetOrthographicSize;

        Camera.main.transform.position = new Vector3(0, 0, Camera.main.transform.position.z);
    }

    public void AutoRestart()
    {
        if (isWin)
        {
            return;
        }
        int death = PlayerPrefs.GetInt("DeathCount");
        PlayerPrefs.SetInt("DeathCount", ++death);
        Invoke("ResetGame", RestartDelay);
    }

    public void ResetGame()
    {
        SceneManager.LoadScene("GameScene");
    }


    public void CheckWin(GameObject star)
    {
        Destroy(star);
        Invoke("StarFinder",0.1f);
    }

    public void StarFinder()
    {
        GameObject obj = GameObject.FindGameObjectWithTag("Star");
        if (obj == null)
        {
            Win();
        }
    }

    public void Win()
    {
        isWin = true;
        WinWindow.SetActive(true);
        Debug.Log("Win!");
    }

    public void NextLevel()
    {
        switch ((StageType)PlayerPrefs.GetInt("StageType"))
        {
            case StageType.Official:
                {
                    if (StageName == "1-6" || StageName == "2-7")
                    {
                        SceneManager.LoadScene("TitleScene");
                    }
                    else
                    {
                        string nextStage = $"{StageName.Substring(0, StageName.Length - 1)}{int.Parse(StageName[StageName.Length - 1].ToString()) +  1}";
                        PlayerPrefs.SetString("StageName", nextStage);
                        SceneManager.LoadScene("GameScene");
                    }
                }
                break;
            case StageType.Custom:
                { SceneManager.LoadScene("TitleScene"); }
                break;
            case StageType.Editing:
                { SceneManager.LoadScene("EditorScene"); }
                break;

        }
    }

    private void WallBind()
    {
        var walls = GameObject.FindGameObjectsWithTag("Wall");
        foreach (var wall in walls)
        {
            if(wall.name == "Tilemap" || wall.name == "WallBinder" || wall.name == "GlassBinder")
            {
                continue;
            }
            wall.transform.parent = WallColiderBinder.transform;
            Destroy(wall.GetComponent<Rigidbody2D>());
        }
        WallColiderBinder.GetComponent<CompositeCollider2D>().GenerateGeometry();

    }

    private void GlassBind()
    {
        var glasses = GameObject.FindGameObjectsWithTag("Glass");
        foreach (var glass in glasses)
        {
            if (glass.name == "Tilemap" || glass.name == "WallBinder" || glass.name == "GlassBinder")
            {
                continue;
            }
            glass.transform.parent = GlassColiderBinder.transform;
            Destroy(glass.GetComponent<Rigidbody2D>());
        }
        GlassColiderBinder.GetComponent<CompositeCollider2D>().GenerateGeometry();
    }

    private void SmallWallBind()
    {
        var smallWalls = GameObject.FindGameObjectsWithTag("SmallWall");
        foreach (var smallWall in smallWalls)
        {
            if (smallWall.name == "Tilemap" || smallWall.name == "WallBinder" || smallWall.name == "GlassBinder" || smallWall.name == "SmallWallBinder")
            {
                continue;
            }
            smallWall.transform.parent = SmallWallColiderBinder.transform;
            Destroy(smallWall.GetComponent<Rigidbody2D>());
        }
        SmallWallColiderBinder.GetComponent<CompositeCollider2D>().GenerateGeometry();
    }
}

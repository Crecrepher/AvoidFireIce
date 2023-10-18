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
    public bool isWin = false;

    public GameObject WinWindow;
    public List<GameObject> SpecialObjsStage1;
    public List<GameObject> SpecialObjsStage2;

    public TMP_Text DeathCounter;
    public TMP_Text MapName;
    public GameObject WallColiderBinder;
    public GameObject GlassColiderBinder;
    public GameObject SmallWallColiderBinder;

    public GameObject Bg;
    public GameObject Star1;
    public GameObject Star2;
    public List<SpriteRenderer> BgFire;
    public List<SpriteRenderer> BgIce;
    int bgSwipe = 1;

    public float FadeSpd = 1.5f;

    public float fadeMaxTimer = 1f;
    public float fadeTimer = 0f;

    public GameObject playerInfo;
    public GameObject playerPrefab;
    public GameObject starPrefab;
    private Vector2 startPos;
    public int levelInfo;
    public GameObject BGM;
    private void Awake()
    {
        isWin = false;
        Time.timeScale = 1f;
        Defines.instance.DefineColor();
        if ((StageType)PlayerPrefs.GetInt("StageType") == StageType.Editing && PlayerPrefs.GetString("TestStageName") != null)
        {
            StageName = PlayerPrefs.GetString("TestStageName");
            StageManager.instance.Load(StageName);
            MapName.text = "";
            levelInfo = 999;
        }
        else if (PlayerPrefs.GetString("StageName") != null && (StageType)PlayerPrefs.GetInt("StageType") == StageType.Official)
        {
            StageName = PlayerPrefs.GetString("StageName");
            StageManager.instance.LoadStage(StageName);
            switch(StageName[0])
            {
                case '1':
                    Instantiate(SpecialObjsStage1[int.Parse(StageName[StageName.Length - 1].ToString())]);
                    levelInfo = 10 + int.Parse(StageName[StageName.Length - 1].ToString());
                    break;
                case '2':
                    Instantiate(SpecialObjsStage2[int.Parse(StageName[StageName.Length - 1].ToString())]);
                    levelInfo = 20 + int.Parse(StageName[StageName.Length - 1].ToString());
                    break;
            }
            MapName.text = PlayerPrefs.GetString("StageName");
        }
        else if (PlayerPrefs.GetString("StageName") != null && (StageType)PlayerPrefs.GetInt("StageType") == StageType.Custom)
        {
            StageName = PlayerPrefs.GetString("StageName");
            StageManager.instance.Load(StageName);
            MapName.text = "";
            levelInfo = 999;
        }
        WallBind();
        GlassBind();
        SmallWallBind();
        AdjustCameraOrthographicSize();
        DeathCounter.text = $"Total Death: {PlayerPrefs.GetInt("DeathCount")}";
        fadeTimer = fadeMaxTimer;
        bgSwipe = 1;
        startPos = GameObject.FindGameObjectWithTag("Player").transform.position;
        if (GameObject.FindGameObjectWithTag("Unloaded") == null)
        {
            Instantiate(BGM);
        }
        bool bg = GlobalData.instance.isBGOn;
        Bg.SetActive(bg);
        Star1.SetActive(bg);
        Star2.SetActive(bg);
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.BackQuote))
        {
            NextLevel();
        }

        if (fadeTimer < fadeMaxTimer && fadeTimer > -fadeMaxTimer)
        { 
            fadeTimer += Time.deltaTime * bgSwipe * FadeSpd;
            foreach (var item in BgFire)
            {
                item.color = new Color(1, 1, 1, Mathf.Min(fadeTimer / fadeMaxTimer,1f));
            }
            foreach (var item in BgIce)
            {
                item.color = new Color(1, 1, 1, Mathf.Max(-(fadeTimer / fadeMaxTimer),0));
            }
        }

        if (playerInfo != null)
        {
            {
                Vector2 startPos = Bg.transform.position;
                Vector2 targetPos = -playerInfo.transform.position / 10f;
                if (Vector2.Distance(startPos, startPos) <= 0.1f)
                {
                    Vector2 direction = (targetPos - startPos).normalized;
                    Vector2 newPosition = startPos + (direction * Time.deltaTime);
                    Bg.transform.position = newPosition;
                }
            }

            {
                Vector2 startPos = Star1.transform.position;
                Vector2 targetPos = -playerInfo.transform.position / 5f; 
                if (Vector2.Distance(startPos, startPos) <= 0.1f)
                {
                    Vector2 direction = (targetPos - startPos).normalized;
                    Vector2 newPosition = startPos + (direction * Time.deltaTime * 2f);
                    Star1.transform.position = newPosition;
                }
            }

            {
                Vector2 startPos = Star2.transform.position;
                Vector2 targetPos = -playerInfo.transform.position / 2f ;
                if (Vector2.Distance(startPos, startPos) <= 0.1f)
                {
                    Vector2 direction = (targetPos - startPos).normalized;
                    Vector2 newPosition = startPos + (direction * Time.deltaTime * 5f);
                    Star2.transform.position = newPosition;
                }
            }
        }
        
    }

    public void BgCheck()
    {
        bool bg = GlobalData.instance.isBGOn;
        Bg.SetActive(bg);
        Star1.SetActive(bg);
        Star2.SetActive(bg);
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
        GetComponent<InGameUi>().DeathPlus();
        Destroy(playerInfo);
        GameObject player = Instantiate(playerPrefab, startPos, Quaternion.identity);
        playerInfo = player;
        if (bgSwipe < 0)
        {
            SwipeBG();
        }
        var stars = GameObject.FindGameObjectsWithTag("Star");
        foreach (var item in stars)
        {
            Destroy(item);
        }
        var starPos = StageManager.instance.Stars;
        foreach (var item in starPos)
        {
            Instantiate(starPrefab, item, Quaternion.identity);
        }
        DeathCounter.text = $"Total Death: {PlayerPrefs.GetInt("DeathCount")}";
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
        GetComponent<InGameUi>().SetWinTXT();
        isWin = true;
        WinWindow.SetActive(true);
        if (PlayerPrefs.GetInt("Clear") < levelInfo)
        {
            PlayerPrefs.SetInt("Clear", levelInfo);
        }
    }

    public bool NextAble()
    {
        return PlayerPrefs.GetInt("Clear") >= levelInfo;
    }
    public void NextLevel()
    {
        switch ((StageType)PlayerPrefs.GetInt("StageType"))
        {
            case StageType.Official:
                {
                    if (StageName == "2-7")
                    {
                        SceneManager.LoadScene("TitleScene");
                    }
                    else if (StageName == "1-6")
                    {
                        PlayerPrefs.SetString("StageName", "2-0");
                        SceneManager.LoadScene("GameScene");
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
    public void PreviousLevel()
    {
        switch ((StageType)PlayerPrefs.GetInt("StageType"))
        {
            case StageType.Official:
                {
                    if (StageName == "1-0" || (StageName == "2-0"))
                    {
                        SceneManager.LoadScene("TitleScene");
                    }
                    else
                    {
                        string preStage = $"{StageName.Substring(0, StageName.Length - 1)}{int.Parse(StageName[StageName.Length - 1].ToString()) - 1}";
                        PlayerPrefs.SetString("StageName", preStage);
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

    public void SwipeBG()
    {
        var element = playerInfo.GetComponent<Player>().CurrentElemental;
        if (element == Element.Fire)
        {
            bgSwipe = 1;
            if (fadeTimer <= -fadeMaxTimer)
            {
                fadeTimer = -fadeMaxTimer + 0.1f;
            }
        }
        else
        {
            bgSwipe = -1;
            if (fadeTimer >= fadeMaxTimer)
            {
                fadeTimer = fadeMaxTimer - 0.1f;
            }
        }

    }


}

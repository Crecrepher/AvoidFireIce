using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using System.IO;
using static QuaternionConverter;
using static UnityEditor.PlayerSettings;

public class WallInfo
{
    public Vector2 position;

    public WallInfo() { }
    public WallInfo(Vector2 position) { this.position = position; }
}

public class EnemyInfo
{
    public Vector2 position;
    public EnemyInfo() { }
    public EnemyInfo(Vector2 position) { this.position = position; }
}

public class StageSaveLoader : MonoBehaviour
{
    public class SaveData
    {
        public Vector2 playerStartPos;
        public List<WallInfo> walls;
        public List<EnemyInfo> enemys;
    }

    public static StageSaveLoader instance
    {
        get
        {
            if (saveLoader == null)
            {
                saveLoader = FindObjectOfType<StageSaveLoader>();
            }
            return saveLoader;
        }
    }

    private static StageSaveLoader saveLoader;

    public GameObject playerStartPosPrefab;
    public GameObject wallPrefab;
    public GameObject enemyPrefab;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            Save("Test");
            Debug.Log("Saved");
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            Load("Test");
            Debug.Log("Loaded");
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            Clear();
            Debug.Log("Clear");
        }
    }

    public void Save(string fileName)
    {
        var saveData = new SaveData();
        saveData.walls = new List<WallInfo>();
        saveData.enemys = new List<EnemyInfo>();

        saveData.playerStartPos = GameObject.FindGameObjectWithTag("PlayerStart").transform.position;

        var walls = GameObject.FindGameObjectsWithTag("Wall");
        foreach (var wall in walls)
        {
            var wallInfo = new WallInfo
            {
                position = wall.transform.position,
            };
            saveData.walls.Add(wallInfo);
        }

        var enemies = GameObject.FindGameObjectsWithTag("Enemy");
        foreach (var enemy in enemies)
        {
            var enemyInfo = new EnemyInfo
            {
                position = enemy.transform.position,
            };
            saveData.enemys.Add(enemyInfo);
        }

        var path = Path.Combine(Application.persistentDataPath, fileName + ".json");
        Debug.Log(path);

        var json = JsonConvert.SerializeObject(saveData, new Vector2Converter(), new WallInfoConverter(), new EnemyInfoConverter());
        Debug.Log(json);

        File.WriteAllText(path, json);
    }

    public void Clear()
    {
        var walls = GameObject.FindGameObjectsWithTag("Wall");
        var enemies = GameObject.FindGameObjectsWithTag("Enemy");
        foreach (var wall in walls)
        {
            Destroy(wall);
        }
        foreach (var enemy in enemies)
        {
            Destroy(enemy);
        }
        Destroy(GameObject.FindGameObjectWithTag("PlayerStart"));
    }

    public void Load(string fileName)
    {
        var path = Path.Combine(Application.persistentDataPath, fileName + ".json");
        var json = File.ReadAllText(path);
        var saveData = JsonConvert.DeserializeObject<SaveData>(json, new Vector2Converter(), new WallInfoConverter(), new EnemyInfoConverter());

        GameObject playerStartPos = Instantiate(playerStartPosPrefab, saveData.playerStartPos, Quaternion.identity);

        foreach (var wall in saveData.walls)
        {
            GameObject obj = Instantiate(wallPrefab, wall.position, Quaternion.identity);
        }
        foreach (var enemy in saveData.enemys)
        {
            GameObject obj = Instantiate(enemyPrefab, enemy.position, Quaternion.identity);
        }
    }
}

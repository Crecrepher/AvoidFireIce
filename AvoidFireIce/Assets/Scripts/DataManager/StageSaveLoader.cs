using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using System.IO;
using static QuaternionConverter;

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

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            Save("Test");
            Debug.Log("Saved");
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            Load();
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

        var enemies = GameObject.FindGameObjectsWithTag("Enemies");
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
        var cubes = GameObject.FindGameObjectsWithTag("Cube");
        foreach (var cube in cubes)
        {
            Destroy(cube);
        }
    }

    public void Load()
    {
        var path = Path.Combine(Application.persistentDataPath, "cubes.json");
        var json = File.ReadAllText(path);
        var cubeList = JsonConvert.DeserializeObject<List<CubeData>>(json, new Vector3Converter(), new QuaternionConverter());
        foreach (var pos in cubeList)
        {
            GameObject obj = Instantiate(prefab, pos.Position, pos.Rotation);
            obj.transform.localScale = pos.Scale;
        }
    }
}

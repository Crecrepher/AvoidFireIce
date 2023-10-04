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

        var path = Path.Combine(Application.persistentDataPath, fileName + ".json");
        Debug.Log(path);

        var json = JsonConvert.SerializeObject(saveData, new Vector2Converter(), new WallInfoConverter(), new EnemyInfoConverter());
        Debug.Log(json);

        File.WriteAllText(path, json);
    }

}

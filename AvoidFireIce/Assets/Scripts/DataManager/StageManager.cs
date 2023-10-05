using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using static QuaternionConverter;
using static StageSaveLoader;

public class StageManager : MonoBehaviour
{
    public static StageManager instance
    {
        get
        {
            if (stageManager == null)
            {
                stageManager = FindObjectOfType<StageManager>();
            }
            return stageManager;
        }
    }

    private static StageManager stageManager;

    public GameObject playerPrefab;
    public GameObject wallPrefab;
    public GameObject enemyPrefab;

    public void Load(string fileName)
    {
        var path = Path.Combine(Application.persistentDataPath, fileName + ".json");
        var json = File.ReadAllText(path);
        var saveData = JsonConvert.DeserializeObject<SaveData>(json, new Vector2Converter(), new WallInfoConverter(), new EnemyInfoConverter());

        GameObject playerStartPos = Instantiate(playerPrefab, saveData.playerStartPos, Quaternion.identity);

        foreach (var wall in saveData.walls)
        {
            GameObject obj = Instantiate(wallPrefab, wall.position, Quaternion.identity);
        }
        foreach (var enemy in saveData.enemys)
        {
            GameObject obj = Instantiate(enemyPrefab, enemy.position, Quaternion.identity);
        }
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape)) { SceneManager.LoadScene("EditorScene"); }
    }
}

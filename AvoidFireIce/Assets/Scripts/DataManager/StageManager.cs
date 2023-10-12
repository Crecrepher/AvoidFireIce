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

    public List<GameObject> ObjsPrefab;

    public void LoadStage(string fileName)
    {
        var path = "Stages/" + fileName;
        var json = Resources.Load<TextAsset>(path).text;
        MakeObjs(json);
    }

    public void Load(string fileName)
    {
        var path = fileName + ".json";
        var json = File.ReadAllText(path);
        MakeObjs(json);
    }

    private void MakeObjs(string json)
    {
        var saveData = JsonConvert.DeserializeObject<SaveData>(json, new EditorObjInfoConverter(), new MoveLoopConverter());

        int loadCount = 0;
        int moveLoopsCount = 0;
        int initCode = saveData.moveLoops[moveLoopsCount].initCode;
        foreach (var loadedObj in saveData.objects)
        {
            GameObject obj = Instantiate(ObjsPrefab[loadedObj.code], loadedObj.pos, loadedObj.rot);
            if (Defines.instance.isHaveElement(loadedObj.code))
            {
                DangerObject dangerObj = obj.GetComponent<DangerObject>();
                dangerObj.element = (Element)loadedObj.element;
                dangerObj.SetColor();
            }
            if (initCode == loadCount)
            {
                MoveLoop ml = saveData.moveLoops[moveLoopsCount];
                moveLoopsCount++;
                if (saveData.moveLoops.Count > moveLoopsCount)
                {
                    initCode = saveData.moveLoops[moveLoopsCount].initCode;
                }
                MoveLoopPlayer mlp = obj.AddComponent<MoveLoopPlayer>();
                mlp.loopTime = ml.loopTime;
                mlp.startPos = obj.transform.position;
                mlp.loopList = ml.loopList;
                mlp.Init();
            }
            loadCount++;
        }
    }
}

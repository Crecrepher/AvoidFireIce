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
        var saveData = JsonConvert.DeserializeObject<SaveData>(json, new EditorObjInfoConverter(), new MoveLoopConverter(),new RotateLoopConverter());

        int loadCount = 0;
        int moveLoopsCount = 0;
        int rotateLoopsCount = 0;

        int moveInitCode = 0;
        int rotateInitCode = 0;
        if (saveData.moveLoops != null && saveData.moveLoops.Count > 0)
        {
            moveInitCode = saveData.moveLoops[moveLoopsCount].initCode;
        }
        if (saveData.rotateLoops != null && saveData.rotateLoops.Count > 0)
        {
            rotateInitCode = saveData.rotateLoops[rotateLoopsCount].initCode;
        }

        foreach (var loadedObj in saveData.objects)
        {
            GameObject obj = Instantiate(ObjsPrefab[loadedObj.code], loadedObj.pos, loadedObj.rot);
            if (Defines.instance.isHaveElement(loadedObj.code))
            {
                DangerObject dangerObj = obj.GetComponent<DangerObject>();
                dangerObj.element = (Element)loadedObj.element;
                dangerObj.SetColor();
            }
            if (moveInitCode == loadCount && saveData.moveLoops != null && saveData.moveLoops.Count > 0)
            {
                MoveLoop ml = saveData.moveLoops[moveLoopsCount];
                moveLoopsCount++;
                if (saveData.moveLoops.Count > moveLoopsCount)
                {
                    moveInitCode = saveData.moveLoops[moveLoopsCount].initCode;
                }
                MoveLoopPlayer mlp = obj.AddComponent<MoveLoopPlayer>();
                mlp.loopTime = ml.loopTime;
                mlp.startPos = obj.transform.position;
                mlp.loopList = ml.loopList;
                mlp.Init();
            }
            if (rotateInitCode == loadCount && saveData.rotateLoops != null && saveData.rotateLoops.Count > 0)
            {
                RotateLoop rl = saveData.rotateLoops[rotateLoopsCount];
                rotateLoopsCount++;
                if (saveData.rotateLoops.Count > rotateLoopsCount)
                {
                    rotateInitCode = saveData.rotateLoops[rotateLoopsCount].initCode;
                }
                RotateLoopPlayer rlp = obj.AddComponent<RotateLoopPlayer>();
                rlp.loopTime = rl.loopTime;
                rlp.startRot = obj.transform.rotation;
                rlp.loopList = rl.loopList;
                rlp.Init();
            }
            loadCount++;
        }
    }
}

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
    public List<Vector2> Stars;
    public List<Vector2> Orbs;
    public List<int> OrbsEle;

    public void LoadStage(string fileName)
    {
        var path = "Stages/" + fileName;
        var json = Resources.Load<TextAsset>(path).text;
        MakeObjs(json);
        var player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            GameManager.instance.playerInfo = player;
        }
    }

    public void Load(string fileName)
    {
        var path = fileName + ".json";
        var json = File.ReadAllText(path);
        MakeObjs(json);
        var player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            GameManager.instance.playerInfo = player;
        }
    }

    private void MakeObjs(string json)
    {
        Stars = new List<Vector2>();
        Orbs = new List<Vector2>();
        OrbsEle = new List<int>();

        var saveData = JsonConvert.DeserializeObject<SaveData>(json, new EditorObjInfoConverter(), new MoveLoopConverter(), new RotateLoopConverter(), new FireLoopConverter());

        int loadCount = 0;
        int moveLoopsCount = 0;
        int rotateLoopsCount = 0;
        int fireLoopsCount = 0;

        int moveInitCode = 0;
        int rotateInitCode = 0;
        int fireInitCode = 0;

        if (saveData.moveLoops != null && saveData.moveLoops.Count > 0)
        {
            moveInitCode = saveData.moveLoops[moveLoopsCount].initCode;
        }
        if (saveData.rotateLoops != null && saveData.rotateLoops.Count > 0)
        {
            rotateInitCode = saveData.rotateLoops[rotateLoopsCount].initCode;
        }
        if (saveData.fireLoops != null && saveData.fireLoops.Count > 0)
        {
            fireInitCode = saveData.fireLoops[fireLoopsCount].initCode;
        }

        GameObject currentGroup = null;
        int groupIndex = -1;

        foreach (var loadedObj in saveData.objects)
        {
            GameObject obj = Instantiate(ObjsPrefab[loadedObj.code], loadedObj.pos, loadedObj.rot);
            if (loadedObj.code == 10)
            {
                currentGroup = obj;
                groupIndex++;
            }
            else if (loadedObj.code == (int)ObjectType.Star)
            {
                Stars.Add(loadedObj.pos);
            }
            else if (loadedObj.code == (int)ObjectType.Bullet)
            {
                Orbs.Add(loadedObj.pos);
                OrbsEle.Add(loadedObj.element);
            }
            if (Defines.instance.isHaveElement(loadedObj.code))
            {
                DangerObject dangerObj = obj.GetComponent<DangerObject>();
                dangerObj.element = (Element)loadedObj.element;
                dangerObj.SetColor();
                if(loadedObj.code == (int)ObjectType.Bullet)
                {
                    obj.GetComponent<Bullet>().SetEffect(dangerObj.element);
                }
            }

            if (moveInitCode == loadCount && saveData.moveLoops != null && saveData.moveLoops.Count > 0)
            {
                MoveLoop ml = saveData.moveLoops[moveLoopsCount];
                moveLoopsCount++;
                if (saveData.moveLoops.Count > moveLoopsCount)
                {
                    moveInitCode = saveData.moveLoops[moveLoopsCount].initCode;
                }
                if (ml.loopList.Count > 0)
                {
                    MoveLoopPlayer mlp = obj.AddComponent<MoveLoopPlayer>();
                    mlp.loopTime = ml.loopTime;
                    mlp.startPos = obj.transform.position;
                    mlp.loopList = ml.loopList;
                    mlp.Init();
                }
            }
            if (rotateInitCode == loadCount && saveData.rotateLoops != null && saveData.rotateLoops.Count > 0)
            {
                RotateLoop rl = saveData.rotateLoops[rotateLoopsCount];
                rotateLoopsCount++;
                if (saveData.rotateLoops.Count > rotateLoopsCount)
                {
                    rotateInitCode = saveData.rotateLoops[rotateLoopsCount].initCode;
                }
                if (rl.loopList.Count > 0)
                {
                    RotateLoopPlayer rlp = obj.AddComponent<RotateLoopPlayer>();
                    rlp.loopTime = rl.loopTime;
                    rlp.startRot = obj.transform.rotation;
                    rlp.loopList = rl.loopList;
                    rlp.Init();
                }
            }
            if (fireInitCode == loadCount && saveData.fireLoops != null && saveData.fireLoops.Count > 0)
            {
                FireLoop fl = saveData.fireLoops[fireLoopsCount];
                if (fl.loopList.Count > 0 )
                {
                    fireLoopsCount++;
                    if (saveData.fireLoops.Count > fireLoopsCount)
                    {
                        fireInitCode = saveData.fireLoops[fireLoopsCount].initCode;
                    }
                    if (fl.loopList.Count > 0)
                    {
                        switch (loadedObj.code)
                        {
                            case 0:
                                {
                                    BulletFireLoopPlayer flp = obj.AddComponent<BulletFireLoopPlayer>();
                                    flp.loopTime = fl.loopTime;
                                    flp.loopList = fl.loopList;
                                    flp.Init();
                                }
                                break;
                            case 1:
                                {
                                    RayFireLoopPlayer flp = obj.AddComponent<RayFireLoopPlayer>();
                                    flp.loopTime = fl.loopTime;
                                    flp.loopList = fl.loopList;
                                    flp.Init();
                                }
                                break;
                        }
                    }
                }
                
            }
            if (groupIndex >= 0 && saveData.groupList != null && saveData.groupList[groupIndex].Contains(loadCount))
            {
                obj.transform.SetParent(currentGroup.transform);
            }
            loadCount++;
        }
    }

    public void RespawnOrbs() 
    {
        for (int i = 0; i < Orbs.Count; i++)
        {
            GameObject obj = Instantiate(ObjsPrefab[2], Orbs[i], Quaternion.identity);
            DangerObject dangerObj = obj.GetComponent<DangerObject>();
            dangerObj.element = (Element)OrbsEle[i];
            dangerObj.SetColor();
            obj.GetComponent<Bullet>().SetEffect(dangerObj.element);
        }
    }
}

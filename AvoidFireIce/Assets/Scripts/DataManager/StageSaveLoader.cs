using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using System.IO;
using static QuaternionConverter;

public class EditorObjInfo
{
    public int code;
    public int element;
    public Vector2 pos;
    public Quaternion rot;
    public EditorObjInfo() { }
    public EditorObjInfo (int code, int element, Vector2 pos, Quaternion rot) 
    { 
        this.code = code;
        this.element = element;
        this.pos = pos;
        this.rot = rot;
    }

}

public class StageSaveLoader : MonoBehaviour
{
    public class SaveData
    {
        public List<EditorObjInfo> objects;
        public List<MoveLoop> moveLoops;
        public List<RotateLoop> rotateLoops;
        public List<FireLoop> fireLoops;
        public List<List<int>> groupList;
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

    public List<GameObject> EditorObjs;

    private void Awake()
    {
        string directoryPath = Application.persistentDataPath + "\\sys";
        DirectoryInfo directory = new DirectoryInfo(directoryPath);
        if (!directory.Exists)
        {
            directory.Create();
        }
        string directoryPath2 = Application.persistentDataPath + "\\CustomLevel";
        DirectoryInfo directory2 = new DirectoryInfo(directoryPath2);
        if (!directory2.Exists)
        {
            directory2.Create();
        }
    }

    private int saveCount = 0;

    public void Save(string fileName)
    {
        saveCount = 0;
        var saveData = new SaveData();
        saveData.objects = new List<EditorObjInfo>();
        saveData.moveLoops = new List<MoveLoop>();
        saveData.rotateLoops = new List<RotateLoop>();
        saveData.fireLoops = new List<FireLoop>();
        saveData.groupList = new List<List<int>>();

        var objs = GameObject.FindGameObjectsWithTag("EditorMarker");
        foreach (var obj in objs)
        {
            RecordData(obj,saveData);
        }

        var stars = GameObject.FindGameObjectsWithTag("Star");
        foreach (var star in stars)
        {
            RecordData(star, saveData);
        }
        GameObject player = GameObject.FindGameObjectWithTag("PlayerStart");
        if (player != null)
        {
            RecordData(GameObject.FindGameObjectWithTag("PlayerStart"), saveData);
        }

        var groups = GameObject.FindGameObjectsWithTag("Group");
        foreach (var group in groups)
        {
            RecordGroupData(group, saveData);
        }

        var path = Path.Combine(Application.persistentDataPath, fileName + ".json");

        var json = JsonConvert.SerializeObject(saveData,new EditorObjInfoConverter(), new MoveLoopConverter(),new RotateLoopConverter(),new FireLoopConverter());

        File.WriteAllText(path, json);
    }

    private void RecordData(GameObject obj, SaveData saveData)
    {
        var objInfo = new EditorObjInfo
        {
            code = obj.GetComponent<MarkerInfo>().ObjectType,
            element = 3,
            pos = obj.transform.position,
            rot = obj.transform.rotation
        };
        if (Defines.instance.isHaveElement(objInfo.code))
        {
            objInfo.element = (int)obj.GetComponent<DangerObject>().element;
        }
        saveData.objects.Add(objInfo);

        MoveLoopData mld = obj.GetComponent<MoveLoopData>();
        if (mld != null) 
        {
            MoveLoop saveMl = mld.ml;
            saveMl.initCode = saveCount;
            saveData.moveLoops.Add(saveMl);
        }

        RotateLoopData rld = obj.GetComponent<RotateLoopData>();
        if (rld != null)
        {
            RotateLoop saveRl = rld.rl;
            saveRl.initCode = saveCount;
            saveData.rotateLoops.Add(saveRl);
        }

        int type = obj.GetComponent<MarkerInfo>().ObjectType;
        if (type == 0 || type == 1)
        {
            FireLoopData fld = obj.GetComponent<FireLoopData>();
            if (fld != null)
            {
                FireLoop saveFl = fld.fl;
                saveFl.initCode = saveCount;
                saveData.fireLoops.Add(saveFl);
            }
        }
        saveCount++;
    }

    private void RecordGroupData(GameObject obj, SaveData saveData)
    {
        List<int> currGroup=  new List<int>();
        var objInfo = new EditorObjInfo
        {
            code = obj.GetComponent<MarkerInfo>().ObjectType,
            element = 3,
            pos = obj.transform.position,
            rot = obj.transform.rotation
        };
        saveData.objects.Add(objInfo);

        MoveLoopData mld = obj.GetComponent<MoveLoopData>();
        if (mld != null)
        {
            MoveLoop saveMl = mld.ml;
            saveMl.initCode = saveCount;
            saveData.moveLoops.Add(saveMl);
        }

        RotateLoopData rld = obj.GetComponent<RotateLoopData>();
        if (rld != null)
        {
            RotateLoop saveRl = rld.rl;
            saveRl.initCode = saveCount;
            saveData.rotateLoops.Add(saveRl);
        }
        saveCount++;

        var children = obj.GetComponentsInChildren<Transform>();
        foreach (var child in children)
        {
            if (child.CompareTag("Group"))
            {
                continue;
            }
            child.SetParent(null);
            RecordData(child.gameObject, saveData);
            currGroup.Add(saveCount-1);
            child.SetParent(obj.transform);
        }
        saveData.groupList.Add(currGroup);
    }

    public void Clear()
    {
        var objs = GameObject.FindGameObjectsWithTag("EditorMarker");
        foreach (var obj in objs)
        {
            Destroy(obj);
        }
    }

    public void Load(string fileName)
    {
        var path = Path.Combine(Application.persistentDataPath, fileName + ".json");
        if (!File.Exists(path))
        { return; }

        Debug.Log(Application.persistentDataPath);
        int LoadCount = 0;
        int LoadMoveLoopCount = 0;
        int LoadRotateLoopCount = 0;
        int LoadFireLoopCount = 0;
        GameObject currentGroup = null;
        int groupIndex = -1;

        var json = File.ReadAllText(path);
        var saveData = JsonConvert.DeserializeObject<SaveData>(json, new EditorObjInfoConverter(), new MoveLoopConverter(), new RotateLoopConverter(),new FireLoopConverter());

        foreach (var loadedObj in saveData.objects)
        {
            GameObject obj = Instantiate(EditorObjs[loadedObj.code], loadedObj.pos, loadedObj.rot);
            if (loadedObj.code == 10)
            {
                currentGroup = obj;
                groupIndex++;
            }
            if (Defines.instance.isHaveElement(loadedObj.code))
            {
                DangerObject dangerObj = obj.GetComponent<DangerObject>();
                dangerObj.element = (Element)loadedObj.element;
                dangerObj.SetColor();
            }
            if (saveData.moveLoops != null && LoadMoveLoopCount < saveData.moveLoops.Count)
            {
                if (saveData.moveLoops[LoadMoveLoopCount].initCode == LoadCount)
                {
                    LoopBlocksList lbl = obj.AddComponent<LoopBlocksList>();
                    lbl.moveLoopBlocks = new List<GameObject>();
                    obj.AddComponent<MoveLoopData>().ml = new MoveLoop();
                    MoveLoop newMl = obj.GetComponent<MoveLoopData>().ml;
                    newMl.loopTime = saveData.moveLoops[LoadMoveLoopCount].loopTime;
                    newMl.loopList = saveData.moveLoops[LoadMoveLoopCount].loopList;
                    LoadMoveLoopCount++;
                }
            }
            if (saveData.rotateLoops != null && LoadRotateLoopCount < saveData.rotateLoops.Count)
            {
                if (saveData.rotateLoops[LoadRotateLoopCount].initCode == LoadCount)
                {
                    LoopBlocksList lbl = obj.GetComponent<LoopBlocksList>();
                    if (lbl == null)
                    {
                        lbl = obj.AddComponent<LoopBlocksList>();
                    }
                    lbl.rotateLoopBlocks = new List<GameObject>();
                    obj.AddComponent<RotateLoopData>().rl = new RotateLoop();
                    RotateLoop newRl = obj.GetComponent<RotateLoopData>().rl;
                    newRl.loopTime = saveData.rotateLoops[LoadRotateLoopCount].loopTime;
                    newRl.loopList = saveData.rotateLoops[LoadRotateLoopCount].loopList;
                    LoadRotateLoopCount++;
                }
            }

            if (saveData.fireLoops != null && LoadFireLoopCount < saveData.fireLoops.Count)
            {
                if (saveData.fireLoops[LoadFireLoopCount].initCode == LoadCount)
                {
                    LoopBlocksList lbl = obj.GetComponent<LoopBlocksList>();
                    if (lbl == null)
                    {
                        lbl = obj.AddComponent<LoopBlocksList>();
                    }
                    lbl.fireLoopBlocks = new List<GameObject>();
                    obj.AddComponent<FireLoopData>().fl = new FireLoop();
                    FireLoop newFl = obj.GetComponent<FireLoopData>().fl;
                    newFl.loopTime = saveData.fireLoops[LoadFireLoopCount].loopTime;
                    newFl.loopList = saveData.fireLoops[LoadFireLoopCount].loopList;
                    LoadFireLoopCount++;
                }
            }
            if (groupIndex >= 0 && saveData.groupList != null && saveData.groupList[groupIndex].Contains(LoadCount))
            {
                obj.transform.SetParent(currentGroup.transform);
                obj.tag = "GroupMember";
            }
            LoadCount++;
        }
    }
}

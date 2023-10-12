using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using System.IO;
using static QuaternionConverter;


//public class WallInfo
//{
//    public Vector2 position;

//    public WallInfo() { }
//    public WallInfo(Vector2 position) { this.position = position; }
//}

//public class EnemyInfo
//{
//    public Vector2 position;
//    public EnemyInfo() { }
//    public EnemyInfo(Vector2 position) { this.position = position; }
//}

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

        RecordData(GameObject.FindGameObjectWithTag("PlayerStart"),saveData);

        var path = Path.Combine(Application.persistentDataPath, fileName + ".json");

        var json = JsonConvert.SerializeObject(saveData,new EditorObjInfoConverter(), new MoveLoopConverter());

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
        saveCount++;
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

        int LoadCount = 0;
        int LoadMoveLoopCount = 0;
        var json = File.ReadAllText(path);
        var saveData = JsonConvert.DeserializeObject<SaveData>(json, new EditorObjInfoConverter(), new MoveLoopConverter());
        foreach (var loadedObj in saveData.objects)
        {
            GameObject obj = Instantiate(EditorObjs[loadedObj.code], loadedObj.pos, loadedObj.rot);
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
            
            LoadCount++;
        }
    }
}

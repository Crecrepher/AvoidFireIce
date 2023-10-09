using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using System.IO;
using static QuaternionConverter;
using static UnityEditor.PlayerSettings;
using System.Xml.Linq;
using static StageSaveLoader;

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
        //public Vector2 playerStartPos;
        public List<EditorObjInfo> objects;
        //public List<WallInfo> walls;
        //public List<EnemyInfo> enemys;
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

    private void Update()
    {
        //if (Input.GetKeyDown(KeyCode.Alpha1))
        //{
        //    Save("Test");
        //    Debug.Log("Saved");
        //}
        //if (Input.GetKeyDown(KeyCode.Alpha2))
        //{
        //    Load("Test");
        //    Debug.Log("Loaded");
        //}
        //if (Input.GetKeyDown(KeyCode.Alpha3))
        //{
        //    Clear();
        //    Debug.Log("Clear");
        //}
    }

    public void Save(string fileName)
    {
        var saveData = new SaveData();
        saveData.objects = new List<EditorObjInfo>();

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
        Debug.Log(path);

        var json = JsonConvert.SerializeObject(saveData,new EditorObjInfoConverter());
        Debug.Log(json);

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
        Debug.Log(Application.persistentDataPath);
        var path = Path.Combine(Application.persistentDataPath, fileName + ".json");
        if (!File.Exists(path))
        { return;}
            
        var json = File.ReadAllText(path);
        var saveData = JsonConvert.DeserializeObject<SaveData>(json, new EditorObjInfoConverter());
        foreach (var loadedObj in saveData.objects)
        {
            GameObject obj = Instantiate(EditorObjs[loadedObj.code], loadedObj.pos, loadedObj.rot);
            if (Defines.instance.isHaveElement(loadedObj.code))
            {
                DangerObject dangerObj = obj.GetComponent<DangerObject>();
                dangerObj.element = (Element)loadedObj.element;
                dangerObj.SetColor();
            }
        }
    }
}

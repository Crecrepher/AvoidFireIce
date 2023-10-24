using Newtonsoft.Json.Linq;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using static StageSaveLoader;

public class CustomStage : MonoBehaviour
{
    private List<string> files;

    public GameObject FileFreefab;
    public GameObject ListArea;
    public ToggleGroup group;
    public GameObject NewFileWindow;
    public TMP_InputField NameBox;
    public GameObject PlayB;
    public Button StartB;
    public Button DeleteB;

    public int selectedNum;

    private void Awake()
    {
		//string directoryPath = Application.persistentDataPath + "\\sys";
		string directoryPath = Path.Combine(Application.persistentDataPath,"sys");
		DirectoryInfo directory = new DirectoryInfo(directoryPath);
        if (!directory.Exists)
        {
            directory.Create();
        }
		//string directoryPath2 = Application.persistentDataPath + "\\CustomLevel";
		string directoryPath2 = Path.Combine(Application.persistentDataPath, "CustomLevel");
		DirectoryInfo directory2 = new DirectoryInfo(directoryPath2);
		if (!directory2.Exists)
        {
            directory2.Create();
        }
    }

    public void Showlists()
    {
        selectedNum = -1;
        this.files = new List<string>();
        var lists = GameObject.FindGameObjectsWithTag("CustomFileList");
        foreach (var list in lists)
        {
            Destroy(list);
        }
        DirectoryInfo directory = new DirectoryInfo(Application.persistentDataPath + "/CustomLevel");
        FileInfo[] files = directory.GetFiles();
        int count = 0;
        foreach (var file in files)
        {
            GameObject listB = Instantiate(FileFreefab);
            listB.transform.SetParent(ListArea.transform);
            listB.GetComponentInChildren<Text>().text = file.Name.Replace(".json", "");
            listB.GetComponent<SaveDataListInfo>().num = count++;
            listB.transform.localScale = Vector3.one;
            Toggle toggle = listB.GetComponent<Toggle>();
            toggle.group = group;
            toggle.onValueChanged.AddListener((value) =>
            {
                if (value)
                {
                    SelectList(toggle);
                }
                PlayB.SetActive(value);
                StartB.interactable = value;
                DeleteB.interactable = value;
            });
            this.files.Add(file.Name);
        }
    }

    public void ActivePlayB(bool on)
    {
        PlayB.SetActive(on);
        StartB.interactable = on;
        DeleteB.interactable = on;
    }
    public void SelectList(Toggle toggle)
    {
        selectedNum = toggle.GetComponent<SaveDataListInfo>().num;
    }

    public void CreateNew(bool on)
    {
        NewFileWindow.SetActive(on);
    }

    public void StartNewEdit()
    {
        PlayerPrefs.SetString("StageName", NameBox.text);
        SceneManager.LoadScene("EditorScene");
    }

    public void Open()
    {
        if (selectedNum == -1)
            return;
        PlayerPrefs.SetString("StageName", (files[selectedNum]).Replace(".json", ""));
        SceneManager.LoadScene("EditorScene");
    }

    public void Delete()
    {
        Debug.Log(selectedNum);
        if (selectedNum == -1)
            return;
        File.Delete(Application.persistentDataPath + "/CustomLevel/" + files[selectedNum]);
        Showlists();
    }

    public void PlayCustomLevel()
    {
        if (selectedNum == -1)
            return;
        PlayerPrefs.SetString("StageName", (Application.persistentDataPath + "/CustomLevel/" + files[selectedNum]).Replace(".json", ""));
        PlayerPrefs.SetInt("StageType", (int)StageType.Custom);
        SceneManager.LoadScene("GameScene");
    }
}

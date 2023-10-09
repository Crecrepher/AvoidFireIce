using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CustomStage : MonoBehaviour
{
    private List<string> files;

    public GameObject FileFreefab;
    public GameObject ListArea;
    public ToggleGroup group;
    public GameObject NewFileWindow;
    public TMP_InputField NameBox;

    public int selectedNum;
    public void Showlists()
    {
        this.files = new List<string>();
        var lists = GameObject.FindGameObjectsWithTag("CustomFileList");
        foreach (var list in lists)
        {
            Destroy(list);
        }
        DirectoryInfo directory = new DirectoryInfo(Application.persistentDataPath);
        FileInfo[] files = directory.GetFiles();
        int count = 0;
        foreach (var file in files) 
        {
            GameObject listB = Instantiate(FileFreefab);
            listB.transform.SetParent(ListArea.transform);
            listB.GetComponentInChildren<Text>().text = file.Name.Replace(".json","");
            listB.GetComponent<SaveDataListInfo>().num = count++;
            Toggle toggle = listB.GetComponent<Toggle>();
            toggle.group = group;
            toggle.onValueChanged.AddListener((value) =>
            {
                if (value)
                {
                    SelectList(toggle);
                }
            });
            this.files.Add(file.Name);
        }
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
        PlayerPrefs.SetString("StageName", Application.persistentDataPath + "/" + NameBox.text);
        SceneManager.LoadScene("EditorScene");
    }

    public void Open()
    {
        if (selectedNum == -1)
            return;
        PlayerPrefs.SetString("StageName", (/*Application.persistentDataPath + "/" +*/ files[selectedNum]).Replace(".json", ""));
        Debug.Log(PlayerPrefs.GetString("StageName"));
        SceneManager.LoadScene("EditorScene");
    }

    public void Delete()
    {
        Debug.Log(selectedNum);
        if(selectedNum == -1)
            return;
        File.Delete(Application.persistentDataPath + "/" + files[selectedNum]);
        Showlists();
    }
}

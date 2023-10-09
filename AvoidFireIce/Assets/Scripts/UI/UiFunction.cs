using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UiFunction : MonoBehaviour
{
    public GameObject MainMenu;
    public GameObject StartMenu;
    public GameObject EditMenu;

    public GameObject StageLevelButtons;
    public GameObject CustomLevelButtons;

    public void ActiveMainUi(bool on)
    {
        MainMenu.SetActive(on);
    }

    public void ActiveStageUi(bool on)
    {
        StartMenu.SetActive(on);
    }

    public void ActiveEditUi(bool on)
    {
        EditMenu.SetActive(on);
    }

    public void ActiveOptionUi(bool on)
    {
        Debug.Log("Options");
    }

    public void GameExit()
    {
        Debug.Log("EndGame");
        Application.Quit();
    }

    public void ActiveStageLevels(bool on)
    {
        StageLevelButtons.SetActive(on);
        CustomLevelButtons.SetActive(!on);
    }

    public void StartStage(string stageNum)
    {
        PlayerPrefs.SetString("StageName", stageNum);
        PlayerPrefs.SetInt("StageType", (int)StageType.Official);
        SceneManager.LoadScene("GameScene");
    }
}

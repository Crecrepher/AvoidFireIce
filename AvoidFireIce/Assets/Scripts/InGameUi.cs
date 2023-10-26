using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class InGameUi : MonoBehaviour
{
    public GameObject PauseMenu;
    public Button NextLevelB;
    public Button PreviousLevelB;

    public TMP_Text PauseLevel;
    public TMP_Text WinPlayTimeTXT;
    public TMP_Text WinDeathCountTXT;
    private float playTime = 0;
    private int deathCount = 0;

    private void Awake()
    {
        deathCount = 0;
        playTime = 0;
        if ((StageType)PlayerPrefs.GetInt("StageType") == StageType.Official)
        {
            PauseLevel.text = PlayerPrefs.GetString("StageName");
        }
        else
        {
            PauseLevel.text = "CustomLevel";
        }
    }
    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            ActivePauseMenu(true);
        }
        playTime += Time.deltaTime;
    }
    public void Pause(bool pause)
    {
        Time.timeScale = pause ? 0f : 1f;
    }

    public void ActivePauseMenu(bool on)
    {
        if (GameManager.instance.isWin && on)
        {
            return;
        }
        Pause(on);
        PauseMenu.SetActive(on);
        //NextLevelB.interactable = 
        if ((StageType)PlayerPrefs.GetInt("StageType") == StageType.Official)
        {
            PreviousLevelB.interactable = true;
            NextLevelB.interactable = GameManager.instance.NextAble();
        }
        else
        {
            PreviousLevelB.interactable = false;
            NextLevelB.interactable = false;
        }

    }

    public void ActiveOnlyPauseMenu(bool on)
    {
        PauseMenu.SetActive(on);

    }

    public void PreviousLevel()
    {
        Time.timeScale =  1f;
        GameManager.instance.PreviousLevel();
    }

    public void NextLevel()
    {
        Time.timeScale = 1f;
        GameManager.instance.NextLevel();
    }
    public void Exit()
    {
        Time.timeScale = 1f;
        switch ((StageType)PlayerPrefs.GetInt("StageType"))
        {
            case StageType.Official:
            case StageType.Custom:
                SceneManager.LoadScene("TitleScene");
                break;
            case StageType.Editing:
                SceneManager.LoadScene("EditorScene");
                break;
        }
    }

    public void DeathPlus()
    {
        deathCount++;
    }
    public void SetWinTXT()
    {
        TimeSpan timeSpan = TimeSpan.FromSeconds(playTime);
        string formattedTime = string.Format("{0:D2}:{1:D2}", timeSpan.Minutes, timeSpan.Seconds);
        WinPlayTimeTXT.text = formattedTime;
        WinDeathCountTXT.text = deathCount.ToString();
        if (PlayerPrefs.GetFloat(PlayerPrefs.GetString("StageName") + "Score") == 0f || playTime < PlayerPrefs.GetFloat(PlayerPrefs.GetString("StageName") + "Score"))
        {
			PlayerPrefs.SetFloat(PlayerPrefs.GetString("StageName") + "Score", playTime);
		}
	}
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UiFunction : MonoBehaviour
{
    public GameObject MainMenu;
    public GameObject StartMenu;
    public GameObject EditMenu;
    public CustomStage CustomStageManager;

    public GameObject StageLevelButtons;

    public GameObject DeleteWarning;
    public GameObject OptionWindow;
    public Slider SFXVolume;
    public Slider BGMVolume;
    public Toggle BGOn;
    public AudioMixer audioMixer;

    public GameObject BGM;
    public GameObject SFX;

    public GameObject Locker2;
    public GameObject Locker3;

    public GameObject EndingObject;

    public void Awake()
    {
        if (PlayerPrefs.GetInt("First") != 1)
        {
            PlayerPrefs.SetFloat("SFX", 1f);
            PlayerPrefs.SetFloat("BGM", 1f);
            PlayerPrefs.SetInt("BG", 1);
        }
        PlayerPrefs.SetInt("DeathCount", 0);
        if ((StageType)PlayerPrefs.GetInt("StageType") == StageType.Editing)
        {
            EditMenu.SetActive(true);
            MainMenu.SetActive(false);
            CustomStageManager.Showlists();
            PlayerPrefs.SetInt("StageType", (int)StageType.Official);
        }
        if (GameObject.FindGameObjectWithTag("Unloaded") == null)
        {
            Instantiate(BGM);
            Instantiate(SFX);
        }
        if (PlayerPrefs.GetInt("Clear") >= 16)
        {
            Locker2.SetActive(false);
        }
        if (PlayerPrefs.GetInt("Clear") >= 27)
        {
            Locker3.SetActive(false);
        }
        PlayerPrefs.SetInt("First", 1);
        GlobalData.instance.PlayMainAudiClip();

        if (PlayerPrefs.GetInt("Clear") >= 36)
        {
            EndingObject.SetActive(true);
        }

		float currentAspectRatio = Screen.width / Screen.height;
        if (currentAspectRatio < 800/480 )
		{
            MainMenu.transform.localScale = new Vector2(0.8f,0.8f);
		}
	}
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

    public void GameExit()
    {
        Application.Quit();
    }

    public void ActiveStageLevels(bool on)
    {
        StageLevelButtons.SetActive(on);
    }

    public void StartStage(string stageNum)
    {
        PlayerPrefs.SetString("StageName", stageNum);
        PlayerPrefs.SetInt("StageType", (int)StageType.Official);
        SceneManager.LoadScene("GameScene");
    }

    public void SetDeleteWarning(bool on)
    {
        DeleteWarning.SetActive(on);
    }

    public void OpenOption(bool on)
    {
        OptionWindow.SetActive(on);
        if (on)
        {
            SFXVolume.value = PlayerPrefs.GetFloat("SFX");
            BGMVolume.value = PlayerPrefs.GetFloat("BGM");
            BGOn.isOn = PlayerPrefs.GetInt("BG") == 1;
        }
        else
        {
            PlayerPrefs.SetFloat("SFX", SFXVolume.value);
            PlayerPrefs.SetFloat("BGM", BGMVolume.value);
            int bg = BGOn.isOn ? 1 : 0;
            PlayerPrefs.SetInt("BG", bg);
        }
    }

    public void BGSwitch()
    {
        GlobalData.instance.isBGOn = BGOn.isOn;
    }

    public void SetMusicVolume()
    {
        audioMixer.SetFloat("BGM", Mathf.Log10(BGMVolume.value) * 20);
    }

    public void SetSFXVolume()
    {
        audioMixer.SetFloat("SFX", Mathf.Log10(SFXVolume.value) * 20);
    }

    public void ButtonSoundPlay()
    {
        ButtonPlaySound.instance.PlayBSound();
    }
}

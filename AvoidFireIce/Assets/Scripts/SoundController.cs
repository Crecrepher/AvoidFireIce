using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SoundController : MonoBehaviour
{
    public GameObject OptionWindow;
    public Slider SFXVolume;
    public Slider BGMVolume;
    public Toggle BGOn;
    public AudioMixer audioMixer;
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
        GameManager.instance.BgCheck();
    }

    public void SetMusicVolume()
    {
        audioMixer.SetFloat("BGM", Mathf.Log10(BGMVolume.value) * 20);
    }

    public void SetSFXVolume()
    {
        audioMixer.SetFloat("SFX", Mathf.Log10(SFXVolume.value) * 20);
    }
}

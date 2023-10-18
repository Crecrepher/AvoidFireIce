using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class GlobalData : MonoBehaviour
{
    public static GlobalData instance
    {
        get
        {
            if (globalData == null)
            {
                globalData = FindObjectOfType<GlobalData>();
            }
            return globalData;
        }
    }

    private static GlobalData globalData;

    public bool isBGOn = true;

    public AudioClip MainMenuClip;
    public AudioClip[] audioClips;
    public AudioMixer mixer;

    private AudioSource audioSource;
    private int currentIndex = -1;
    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
    }

    private void Start()
    {
        mixer.SetFloat("BGM", Mathf.Log10(PlayerPrefs.GetFloat("BGM")) * 20);
        mixer.SetFloat("SFX", Mathf.Log10(PlayerPrefs.GetFloat("SFX")) * 20);
        isBGOn = PlayerPrefs.GetInt("BG") == 1;
    }

    private void PlayRandomAudioClip()
    {
        if (audioClips.Length > 0)
        {
            int randomIndex;
            do
            {
                randomIndex = Random.Range(0, audioClips.Length);
            } while (currentIndex == randomIndex);
            currentIndex = randomIndex;
            audioSource.clip = audioClips[randomIndex];
            audioSource.loop = true;
            audioSource.Play();
        }
        else
        {
            Debug.LogWarning("Audio clips array is empty.");
        }
    }

    public void PlayNextRandomAudioClip()
    {
        audioSource.Stop();
        PlayRandomAudioClip();
    }

    public void PlayMainAudiClip()
    {
        if (audioSource != null)
        {
            audioSource.Stop();
        }
        audioSource.clip = MainMenuClip;
        audioSource.loop = true;
        audioSource.Play();
    }

    public void StopMusic()
    {
        audioSource.Stop();
    }
}

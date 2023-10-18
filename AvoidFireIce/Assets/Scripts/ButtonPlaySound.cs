using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using static Unity.VisualScripting.Member;

public class ButtonPlaySound : MonoBehaviour
{
    public AudioClip Sound;
    public AudioMixerGroup Group;
    private AudioSource source;
    public static ButtonPlaySound instance
    {
        get
        {
            if (buttonPlaySound == null)
            {
                buttonPlaySound = FindObjectOfType<ButtonPlaySound>();
            }
            return buttonPlaySound;
        }
    }

    private static ButtonPlaySound buttonPlaySound;

    private void Awake()
    {
        source = gameObject.AddComponent<AudioSource>();
        source.outputAudioMixerGroup = Group;
    }

    public void PlayBSound()
    {
        source.PlayOneShot(Sound);
    }
}

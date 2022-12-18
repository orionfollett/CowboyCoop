using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public AudioSource audioSource;
    public Dictionary<string, AudioClip> audioClips = new Dictionary<string, AudioClip>();
    private SoundManager instance;

    [Header("Sounds")]
    public AudioClip rifleShot;
    public AudioClip successfulHit;
    public AudioClip successfulKill;


    public void Awake()
    {
        if (instance != null)
        {
            Debug.Log("Only one Sound Manager allowed!");
        }
        else {
            instance = this;
        }

        LoadSounds();
    }

    public void LoadSounds() {
        audioClips.Add("rifleshot", rifleShot);
        audioClips.Add("successfulHit", successfulHit);
        audioClips.Add("successfulKill", successfulKill);
    }

    public void PlaySound(string audioDirection, string soundName) {
        if (audioClips.ContainsKey(soundName))
        {
            //audioSource.clip = audioClips[soundName];
            audioSource.panStereo = ConvertDirectionToStereo(audioDirection);
            audioSource.PlayOneShot(audioClips[soundName]);
        }
        else {
            Debug.Log("Sound not found: " + soundName);
        }
    }

    public float ConvertDirectionToStereo(string audioDirection) {
        switch (audioDirection) {
            case "l":
                return -0.8f;
            case "r":
                return 0.8f;
            default:
                return 0.0f;
        }
    }
}

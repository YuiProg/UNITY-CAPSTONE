using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [Header("Audio Source")]
    [SerializeField] AudioSource SFXSource;
    [Space(5)]

    [Header("Audio Clip")]
    public AudioClip Respawn;
    public AudioClip Slash;
    public AudioClip Block;
    public AudioClip walk;
    public AudioClip run;
    public AudioClip hurt;
    public AudioClip die;
    public AudioClip parry;


    public void PlaySFX(AudioClip audio)
    {
            SFXSource.PlayOneShot(audio);
    }

}

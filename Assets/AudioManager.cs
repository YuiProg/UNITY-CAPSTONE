using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [Header("Audio Source")]
    [SerializeField] AudioSource sfxSource;
    [Space(2)]
    [Header("SFX")]
    public AudioClip NormalAttack;
    public AudioClip HardAttack;
    public AudioClip SpearSkill;
    public AudioClip Block;
    public AudioClip Parry;
    public AudioClip Hurt;

    public void PlaySFX(AudioClip clip)
    {
        sfxSource.PlayOneShot(clip);
    }
}

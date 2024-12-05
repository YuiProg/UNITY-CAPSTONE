using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [Header("Audio Source")]
    [SerializeField] AudioSource sfxSource;
    [Space(2)]
    [Header("SFX PLAYER")]
    public AudioClip NormalAttack;
    public AudioClip HardAttack;
    public AudioClip SpearSkill;
    public AudioClip Block;
    public AudioClip Parry;
    public AudioClip Hurt;
    [Space(2)]
    [Header("SFX FINAL BOSS")]
    public AudioClip FB_Attack;
    public AudioClip FB_Thunder;
    public AudioClip Transform;
    [Space(2)]
    [Header("SFX MAGELLAN BOSS")]
    public AudioClip M_Attack;
    public AudioClip M_Explosion;
    public AudioClip M_Death;
    [Space(2)]
    [Header("SFX HORSE BOSS")]
    public AudioClip H_Attack;
    public AudioClip H_Chase;
    [Space(2)]
    [Header("SFX TONDO BOSS")]
    public AudioClip TB_Attack;
    public AudioClip TB_SLAM;
    public AudioClip TB_Transform;


    public void PlaySFX(AudioClip clip)
    {
        sfxSource.PlayOneShot(clip);
    }

    public void StopSFX()
    {
        sfxSource.Stop();
    }
}

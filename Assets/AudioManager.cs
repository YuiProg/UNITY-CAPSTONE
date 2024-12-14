using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [Header("Audio Source")]
    [SerializeField] AudioSource sfxSource;
    [SerializeField] AudioSource BGFXSource;
    [Space(2)]
    [Header("SFX PLAYER")]
    public AudioClip NormalAttack;
    public AudioClip HardAttack;
    public AudioClip SpearSkill;
    public AudioClip Block;
    public AudioClip Parry;
    public AudioClip Hurt;
    public AudioClip HealSkill;
    public AudioClip BUTTONCLICK;
    public AudioClip BUTTONCLICK2;

    [Space(2)]
    [Header("OTHER SFX")]
    public AudioClip JournalFlip;

    [Space(2)]
    [Header("SFX BGM AREA")]
    public AudioClip FirstArea;
    public AudioClip ForestArea;
    public AudioClip BeachArea;
    public AudioClip TondoArea;
    public AudioClip SpaceArea;
    public AudioClip CaveArea;
    public AudioClip SideQuest;


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
    [Space(2)]
    [Header("SFX GOLEM BOSS")]
    public AudioClip G_Swing;
    public AudioClip G_Roll;
    public AudioClip G_Die;


    bool BGMplaying = false;
    
    private void Update()
    {
        if (!BGMplaying)
        {
            checker();
        }
        
    }
    public void PlaySFX(AudioClip clip)
    {
        sfxSource.PlayOneShot(clip);
        
    }
    public void PlayBGSFX(AudioClip clip)
    {
        BGFXSource.PlayOneShot(clip);
    }
    void checker()
    {
        if (PlayerController.Instance.pState.inIfugaoSFX)
        {
            PlayBGSFX(FirstArea);
            BGMplaying = true;
        }
        else if (PlayerController.Instance.pState.inMactanSFX)
        {
            PlayBGSFX(BeachArea);
            BGMplaying = true;
        }
        else if (PlayerController.Instance.pState.inTondoSFX)
        {
            PlayBGSFX(TondoArea);
            BGMplaying = true;
        }
        else if (PlayerController.Instance.pState.inCaveSFX)
        {
            PlayBGSFX(CaveArea);
            BGMplaying = true;
        }
        else if (PlayerController.Instance.pState.inSpaceSFX)
        {
            PlayBGSFX(SpaceArea);
            BGMplaying = true;
        }
        else if (PlayerController.Instance.pState.inSQSFX)
        {
            PlayBGSFX(SideQuest);
            BGMplaying = true;
        }
    }

    public void StopSFX()
    {
        sfxSource.Stop();
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BtnAudio : MonoBehaviour
{
    AudioManager audiomanager;
    void Start()
    {
        audiomanager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
    }

    public void playClick()
    {
        audiomanager.PlaySFX(audiomanager.BUTTONCLICK2);
    }
}

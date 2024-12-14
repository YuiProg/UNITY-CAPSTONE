using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class JournalBOOK : MonoBehaviour
{
    [SerializeField] GameObject IFUGAO;
    [SerializeField] GameObject MACTAN;
    [SerializeField] GameObject TONDO;
    //lock
    [SerializeField] GameObject Lock;
    [SerializeField] Text locktxt;


    AudioManager audiomanager;

    private void Start()
    {
        audiomanager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
    }
    public void IFUGAOBTN()
    {
        if (PlayerPrefs.GetInt("HERALD GOLEM") == 1)
        {
            IFUGAO.SetActive(true);
            MACTAN.SetActive(false);
            TONDO.SetActive(false);
            Lock.SetActive(false);
            audiomanager.PlaySFX(audiomanager.JournalFlip);
        }
        else
        {
            locktxt.text = "COMPLETE IFUGAO AREA TO UNLOCK PAGE.";
            Lock.SetActive(true);
            IFUGAO.SetActive(false);
            MACTAN.SetActive(false);
            TONDO.SetActive(false);
            audiomanager.PlaySFX(audiomanager.BUTTONCLICK2);
        }
    }

    public void MACTANBTN()
    {
        if (PlayerPrefs.GetInt("MAGELLAN") == 1)
        {
            MACTAN.SetActive(true);
            IFUGAO.SetActive(false);
            TONDO.SetActive(false);
            Lock.SetActive(false);
            audiomanager.PlaySFX(audiomanager.JournalFlip);
        }
        else
        {
            locktxt.text = "COMPLETE MACTAN AREA TO UNLOCK PAGE.";
            Lock.SetActive(true);
            IFUGAO.SetActive(false);
            MACTAN.SetActive(false);
            TONDO.SetActive(false);
            audiomanager.PlaySFX(audiomanager.BUTTONCLICK2);
        }
    }

    public void TONDOBTN()
    {
        if (PlayerPrefs.GetInt("TONDOMBOSS") == 1)
        {
            TONDO.SetActive(true);
            MACTAN.SetActive(false);
            IFUGAO.SetActive(false);
            Lock.SetActive(false);
            audiomanager.PlaySFX(audiomanager.JournalFlip);
        }
        else
        {
            locktxt.text = "COMPLETE BANGKUSAY AREA TO UNLOCK PAGE.";
            Lock.SetActive(true);
            IFUGAO.SetActive(false);
            MACTAN.SetActive(false);
            TONDO.SetActive(false);
            audiomanager.PlaySFX(audiomanager.BUTTONCLICK2);
        }
    }
}

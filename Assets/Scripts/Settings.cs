using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;
using static UnityEngine.Rendering.DebugUI;

public class Settings : MonoBehaviour
{
    public bool isLOW = false;
    public bool isMEDIUM = false;
    public bool isHIGH = false;
    [SerializeField] GameObject Light1;
    [SerializeField] GameObject Light2;
    [SerializeField] GameObject Light3;
    [SerializeField] GameObject Light4;
    [SerializeField] GameObject Light5;
    [SerializeField] GameObject Light6;
    [SerializeField] GameObject Light7;
    [SerializeField] GameObject Light8;
    [SerializeField] GameObject Light9;
    [SerializeField] GameObject Light10;
    [SerializeField] GameObject LOWLIGHT;

    //settingsbtn
    [SerializeField] GameObject settings;
    [SerializeField] GameObject MAIN;
    [SerializeField] GameObject volBTN;

    private void Start()
    {
        if (PlayerPrefs.GetInt("LOW") == 1)
        {
            LOW();
        }
        else if (PlayerPrefs.GetInt("MEDIUM") == 1)
        {
            MEDIUM();
        }
        else if (PlayerPrefs.GetInt("HIGH") == 1)
        {
            HIGH();
        }
    }
    

    public void LOW()
    {
        try
        {
            isLOW = true;
            isHIGH = false;
            isMEDIUM = false;
            Light1.SetActive(false);
            Light2.SetActive(false);
            Light3.SetActive(false);
            Light4.SetActive(false);
            Light5.SetActive(false);
            Light6.SetActive(false);
            Light7.SetActive(false);
            Light8.SetActive(false);
            Light9.SetActive(false);
            Light10.SetActive(false);
            LOWLIGHT.SetActive(true);
        }
        catch (System.Exception e)
        {
            Debug.Log(e.Message);
            throw;
        }
        
    }

    public void MEDIUM()
    {
        try
        {
            isLOW = false;
            isHIGH = false;
            isMEDIUM = true;
            Light1.SetActive(true);
            Light2.SetActive(false);
            Light3.SetActive(false);
            Light4.SetActive(true);
            Light5.SetActive(true);
            Light6.SetActive(true);
            Light7.SetActive(false);
            Light8.SetActive(false);
            Light9.SetActive(true);
            Light10.SetActive(false);
            LOWLIGHT.SetActive(false);
        }
        catch (System.Exception e)
        {
            Debug.Log(e.Message);
            throw;
        }
        
    }

    public void HIGH()
    {
        try
        {
            isLOW = false;
            isHIGH = true;
            isMEDIUM = false;
            Light1.SetActive(true);
            Light2.SetActive(true);
            Light3.SetActive(true);
            Light4.SetActive(true);
            Light5.SetActive(true);
            Light6.SetActive(true);
            Light7.SetActive(true);
            Light8.SetActive(true);
            Light9.SetActive(true);
            Light10.SetActive(true);
            LOWLIGHT.SetActive(false);
        }
        catch (System.Exception e)
        {
            Debug.Log(e.Message);
            throw;
        }
        
    }

    public void settingsBTN()
    {
        MAIN.SetActive(false);
        settings.SetActive(true);
        volBTN.SetActive(true);
    }


    public void MainMenu()
    {
        MAIN.SetActive(true);
        settings.SetActive(false);
        volBTN.SetActive(false);
    }

    public void settingsLOW()
    {
        PlayerPrefs.SetInt("LOW", 1);
        PlayerPrefs.SetInt("MEDIUM", 0);
        PlayerPrefs.SetInt("HIGH", 0);
    }
    public void settingsMED()
    {
        PlayerPrefs.SetInt("LOW", 0);
        PlayerPrefs.SetInt("MEDIUM", 1);
        PlayerPrefs.SetInt("HIGH", 0);
    }
    public void settingsHIGH()
    {
        PlayerPrefs.SetInt("LOW", 0);
        PlayerPrefs.SetInt("MEDIUM", 0);
        PlayerPrefs.SetInt("HIGH", 1);
    }
}

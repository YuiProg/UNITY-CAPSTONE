using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Pause_Menu : MonoBehaviour
{
    [SerializeField] GameObject pauseMenu;
    [SerializeField] GameObject UI;
    [SerializeField] GameObject SettingsUI;
    [SerializeField] GameObject StatsUI;
    [SerializeField] GameObject TUTORIAL;
    //world map
    [SerializeField] GameObject WORLDMAP;
    [SerializeField] GameObject WORLDMAPBTN;
    AudioManager audiomanager;

    private void Start()
    {
        audiomanager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
    }
    private void Update()
    {
        if (PlayerController.Instance.pState.obtainedMAP)
        {
            WORLDMAPBTN.SetActive(true);
        }
        else
        {
            WORLDMAPBTN.SetActive(false);
        }
        
    }
    public void ResumeGame()
    {
        Cursor.visible = false;
        Time.timeScale = 1;
        UI.SetActive(true);
        PlayerController.Instance.pState.isPaused = false;
        pauseMenu.SetActive(false);
        SettingsUI.SetActive(false);
        audiomanager.PlaySFX(audiomanager.BUTTONCLICK);
        
    }

    public void QuitGame()
    {
        Application.Quit();
        audiomanager.PlaySFX(audiomanager.BUTTONCLICK);
    }

    public void Settings()
    {
        pauseMenu.SetActive(false);
        SettingsUI.SetActive(true);
        audiomanager.PlaySFX(audiomanager.BUTTONCLICK);
    }

    public void Stats()
    {
        pauseMenu.SetActive(false);
        StatsUI.SetActive(true);
        audiomanager.PlaySFX(audiomanager.BUTTONCLICK);
    }

    public void statstoMenu()
    {
        StatsUI.SetActive(false);
        pauseMenu.SetActive(true);
        audiomanager.PlaySFX(audiomanager.BUTTONCLICK);
    }

    public void settingstoMenu()
    {
        SettingsUI.SetActive(false);
        pauseMenu.SetActive(true);
        audiomanager.PlaySFX(audiomanager.BUTTONCLICK);
    }

    public void WORLDMAPUI()
    {
        SettingsUI.SetActive(false);
        WORLDMAP.SetActive(true);
        audiomanager.PlaySFX(audiomanager.BUTTONCLICK);
    }

    public void worldmaptosettings()
    {
        WORLDMAP.SetActive(false);
        pauseMenu.SetActive(true);
        audiomanager.PlaySFX(audiomanager.BUTTONCLICK);
    }

    public void tutorialMenu()
    {
        pauseMenu.SetActive(false);
        TUTORIAL.SetActive(true);
        audiomanager.PlaySFX(audiomanager.BUTTONCLICK);
    }
}

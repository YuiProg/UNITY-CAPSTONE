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
    //world map
    [SerializeField] GameObject WORLDMAP;
    [SerializeField] GameObject WORLDMAPBTN;

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

        
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void Settings()
    {
        pauseMenu.SetActive(false);
        SettingsUI.SetActive(true);
    }

    public void Stats()
    {
        pauseMenu.SetActive(false);
        StatsUI.SetActive(true);
    }

    public void statstoMenu()
    {
        StatsUI.SetActive(false);
        pauseMenu.SetActive(true);
    }

    public void settingstoMenu()
    {
        SettingsUI.SetActive(false);
        pauseMenu.SetActive(true);
    }

    public void WORLDMAPUI()
    {
        SettingsUI.SetActive(false);
        WORLDMAP.SetActive(true);
    }

    public void worldmaptosettings()
    {
        WORLDMAP.SetActive(false);
        pauseMenu.SetActive(true);
    }
}

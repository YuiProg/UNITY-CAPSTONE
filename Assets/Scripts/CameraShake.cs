using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraShake : MonoBehaviour
{
    private CinemachineVirtualCamera cam;
    private float shakeIntesity = 3f;
    private float shakeTime = 0.2f;
    [SerializeField] GameObject pauseMenu;
    [SerializeField] GameObject UI;
    [SerializeField] GameObject settingsUI;
    [SerializeField] GameObject statUI;
    [SerializeField] GameObject WORLDMAP;
    [SerializeField] GameObject TUTORIAL;
    [SerializeField] GameObject JOURNAL;

    private float timer;
    private CinemachineBasicMultiChannelPerlin _cbmcp;
    bool isPaused = false;

    public static CameraShake Instance;
    private void Start()
    {
        StopShake();
        Cursor.visible = false;
        pauseMenu.SetActive(false);
        settingsUI.SetActive(false);
    }
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
        cam = GetComponent<CinemachineVirtualCamera>();
    }

    public void ShakeCamera()
    {
        CinemachineBasicMultiChannelPerlin _cbmcp = cam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        _cbmcp.m_AmplitudeGain = shakeIntesity;

        timer = shakeTime;
    }
    void StopShake()
    {
        CinemachineBasicMultiChannelPerlin _cbmcp = cam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        _cbmcp.m_AmplitudeGain = 0f;
        timer = 0;
    }
    bool journalOpen = false;
    void Update()
    {
        if (!PlayerController.Instance.pState.isAlive)
        {
            UI.SetActive(false);
        }
        if (PlayerController.Instance.takingDamage)
        {
            ShakeCamera();
        }
        if (timer > 0)
        {
            timer -= Time.deltaTime;
            if (timer < 0)
            {
                StopShake();
            }
        }
        if (Input.GetKeyDown(KeyCode.Escape) && PlayerController.Instance.pState.canPause)
        {
            if (isPaused)
            {
                TUTORIAL.SetActive(false);
                WORLDMAP.SetActive(false);
                Cursor.visible = false;
                Time.timeScale = 1;
                isPaused = false;
                pauseMenu.SetActive(false);
                UI.SetActive(true);
                settingsUI.SetActive(false);
                statUI.SetActive(false);
                PlayerController.Instance.pState.isPaused = false;
                PlayerController.Instance.pState.canOpenJournal = true;
            }
            else
            {
                PlayerController.Instance.pState.isPaused = true;
                PlayerController.Instance.pState.canOpenJournal = false;
                Cursor.visible = true;
                Time.timeScale = 0;
                UI.SetActive(false);
                settingsUI.SetActive(false);
                JOURNAL.SetActive(false);
                PlayerController.Instance.pState.isNPC = false;
                statUI.SetActive(false);
                isPaused = true;
                pauseMenu.SetActive(true);
            }
        }

        if (Input.GetKeyDown(KeyCode.J) && PlayerController.Instance.pState.hasBOOK && PlayerController.Instance.pState.canOpenJournal)
        {
            if (!journalOpen)
            {
                UI.SetActive(false);
                Cursor.visible = true;
                PlayerController.Instance.pState.isNPC = true;
                JOURNAL.SetActive(true);
                journalOpen = true;
            }
            else
            {
                UI.SetActive(true);
                Cursor.visible = false;
                PlayerController.Instance.pState.isNPC = false;
                JOURNAL.SetActive(false);
                journalOpen = false;
            }
        }
    }
}

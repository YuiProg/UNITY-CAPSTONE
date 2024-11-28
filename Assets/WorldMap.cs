using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WorldMap : MonoBehaviour
{
    [Header("MAP")]
    [SerializeField] GameObject MAP;
    [Space(5)]
    [Header("MAP DETAILS")]
    [SerializeField] GameObject MACTANART;
    [SerializeField] GameObject MACTANSKKILL;
    [SerializeField] GameObject IFUGAOART;
    [SerializeField] GameObject IFUGAOSKILL;
    [Space(5)]
    [Header("BTNS")]
    [SerializeField] GameObject IFUGAOBTN;
    [SerializeField] GameObject IFUGAOENTERBTN;
    [SerializeField] GameObject MACTANBTN;
    [SerializeField] GameObject MACTANENTERBTN;
    [Space(5)]
    [Header("Texts")]
    [SerializeField] Text EnterArea;
    [Space(5)]
    [Header("Teleporter")]
    [SerializeField] Transform MACTANTP;
    [SerializeField] Transform IFUGAOTP;
    [Space(5)]
    [Header("Pause Menu")]
    [SerializeField] GameObject Pausemenu;
    [SerializeField] GameObject UI;


    public int mactan;
    public int ifugao;
    public bool teleporting = false;
    private void Start()
    {
        MACTANART.SetActive(false);
        MACTANSKKILL.SetActive(false);
        MACTANENTERBTN.SetActive(false);
        EnterArea.text = "Select Area";
        unlockCheck();
    }


    void unlockCheck()
    {
        if (PlayerPrefs.GetInt("Ifugao") == 0 || !PlayerPrefs.HasKey("Ifugao"))
        {
            IFUGAOBTN.SetActive(false);
        }

    }

    //mactan
    public void MACTAN()
    {
        mactan++;
        ifugao = 0;
        EnterArea.text = "Enter MACTAN";
        MACTANART.SetActive(true);
        MACTANSKKILL.SetActive(true);
        MACTANENTERBTN.SetActive(true);
        IFUGAOART.SetActive(false);
        IFUGAOSKILL.SetActive(false);
    }

    public void enterMACTAN()
    {
        Time.timeScale = 1f;
        Pausemenu.SetActive(false);
        StartCoroutine(TPMACTAN());
        Cursor.visible = false;
    }

    public void enterIFUGAO()
    {
        Time.timeScale = 1f;
        Pausemenu.SetActive(false);
        StartCoroutine(TPIFUGAO());
        Cursor.visible = false;
    }

    public void IFUGAO()
    {
        mactan = 0;
        ifugao++;
        EnterArea.text = "Enter IFUGAO";
        MACTANART.SetActive(false);
        MACTANSKKILL.SetActive(false);
        MACTANENTERBTN.SetActive(false);
        IFUGAOART.SetActive(true);
        IFUGAOSKILL.SetActive(true);
        IFUGAOENTERBTN.SetActive(true);
    }
    //ifugao


    
    //teleporters
    IEnumerator TPMACTAN()
    {
        
        teleporting = true;
        PlayerController.Instance.pState.Transitioning = true;
        yield return new WaitForSeconds(2.5f);
        PlayerController.Instance.transform.position = MACTANTP.transform.position;
        PlayerController.Instance.health = PlayerController.Instance.maxHealth;
        PlayerController.Instance.shieldCount = PlayerController.Instance.maxShield;
        PlayerController.Instance.potionCount = PlayerController.Instance.maxPotions;
        Save.instance.saveData();
        teleporting = false;
        LevelManager.instance.loadscene("Cave_1");
    }

    IEnumerator TPIFUGAO()
    {
        teleporting = true;
        PlayerController.Instance.pState.Transitioning = true;
        yield return new WaitForSeconds(2.5f);
        PlayerController.Instance.transform.position = IFUGAOTP.transform.position;
        PlayerController.Instance.health = PlayerController.Instance.maxHealth;
        PlayerController.Instance.shieldCount = PlayerController.Instance.maxShield;
        PlayerController.Instance.potionCount = PlayerController.Instance.maxPotions;
        Save.instance.saveData();
        teleporting = false;
        LevelManager.instance.loadscene("Cave_1");
    }

    public void exitWM()
    {

    }

}

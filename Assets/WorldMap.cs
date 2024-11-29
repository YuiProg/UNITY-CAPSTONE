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
    [SerializeField] GameObject MACTANSQART;
    [SerializeField] GameObject MACTANSQSKILL;
    [SerializeField] GameObject IFUGAOART;
    [SerializeField] GameObject IFUGAOSKILL;
    [Space(5)]
    [Header("BTNS")]
    [SerializeField] GameObject IFUGAOBTN;
    [SerializeField] GameObject IFUGAOENTERBTN;
    [SerializeField] GameObject MACTANBTN;
    [SerializeField] GameObject MACTANENTERBTN;
    [SerializeField] GameObject MACTANSQENTERBTN;
    [Space(5)]
    [Header("SIDE QUEST BTNS")]
    [SerializeField] GameObject SIDEQUEST1BTNMACTAN;
    [Space(5)]
    [Header("Texts")]
    [SerializeField] Text EnterArea;
    [Space(5)]
    [Header("Teleporter")]
    [SerializeField] Transform MACTANTP;
    [SerializeField] Transform IFUGAOTP;
    [SerializeField] Transform MACTANSIDEQUEST;
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
        if (PlayerPrefs.GetInt("Mactan") == 0 || !PlayerPrefs.HasKey("Mactan"))
        {
            MACTANBTN.SetActive(false);
            SIDEQUEST1BTNMACTAN.SetActive(false);
        }

    }

    //mactan
    public void MACTAN()
    {
        mactan++;//wala nato katamad na ibahin baka mag iba pa
        ifugao = 0;
        EnterArea.text = "Enter MACTAN";
        MACTANART.SetActive(true);
        MACTANSKKILL.SetActive(true);
        MACTANENTERBTN.SetActive(true);
        IFUGAOART.SetActive(false);
        IFUGAOSKILL.SetActive(false);
        IFUGAOENTERBTN.SetActive(false);
        MACTANSQART.SetActive(false);
        MACTANSQSKILL.SetActive(false);
        MACTANSQENTERBTN.SetActive(false);
    }

    public void MACTANSIDEQUESTART()
    {
        EnterArea.text = "Mactan SideQuest";
        MACTANART.SetActive(false);
        MACTANSKKILL.SetActive(false);
        MACTANENTERBTN.SetActive(false);
        IFUGAOART.SetActive(false);
        IFUGAOSKILL.SetActive(false);
        IFUGAOENTERBTN.SetActive(false);
        MACTANSQART.SetActive(true);
        MACTANSQSKILL.SetActive(true);
        MACTANSQENTERBTN.SetActive(true);
    }

    public void enterMACTAN()
    {
        Time.timeScale = 1f;
        Pausemenu.SetActive(false);
        StartCoroutine(TPMACTAN());
        Cursor.visible = false;
    }

    public void enterMactanSQ()
    {
        Time.timeScale = 1f;
        Pausemenu.SetActive(false);
        StartCoroutine(TPSIDEQUESTMACTAN());
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
        MACTANSQART.SetActive(false);
        MACTANSQSKILL.SetActive(false);
        MACTANSQENTERBTN.SetActive(false);
    }
    //ifugao


    
    //teleporters
    IEnumerator TPMACTAN()
    {

        if (!teleporting)
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
        else
        {
            yield return null;
        }
        
    }

    IEnumerator TPIFUGAO()
    {
        if (!teleporting)
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
        else
        {
            yield return null;
        }
        
    }

    IEnumerator TPSIDEQUESTMACTAN()//SIDE QUEST MACTAN
    {
        if (!teleporting)
        {
            teleporting = true;
            PlayerController.Instance.pState.Transitioning = true;
            yield return new WaitForSeconds(2.5f);
            PlayerController.Instance.transform.position = MACTANSIDEQUEST.transform.position;
            PlayerController.Instance.health = PlayerController.Instance.maxHealth;
            PlayerController.Instance.shieldCount = PlayerController.Instance.maxShield;
            PlayerController.Instance.potionCount = PlayerController.Instance.maxPotions;
            Save.instance.saveData();
            teleporting = false;
            LevelManager.instance.loadscene("Cave_1");
        }
        else
        {
            yield return null;
        }
    }
    public void exitWM()
    {

    }

}

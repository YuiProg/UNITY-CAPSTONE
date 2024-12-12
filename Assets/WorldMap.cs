using System.Collections;
using System.Collections.Generic;
using UnityEditor;
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
    [SerializeField] GameObject IFUGAOSQART;
    [SerializeField] GameObject IFUGAOSQSKILL;
    [SerializeField] GameObject TONDOART;
    [SerializeField] GameObject TONDOSKILL;
    [SerializeField] GameObject TONDOSQART;
    [SerializeField] GameObject TONDOSQSKILL;
    [SerializeField] GameObject SHOPART;
    [SerializeField] GameObject SHOPSKILL;
    [Space(5)]
    [Header("BTNS")]
    [SerializeField] GameObject IFUGAOBTN;
    [SerializeField] GameObject IFUGAOENTERBTN;
    [SerializeField] GameObject IFUGAOSQENTERBTN;
    [SerializeField] GameObject MACTANBTN;
    [SerializeField] GameObject MACTANENTERBTN;
    [SerializeField] GameObject MACTANSQENTERBTN;
    [SerializeField] GameObject TONDOBTN;
    [SerializeField] GameObject TONDOENTERBTN;
    [SerializeField] GameObject TONDOSQENTERBTN;
    [SerializeField] GameObject SHOPBTN;
    [SerializeField] GameObject SHOPENTERBTN;
    [Space(5)]
    [Header("SIDE QUEST BTNS")]
    [SerializeField] GameObject SIDEQUESTBTNIFUGAO;
    [SerializeField] GameObject SIDEQUEST1BTNMACTAN;
    [SerializeField] GameObject SIDEQUEST2BTNTONDO;
    [Space(5)]
    [Header("Texts")]
    [SerializeField] Text EnterArea;
    [Space(5)]
    [Header("Teleporter")]
    [SerializeField] Transform MACTANTP;
    [SerializeField] Transform IFUGAOTP;
    [SerializeField] Transform MACTANSIDEQUEST;
    [SerializeField] Transform TONDOTP;
    [SerializeField] Transform TONDOSQTP;
    [SerializeField] Transform IFUGAOSQTP;
    [SerializeField] Transform SHOPTP;
    [Space(5)]
    [Header("Pause Menu")]
    [SerializeField] GameObject Pausemenu;
    [SerializeField] GameObject UI;

    //wala black lang
    [SerializeField] GameObject BLACK;
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
    private void Update()
    {
        unlockCheck();
    }
    void unlockCheck()
    {
        if (PlayerPrefs.GetInt("Mactan") == 0 || !PlayerPrefs.HasKey("Mactan"))
        {
            MACTANBTN.SetActive(false);
            SIDEQUEST1BTNMACTAN.SetActive(false);
        }
        else if (PlayerPrefs.GetInt("SIDEQUESTCOMP") == 1)
        {
            SIDEQUEST1BTNMACTAN.SetActive(false);
        }
        else
        {
            MACTANBTN.SetActive(true);
            SIDEQUEST1BTNMACTAN.SetActive(true);
        }
        if (PlayerPrefs.GetInt("Tondo") == 0 || !PlayerPrefs.HasKey("Tondo"))
        {
            TONDOBTN.SetActive(false);
            SIDEQUEST2BTNTONDO.SetActive(false);
        }
        else if(PlayerPrefs.GetInt("SIDEQUEST3COMP") == 1)
        {
            SIDEQUEST2BTNTONDO.SetActive(false);
        }
        else
        {
            TONDOBTN.SetActive(true);
            SIDEQUEST2BTNTONDO.SetActive(true);
        }
        if (PlayerPrefs.GetInt("SIDEQUEST2COMP") == 1)
        {
            SIDEQUESTBTNIFUGAO.SetActive(false);
        }
        else
        {
            SIDEQUESTBTNIFUGAO.SetActive(true);
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
        TONDOART.SetActive(false);
        TONDOENTERBTN.SetActive(false);
        TONDOSQART.SetActive(false);
        TONDOSKILL.SetActive(false);
        TONDOSQSKILL.SetActive(false);
        TONDOSQENTERBTN.SetActive(false);
        IFUGAOSQART.SetActive(false);
        IFUGAOSQSKILL.SetActive(false);
        IFUGAOENTERBTN.SetActive(false);
        SHOPART.SetActive(false);
        SHOPSKILL.SetActive(false);
        SHOPENTERBTN.SetActive(false);
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
        TONDOART.SetActive(false);
        TONDOENTERBTN.SetActive(false);
        TONDOSQART.SetActive(false);
        TONDOSKILL.SetActive(false);
        TONDOSQSKILL.SetActive(false);
        TONDOSQENTERBTN.SetActive(false);
        IFUGAOSQART.SetActive(false);
        IFUGAOSQSKILL.SetActive(false);
        IFUGAOENTERBTN.SetActive(false);
        SHOPART.SetActive(false);
        SHOPSKILL.SetActive(false);
        SHOPENTERBTN.SetActive(false);
    }

    public void TONDO()
    {
        EnterArea.text = "Enter Tondo";
        MACTANART.SetActive(false);
        MACTANSKKILL.SetActive(false);
        MACTANENTERBTN.SetActive(false);
        IFUGAOART.SetActive(false);
        IFUGAOSKILL.SetActive(false);
        IFUGAOENTERBTN.SetActive(false);
        MACTANSQART.SetActive(false);
        MACTANSQSKILL.SetActive(false);
        MACTANSQENTERBTN.SetActive(false);
        TONDOART.SetActive(true);
        TONDOENTERBTN.SetActive(true);
        TONDOSQART.SetActive(false);
        TONDOSKILL.SetActive(true);
        TONDOSQSKILL.SetActive(false);
        TONDOSQENTERBTN.SetActive(false);
        IFUGAOSQART.SetActive(false);
        IFUGAOSQSKILL.SetActive(false);
        IFUGAOENTERBTN.SetActive(false);
        SHOPART.SetActive(false);
        SHOPSKILL.SetActive(false);
        SHOPENTERBTN.SetActive(false);
    }

    public void TONDOSIDEQUESTART()
    {
        EnterArea.text = "Tondo SideQuest";
        MACTANART.SetActive(false);
        MACTANSKKILL.SetActive(false);
        MACTANENTERBTN.SetActive(false);
        IFUGAOART.SetActive(false);
        IFUGAOSKILL.SetActive(false);
        IFUGAOENTERBTN.SetActive(false);
        MACTANSQART.SetActive(false);
        MACTANSQSKILL.SetActive(false);
        MACTANSQENTERBTN.SetActive(false);
        TONDOART.SetActive(false);
        TONDOENTERBTN.SetActive(false);
        TONDOSQART.SetActive(true);
        TONDOSKILL.SetActive(false);
        TONDOSQSKILL.SetActive(true);
        TONDOSQENTERBTN.SetActive(true);
        IFUGAOSQART.SetActive(false);
        IFUGAOSQSKILL.SetActive(false);
        IFUGAOENTERBTN.SetActive(false);
        SHOPART.SetActive(false);
        SHOPSKILL.SetActive(false);
        SHOPENTERBTN.SetActive(false);
    }

    public void IFUGAOSIDEQUESTART()
    {
        EnterArea.text = "Ifugao SideQuest";
        MACTANART.SetActive(false);
        MACTANSKKILL.SetActive(false);
        MACTANENTERBTN.SetActive(false);
        IFUGAOART.SetActive(false);
        IFUGAOSKILL.SetActive(false);
        IFUGAOENTERBTN.SetActive(false);
        MACTANSQART.SetActive(false);
        MACTANSQSKILL.SetActive(false);
        MACTANSQENTERBTN.SetActive(false);
        TONDOART.SetActive(false);
        TONDOENTERBTN.SetActive(false);
        TONDOSQART.SetActive(false);
        TONDOSKILL.SetActive(false);
        TONDOSQSKILL.SetActive(false);
        TONDOSQENTERBTN.SetActive(false);
        IFUGAOSQART.SetActive(true);
        IFUGAOSQSKILL.SetActive(true);
        IFUGAOSQENTERBTN.SetActive(true);
        SHOPART.SetActive(false);
        SHOPSKILL.SetActive(false);
        SHOPENTERBTN.SetActive(false);
    }
    public void SHOPMAINART()
    {
        EnterArea.text = "Balweg's Shop";
        MACTANART.SetActive(false);
        MACTANSKKILL.SetActive(false);
        MACTANENTERBTN.SetActive(false);
        IFUGAOART.SetActive(false);
        IFUGAOSKILL.SetActive(false);
        IFUGAOENTERBTN.SetActive(false);
        MACTANSQART.SetActive(false);
        MACTANSQSKILL.SetActive(false);
        MACTANSQENTERBTN.SetActive(false);
        TONDOART.SetActive(false);
        TONDOENTERBTN.SetActive(false);
        TONDOSQART.SetActive(false);
        TONDOSKILL.SetActive(false);
        TONDOSQSKILL.SetActive(false);
        TONDOSQENTERBTN.SetActive(false);
        IFUGAOSQART.SetActive(false);
        IFUGAOSQSKILL.SetActive(false);
        IFUGAOSQENTERBTN.SetActive(false);
        SHOPART.SetActive(true);
        SHOPSKILL.SetActive(true);
        SHOPENTERBTN.SetActive(true);
    }
    public void enterMACTAN()
    {
        Time.timeScale = 1f;
        Pausemenu.SetActive(false);
        StartCoroutine(TPMACTAN());
        Cursor.visible = false;
    }
    public void enterTONDO()
    {
        Time.timeScale = 1f;
        Pausemenu.SetActive(false);
        StartCoroutine(TPITONDO());
        Cursor.visible = false;
    }
    public void enterTONDOSQ()
    {
        Time.timeScale = 1f;
        Pausemenu.SetActive(false);
        StartCoroutine(TPSIDEQUESTTONDO());
        Cursor.visible = false;
    }
    
    public void enterIFUGAOSQ()
    {
        Time.timeScale = 1f;
        Pausemenu.SetActive(false);
        StartCoroutine(IFUGAOSQ());
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
    
    public void enterSHOP()
    {
        Time.timeScale = 1f;
        Pausemenu.SetActive(false);
        StartCoroutine(SHOPTPFUNC());
        Cursor.visible = false;
    }


    public void IFUGAO()
    {
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
        TONDOART.SetActive(false);
        TONDOENTERBTN.SetActive(false);
        TONDOSQART.SetActive(false);
        TONDOSKILL.SetActive(false);
        TONDOSQSKILL.SetActive(false);
        TONDOSQENTERBTN.SetActive(false);
        IFUGAOSQART.SetActive(false);
        IFUGAOSQSKILL.SetActive(false);
        SHOPART.SetActive(false);
        SHOPSKILL.SetActive(false);
        SHOPENTERBTN.SetActive(false);
    }
    //ifugao


    
    //teleporters
    IEnumerator SHOPTPFUNC()
    {
        if (!teleporting)
        {
            teleporting = true;
            PlayerController.Instance.pState.Transitioning = true;
            yield return new WaitForSeconds(2.5f);
            MAP.SetActive(false);
            PlayerController.Instance.pState.Transitioning = false;
            PlayerController.Instance.transform.position = SHOPTP.transform.position;
            PlayerController.Instance.health = PlayerController.Instance.maxHealth;
            PlayerController.Instance.shieldCount = PlayerController.Instance.maxShield;
            PlayerController.Instance.potionCount = PlayerController.Instance.maxPotions;
            Save.instance.saveStats();
            PlayerController.Instance.pState.isNPC = false;
            PlayerController.Instance.pState.canPause = true;
            PlayerController.Instance.pState.isPaused = false;
            UI.SetActive(true);
            teleporting = false;
        }
        else
        {
            yield return null;
        }
    }
    IEnumerator IFUGAOSQ()
    {
        if (!teleporting)
        {
            teleporting = true;
            PlayerController.Instance.pState.Transitioning = true;
            yield return new WaitForSeconds(2f);
            BLACK.SetActive(true);
            PlayerController.Instance.transform.position = IFUGAOSQTP.transform.position;
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
    IEnumerator TPMACTAN()
    {

        if (!teleporting)
        {
            teleporting = true;
            PlayerController.Instance.pState.Transitioning = true;
            yield return new WaitForSeconds(2f);
            BLACK.SetActive(true);
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
            yield return new WaitForSeconds(2f);
            BLACK.SetActive(true);
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
    IEnumerator TPITONDO()
    {
        if (!teleporting)
        {
            teleporting = true;
            PlayerController.Instance.pState.Transitioning = true;
            yield return new WaitForSeconds(2f);
            BLACK.SetActive(true);
            PlayerController.Instance.transform.position = TONDOTP.transform.position;
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
            yield return new WaitForSeconds(2f);
            BLACK.SetActive(true);
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
    IEnumerator TPSIDEQUESTTONDO()//SIDE QUEST TONDO
    {
        if (!teleporting)
        {
            teleporting = true;
            PlayerController.Instance.pState.Transitioning = true;
            yield return new WaitForSeconds(2f);
            BLACK.SetActive(true);
            PlayerController.Instance.transform.position = TONDOSQTP.transform.position;
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

using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class IFUGAONPC : MonoBehaviour
{
    //DLG
    [SerializeField] GameObject DLG;
    [SerializeField] Text Dialogue;
    [SerializeField] Text NameNPC;
    [SerializeField] GameObject UI;

    //loc
    [SerializeField] Transform loc2;

    //border
    [SerializeField] GameObject BORDER;
    [SerializeField] GameObject BORDER2;
    //notif
    [SerializeField] Text notif;

    //world map
    [SerializeField] GameObject worldMap;
    bool onTrigger;
    bool isTalking = false;
    bool haskilled = false;
    private void Start()
    {
        if (PlayerPrefs.GetInt("IFUGAONPC") == 1)
        {
            BORDER.SetActive(false);
        }
        BORDER2.SetActive(false);
    }
    private void Update()
    {
        
        flip();
        if (PlayerPrefs.GetInt("IFUGAONPC") == 1) notif.text = "";
        if (onTrigger && Input.GetKeyDown(KeyCode.E) && PlayerPrefs.GetInt("IFUGAONPC") != 1)
        {
            if (!isTalking)
            {
                if (PlayerPrefs.GetInt("HERALD GOLEM") != 1)
                {
                    StartCoroutine(DIALOGUE(4.5f));
                }
                else
                {
                    BORDER2.SetActive(true);
                    StartCoroutine(Dialogue2(4.5f));
                }
            }
        }

        if (PlayerPrefs.GetInt("HERALD GOLEM") == 1) transform.position = loc2.transform.position;
        if (PlayerPrefs.GetInt("IFUGAONPC") == 1) BORDER2.SetActive(false);

        if (PlayerPrefs.GetInt("HERALD GOLEM") == 1 && !haskilled)
        {
            haskilled = true;
            BORDER2.SetActive(true);
        }
    }


    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            onTrigger = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            onTrigger = false;
        }
    }

    IEnumerator DIALOGUE(float time)
    {
        DLG.SetActive(true);
        UI.SetActive(false);
        notif.text = "";
        isTalking = true;
        PlayerController.Instance.pState.isNPC = true;
        PlayerController.Instance.pState.canPause = false;
        string[] words = new[]
        {
            "Ang aming lugar ay sinalakay ng isang malaking batong gumagalaw na hindi ko maipaliwanag at mga lalaking may espada na naka itim na kapa.",
            "Hindi po nandito ako para tumulong.",
            "No where are they I can help.",
            "We are on time"
        };

        string[] name = new[]
        {
            "Igorot Elder Tribe",
            "Igorot Elder Tribe",
            "Zeick",
            "Gilmoure of Ages",
        };


        for (int i = 0; i < words.Length; i++)
        {
            Dialogue.text = words[i];
            NameNPC.text = name[i];

            float elapsedTime = 0f;
            while (elapsedTime < time)
            {
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    elapsedTime = time;
                    break;
                }
                else if (Input.GetKeyDown(KeyCode.F))
                {
                    elapsedTime = time;
                }
                elapsedTime += Time.deltaTime;
                yield return null;
            }
        }
        DLG.SetActive(false);
        NameNPC.text = "";
        Dialogue.text = "";
        QuestTracker.instance.hasQuest = true;
        PlayerPrefs.SetString("Quest", "Defeat the invader and unknown creature.");
        UI.SetActive(true);
        BORDER.SetActive(false);
        PlayerController.Instance.pState.isNPC = false;
        PlayerController.Instance.pState.canPause = true;
        isTalking = false;
        
        
    }

    IEnumerator Dialogue2(float time)
    {
        PlayerController.Instance.pState.isNPC = true;
        PlayerController.Instance.pState.canPause = false;
        UI.SetActive(false);
        notif.text = "";
        DLG.SetActive(true);
        PlayerController.Instance.pState.canPause = false;
        Dialogue.text = "Salamat estranghero, nawa'y makatulong sa iyong paglalakbay ang sibat na ito";
        NameNPC.text = "Igorot Leader";
        PlayerPrefs.SetInt("SPEAR", 1);
        yield return new WaitForSeconds(time);
        Dialogue.text = "";
        NameNPC.text = "";
        DLG.SetActive(false);
        PlayerController.Instance.pState.SkillBOSS = true;
        PlayerPrefs.SetInt("Mactan", 1);
        QuestTracker.instance.hasQuest = false;
        yield return new WaitForSeconds(time - 2);
        DLG.SetActive(true);
        Dialogue.text = "PRESS 1 - 3 AND THEN R TO USE SKILL.";
        yield return new WaitForSeconds(time);
        PlayerPrefs.DeleteKey("Quest");
        PlayerPrefs.SetInt("IFUGAONPC", 1);
        Dialogue.text = "";
        DLG.SetActive(false);
        BORDER2.SetActive(false);
        yield return new WaitForSeconds(time - 2);
        PlayerController.Instance.pState.Transitioning = true;
        yield return new WaitForSeconds(time - 2);
        PlayerController.Instance.pState.Transitioning = false;
        worldMap.SetActive(true);
        Cursor.visible = true;
        
    }

    void flip()
    {
        if (PlayerController.Instance.transform.position.x > transform.position.x)
        {
            transform.eulerAngles = new Vector3(0, 180, 0);
        }
        else
        {
            transform.eulerAngles = new Vector3(0, 0, 0);
        }
    }
}

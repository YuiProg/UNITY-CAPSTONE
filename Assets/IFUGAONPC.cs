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

    //loc
    [SerializeField] Transform loc2;

    //border
    [SerializeField] GameObject BORDER;
    [SerializeField] GameObject BORDER2;

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
        isTalking = true;
        PlayerController.Instance.pState.isNPC = true;
        PlayerController.Instance.pState.canPause = false;
        string[] words = new[]
        {
            "Our village has been raided by a huge unknown creature held by a man with a cloak",
            "Are you with them?",
            "No where are they I can help.",
            "We are on time"
        };

        string[] name = new[]
        {
            "Igorot Leader",
            "Igorot Leader",
            "",
            "Gilmoure Book",
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
                elapsedTime += Time.deltaTime;
                yield return null;
            }
        }
        DLG.SetActive(false);
        NameNPC.text = "";
        Dialogue.text = "";
        QuestTracker.instance.hasQuest = true;
        PlayerPrefs.SetString("Quest", "Defeat the invader and unknown creature.");
        BORDER.SetActive(false);
        PlayerController.Instance.pState.isNPC = false;
        PlayerController.Instance.pState.canPause = true;
        isTalking = false;
        
        
    }

    IEnumerator Dialogue2(float time)
    {
        PlayerController.Instance.pState.isNPC = true;
        DLG.SetActive(true);
        PlayerController.Instance.pState.canPause = false;
        Dialogue.text = "Thank you stranger may this spear  help you in your journey";
        NameNPC.text = "Igorot Leader";
        PlayerPrefs.SetInt("SPEAR", 1);
        yield return new WaitForSeconds(time);
        PlayerController.Instance.pState.SkillBOSS = true;
        QuestTracker.instance.hasQuest = false;
        PlayerPrefs.DeleteKey("Quest");
        PlayerPrefs.SetInt("IFUGAONPC", 1);
        Dialogue.text = "";
        DLG.SetActive(false);
        BORDER2.SetActive(false);
        PlayerController.Instance.pState.isNPC = false;
        PlayerController.Instance.pState.canPause = true;
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

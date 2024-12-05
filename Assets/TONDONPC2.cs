using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class TONDONPC2 : MonoBehaviour
{
    [SerializeField] GameObject UI;
    [SerializeField] GameObject DIALOGUE;
    [SerializeField] Text dialogue;
    [SerializeField] Text nameNPC;
    [SerializeField] GameObject BORDER;
    [SerializeField] Text notif;
    bool inTrigger;
    bool isTalking = false;

    void Update()
    {
        if (PlayerPrefs.GetInt("TONDOBOSSNPC") != 1) notif.text = "!";
        if (PlayerPrefs.GetInt("TONDOBOSSNPC") == 1) BORDER.SetActive(false);
        if (inTrigger && !isTalking && Input.GetKeyDown(KeyCode.E) && PlayerPrefs.GetInt("TONDOBOSSNPC") != 1)
        {
            StartCoroutine(Dialogue1(4.5f));
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            inTrigger = true;
        }    
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            inTrigger = false;
        }
    }

    IEnumerator Dialogue1(float time)
    {
        notif.text = "";
        Cursor.visible = true;
        UI.SetActive(false);
        PlayerController.Instance.pState.canPause = false;
        PlayerController.Instance.pState.isNPC = true;
        DIALOGUE.SetActive(true);

        string[] words = new[]
        {
            "Sultan! No!",
            "Do not let my sacrifice be in vain. Fight, Zieck. End this.",
            "Your sacrifice will not be forgotten. I will honor you with victory!",
        };

        string[] names = new[]
        {
            "",
            "Sultan Sulayman",
            ""
        };

        for (int i = 0; i < words.Length; i++)
        {
            float elapsedtime = 0f;
            dialogue.text = words[i];
            nameNPC.text = names[i];
            while (elapsedtime < time)
            {
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    elapsedtime = time;
                    break;
                }
                elapsedtime += Time.deltaTime;
                yield return null;
            }
        }
        notif.text = "";
        DIALOGUE.SetActive(false);
        PlayerController.Instance.pState.canPause = true;
        PlayerController.Instance.pState.isNPC = false;
        QuestTracker.instance.hasQuest = true;
        PlayerPrefs.SetString("Quest", "Defeat Area BOSS.");
        PlayerPrefs.SetInt("TONDOBOSSNPC", 1);
        BORDER.SetActive(false);
        Cursor.visible = false;
        UI.SetActive(true);
    }


}

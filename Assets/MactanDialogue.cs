using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MactanDialogue : MonoBehaviour
{
    bool inTrigger;
    bool isTalking = false;
    [SerializeField] GameObject UI;
    [SerializeField] GameObject DIALOGUE;
    [SerializeField] Text dlg;
    [SerializeField] Text npcName;
    void Update()
    {
        if (inTrigger && PlayerPrefs.GetInt("MACTANNPCTRIGGER") != 1 && !isTalking)
        {
            StartCoroutine(dialogue1(4.5f));
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

    IEnumerator dialogue1(float time)
    {
        isTalking = true;
        PlayerController.Instance.pState.isNPC = true;
        PlayerController.Instance.pState.canPause = false;
        PlayerController.Instance.pState.canOpenJournal = false;
        UI.SetActive(false);
        DIALOGUE.SetActive(true);

        string[] words = new[]
        {
            "Looks like we're late.",
            "We need to go through this mountain to find the battle of mactan."
        };

        string[] names = new[]
        {
            "Gilmoire of Ages",
            "Gilmoire of Ages"
        };

        for (int i = 0; i < words.Length; i++)
        {
            dlg.text = words[i];
            npcName.text = names[i];
            float elapsedtime = 0f;
            while (elapsedtime < time)
            {
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    elapsedtime = time;
                    break;
                }
                else if (Input.GetKeyDown(KeyCode.F))
                {
                    elapsedtime = time;
                }
                elapsedtime += Time.deltaTime;
                yield return null;
            }
        }
        DIALOGUE.SetActive(false);
        UI.SetActive(true);
        QuestTracker.instance.hasQuest = true;
        PlayerPrefs.SetInt("MACTANNPCTRIGGER", 1);
        PlayerPrefs.SetString("Quest", "Clear the camp of magellan");
        PlayerController.Instance.pState.isNPC = false;
        PlayerController.Instance.pState.canPause = true;
        PlayerController.Instance.pState.canOpenJournal = true;
        isTalking = false;
    }
}

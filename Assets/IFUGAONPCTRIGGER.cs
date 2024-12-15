using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IFUGAONPCTRIGGER : MonoBehaviour
{
    [SerializeField] GameObject NPC;
    [SerializeField] GameObject DIALOGUE;
    [SerializeField] Text dlg;
    [SerializeField] Text npcName;
    [SerializeField] GameObject UI;


    bool intrigger;
    bool activated = false;
    private void Start()
    {
        NPC.SetActive(PlayerPrefs.GetInt("IFUGAONPC2") == 1);
    }
    private void Update()
    {
        
        if (!activated && intrigger && PlayerPrefs.GetInt("IFUGAONPC2") != 1)
        {
            StartCoroutine(dialogue1(4.5f));
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            intrigger = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            intrigger = false;
        }
    }

    IEnumerator dialogue1(float time)
    {
        UI.SetActive(false);
        PlayerController.Instance.pState.canOpenJournal = false;
        PlayerController.Instance.pState.canPause = false;
        QuestTracker.instance.hasQuest = false;
        PlayerPrefs.DeleteKey("Quest");
        PlayerController.Instance.pState.isNPC = true;
        activated = true;
        PlayerController.Instance.pState.Transitioning = true;
        yield return new WaitForSeconds(time - 2);
        PlayerController.Instance.pState.Transitioning = false;
        yield return new WaitForSeconds(time - 2);
        DIALOGUE.SetActive(true);
        NPC.SetActive(true);
        string[] words = new[]
        {
            "Stop right there! Who are you? Are you one of them?",
            "No, I’m not with them.",
            "Do not lie to me!",
            "What are you doing here!?",
            "I’m not your enemy. I have no ties to the Spaniards.",
            "If that’s true, then prove it. To earn our trust, you must defeat the knight hiding at the top of the mountain.",
            "We will be watching you. Don’t try anything foolish."
        };

        string[] names = new[]
        {
            "Tausug Scout",
            "Zieck",
            "Tausug Scout",
            "Tausug Scout",
            "Zieck",
            "Tausug Scout",
            "Tausug Scout"
        };

        for (int i = 0; i < words.Length; i++)
        {
            float elapsedtime = 0f;
            dlg.text = words[i];
            npcName.text = names[i];
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
        PlayerPrefs.SetInt("IFUGAONPC2", 1);
        QuestTracker.instance.hasQuest = true;
        PlayerPrefs.SetString("Quest", "Defeat the knight");
        DIALOGUE.SetActive(false);
        PlayerController.Instance.pState.isNPC = false;
        PlayerController.Instance.pState.canPause = true;
        PlayerController.Instance.pState.canOpenJournal = true;
        UI.SetActive(true);
    }
}

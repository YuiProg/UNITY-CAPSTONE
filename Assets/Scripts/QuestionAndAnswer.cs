using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class QuestionAndAnswer : MonoBehaviour
{
    [SerializeField] GameObject QNA;
    [SerializeField] GameObject InputAnswer;
    [SerializeField] GameObject NPCDIALOGUE;
    [SerializeField] GameObject STATUE1;
    [SerializeField] GameObject STATUE2;
    [SerializeField] Text dialogue;
    [SerializeField] GameObject BORDER;
    [SerializeField] GameObject UI;
    [SerializeField] GameObject worldMap;

    public bool inTrigger = false;
    bool isTalking = false;
    private void Start()
    {
        QNA.SetActive(false);
        InputAnswer.SetActive(false);
        if (PlayerPrefs.GetInt("Desert") == 1)
        {
            BORDER.SetActive(false);
        }
    }
    private void Update()
    {
        if (inTrigger && Input.GetKeyDown(KeyCode.E) && !isTalking)
        {
            if (PlayerPrefs.GetInt("Bandit") != 1 && PlayerPrefs.GetInt("SideQuest1") != 1)
            {
                isTalking = true;
                StartCoroutine(DialogueNPCQNA(4.5f));
            }
            if (PlayerPrefs.GetInt("Bandit") == 1 && PlayerPrefs.GetInt("SideQuest1") == 1)
            {
                isTalking = true;
                StartCoroutine(EssenceDialogue(4.5f));
            }
        }

        if (PlayerPrefs.GetInt("SideQuest1") == 1) BORDER.SetActive(false);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            inTrigger = false;
        }
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            inTrigger = true;
        }
    }

    
    IEnumerator EssenceDialogue(float time)
    {
        UI.SetActive(false);
        QuestTracker.instance.hasQuest = false;
        PlayerPrefs.DeleteKey("Quest");
        Cursor.visible = true;
        PlayerController.Instance.pState.isNPC = true;
        NPCDIALOGUE.SetActive(true);
        string[] dialogues = new[]
        {
            "Well done. His sacrifice and his work on our country lives on",
            "..."
        };

        for (int i = 0; i < dialogues.Length; i++)
        {
            
            dialogue.text = dialogues[i];
            float waitforseconds = 0f;
            while (waitforseconds < time)
            {
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    waitforseconds = time;
                    break;
                }
                waitforseconds += Time.deltaTime;
                yield return null;
            }
            
        }
        NPCDIALOGUE.SetActive(false);
        PlayerController.Instance.pState.Transitioning = true;
        yield return new WaitForSeconds(time);
        PlayerController.Instance.pState.Transitioning = false;
        STATUE2.SetActive(true);
        STATUE1.SetActive(false);
        yield return new WaitForSeconds(time);
        NPCDIALOGUE.SetActive(true);
        dialogue.text = "You may now proceed on your adventure";
        yield return new WaitForSeconds(time);
        dialogue.text = "";
        NPCDIALOGUE.SetActive(false);
        PlayerPrefs.SetInt("SIDEQUESTCOMP", 1);
        PlayerController.Instance.pState.Transitioning = true;
        yield return new WaitForSeconds(time - 2);
        PlayerController.Instance.pState.Transitioning = false;
        worldMap.SetActive(true);
        Save.instance.saveData();      
    }

    IEnumerator DialogueNPCQNA(float time)
    {
        Cursor.visible = true;
        PlayerController.Instance.pState.isNPC = true;
        UI.SetActive(false);
        NPCDIALOGUE.SetActive(true);

        string[] dialogues = new string[]
        {
            "We've come so far, but there's one last puzzle.",
            "They say these were his last words will be the key to broke this curse statue,",
            "a declaration that his mission was complete.",
            "It's a phrase in Latin. It will broke this curse nameless hero"
        };

        for (int i = 0; i < dialogues.Length; i++)
        {
            float elapsedtime = 0f;
            dialogue.text = dialogues[i];
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

        QNA.SetActive(true);
        showQNAUI();
    }

    void showQNAUI()
    {
        StopCoroutine(DialogueNPCQNA(4.5f));
        dialogue.text = "It's a phrase in Latin. It will broke this curse nameless hero";
        InputAnswer.SetActive(true);
    }

    IEnumerator CORRECTANSWER(float time)
    {
        InputAnswer.SetActive(false);

        string[] dialogues = new[]
        {
            "Yes... the curse has been broke but you can't leave here without the heroic essence.",
            "You must defeat Panday Tulisán to obtain his essence.",
        };

        for (int i = 0; i < dialogues.Length; i++)
        {
            dialogue.text = dialogues[i];
            float elapsedtime = 0f;
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
        PlayerPrefs.SetInt("SideQuest1", 1);
        dialogue.text = "";
        NPCDIALOGUE.SetActive(false);
        Cursor.visible = false;
        PlayerController.Instance.pState.isNPC = false;
        UI.SetActive(true);
        isTalking = false;
        Destroy(BORDER);
    }

    public void CloseUI()
    {
        StartCoroutine(CORRECTANSWER(4.5f));
    }

    public void WrongAnswer()
    {
        dialogue.text = "Well... Looks like you're not prepared yet.";
        InputAnswer.SetActive(false);
        PlayerController.Instance.health = PlayerController.Instance.health - 99999;
    }
}

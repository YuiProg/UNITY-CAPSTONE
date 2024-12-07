using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class QuestionAndAnswer2 : MonoBehaviour
{
    [SerializeField] GameObject QNA;
    [SerializeField] GameObject InputAnswer;
    [SerializeField] GameObject NPCDIALOGUE;
    [SerializeField] GameObject STATUE1;
    [SerializeField] GameObject STATUE2;
    [SerializeField] Text dialogue;
    [SerializeField] GameObject BORDER;
    [SerializeField] GameObject UI;
    //world map

    [SerializeField] GameObject WORLDMAP;

    public bool inTrigger = false;
    bool isTalking = false;
    private void Start()
    {
        QNA.SetActive(false);
        InputAnswer.SetActive(false);
    }

    private void Update()
    {
        if (PlayerPrefs.GetInt("SideQuest2") == 1) BORDER.SetActive(false);

        if (inTrigger)
        {
            if (Input.GetKeyDown(KeyCode.E) && !isTalking)
            {
                if (PlayerPrefs.GetInt("Desert") == 1)
                {
                    StartCoroutine(winDLG(4.5f));
                }
                if (PlayerPrefs.GetInt("Desert") != 1 && PlayerPrefs.GetInt("SideQuest2") != 1)
                {
                    StartCoroutine(Dialogue(4.5f));
                }
            }
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

    IEnumerator Dialogue(float time)
    {
        UI.SetActive(false);
        PlayerController.Instance.pState.canPause = false;
        Cursor.visible = true;
        isTalking = true;
        PlayerController.Instance.pState.isNPC = true;
        NPCDIALOGUE.SetActive(true);

        string[] words = new[]
        {
            "To proceed, you must prove your knowledge of history, for the Heroic Essence is not for the ignorant.",
            "He's a brilliant strategist but betrayed by his people. The path of loss of our revolution forces has now been created.",
            "But because of these anomalies, his date of death has been forgotten, causing his heroic essence to be contained.",
            "Answer my question child, what is the date of his death when he was ambushed in Cabanatuan, and betrayed by men that he trusted?"
        };

        for (int i = 0; i < words.Length; i++)
        {
            dialogue.text = words[i];
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
        QNA.SetActive(true);
        showqna();
    }

    void showqna()
    {
        InputAnswer.SetActive(true);
        StopCoroutine(Dialogue(4.5f));
        dialogue.text = "Answer my question child, what is the date of his death when he was ambushed in Cabanatuan, and betrayed by men that he trusted?";
    }

    public void correctanswer()
    {
        InputAnswer.SetActive(false);
        StartCoroutine(dialogue2(4.5f));
    }

    IEnumerator winDLG(float time)
    {
        UI.SetActive(false);
        Cursor.visible = true;
        isTalking = true;
        NPCDIALOGUE.SetActive(true);
        PlayerController.Instance.pState.isNPC = true;
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
        STATUE2.SetActive(true);
        STATUE1.SetActive(false);
        yield return new WaitForSeconds(time - 1);
        PlayerController.Instance.pState.Transitioning = false;
        PlayerPrefs.DeleteKey("Quest");
        NPCDIALOGUE.SetActive(true);
        dialogue.text = "You may now proceed on your adventure";
        yield return new WaitForSeconds(time);
        PlayerController.Instance.pState.Transitioning = true;
        NPCDIALOGUE.SetActive(false);
        yield return new WaitForSeconds(time - 2);
        PlayerController.Instance.pState.Transitioning = false;
        PlayerPrefs.SetInt("SIDEQUEST2COMP", 1);
        dialogue.text = "";
        isTalking = false;
        WORLDMAP.SetActive(true);
    }
    IEnumerator dialogue2(float time)
    {
        PlayerController.Instance.pState.canPause = false;
        Cursor.visible = true;
        isTalking = true;
        PlayerController.Instance.pState.isNPC = true;
        NPCDIALOGUE.SetActive(true);

        string[] words = new[]
        {
            "Before you leave.",
            "You must know that this place has a lot of traps and there is a gate that you need to unlock. Find the lever so you can proceed to the guardian."
        };

        for (int i = 0; i < words.Length; i++)
        {
            dialogue.text = words[i];
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
        NPCDIALOGUE.SetActive(false);
        BORDER.SetActive(false);
        Cursor.visible = false;
        isTalking = false;
        QuestTracker.instance.hasQuest = true;
        PlayerPrefs.SetString("Quest", "Find the lever");
        PlayerPrefs.SetInt("SideQuest2", 1);
        PlayerController.Instance.pState.canPause = true;
        PlayerController.Instance.pState.isNPC = false;
        UI.SetActive(true);
    }
}

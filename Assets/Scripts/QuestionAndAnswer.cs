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
            NPCDIALOGUE.SetActive(true);
            if (PlayerPrefs.GetInt("Desert") != 1)
            {
                isTalking = true;
                StartCoroutine(DialogueNPCQNA(4.5f));
            }
            else if (PlayerPrefs.GetInt("Desert") == 1)
            {
                isTalking = true;
                StartCoroutine(EssenceDialogue(4.5f));
            }
        }
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
        Cursor.visible = true;
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

        PlayerController.Instance.pState.Transitioning = true;
        yield return new WaitForSeconds(time);
        STATUE2.SetActive(true);
        STATUE1.SetActive(false);
        yield return new WaitForSeconds(time);
        PlayerController.Instance.pState.Transitioning = false;
        dialogue.text = "You may now proceed on your adventure";
        yield return new WaitForSeconds(time);
        dialogue.text = "";
        PlayerController.Instance.pState.isNPC = false;
        Cursor.visible = false;
    }

    IEnumerator DialogueNPCQNA(float time)
    {
        Cursor.visible = true;
        PlayerController.Instance.pState.isNPC = true;

        

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

    IEnumerator CORRECTANSWER()
    {
        InputAnswer.SetActive(false);
        dialogue.text = "Yes... the curse has been broke but you can't leave here without the heroic essence.";
        yield return new WaitForSeconds(3f);
        dialogue.text = "You must defeat the datu batungan to obtain his essence.";
        yield return new WaitForSeconds(3f);
        dialogue.text = "";
        NPCDIALOGUE.SetActive(false);
        Cursor.visible = false;
        PlayerController.Instance.pState.isNPC = false;
        isTalking = false;
        Destroy(BORDER);
    }

    public void CloseUI()
    {
        StartCoroutine(CORRECTANSWER());
    }

    public void WrongAnswer()
    {
        dialogue.text = "Well... Looks like you're not prepared yet.";
        InputAnswer.SetActive(false);
        PlayerController.Instance.health = PlayerController.Instance.health - 99999;
    }
}

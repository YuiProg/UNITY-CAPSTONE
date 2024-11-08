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
                StartCoroutine(DialogueNPCQNA());
            }
            else if (PlayerPrefs.GetInt("Desert") == 1)
            {
                isTalking = true;
                StartCoroutine(EssenceDialogue());
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

    
    IEnumerator EssenceDialogue()
    {
        Cursor.visible = true;
        PlayerController.Instance.pState.isNPC = true;
        dialogue.text = "Well done. His sacrifice and his work on our country lives on";
        yield return new WaitForSeconds(3f);
        dialogue.text = "";
        PlayerController.Instance.pState.Transitioning = true;
        yield return new WaitForSeconds(4f);
        STATUE2.SetActive(true);
        STATUE1.SetActive(false);
        yield return new WaitForSeconds(4f);
        PlayerController.Instance.pState.Transitioning = false;
        dialogue.text = "You may not proceed on your adventure";
        yield return new WaitForSeconds(3f);
        PlayerController.Instance.pState.isNPC = false;
        Cursor.visible = false;
    }

    IEnumerator DialogueNPCQNA()
    {
        Cursor.visible = true;
        PlayerController.Instance.pState.isNPC = true;
        dialogue.text = "We've come so far, but there's one last puzzle.";
        yield return new WaitForSeconds(3f);
        dialogue.text = "They say these were his last words will be the key to broke this curse statue,";
        yield return new WaitForSeconds(3f);
        dialogue.text = "a declaration that his mission was complete.";
        yield return new WaitForSeconds(3f);
        dialogue.text = "It's a phrase in Latin. It will broke this curse nameless hero";
        yield return new WaitForSeconds(3f);
        QNA.SetActive(true);
        showQNAUI();
    }

    void showQNAUI()
    {
        StopCoroutine(DialogueNPCQNA());
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

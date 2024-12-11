using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TUTORIALMOVE : MonoBehaviour
{
    bool onTrigger;
    bool onGoing = false;
    [SerializeField] GameObject DLG;
    [SerializeField] Text dialogue;
    [SerializeField] GameObject UI;


    private void Update()
    {
        if (PlayerPrefs.GetInt("MOVEMENT TUTORIAL") == 1)
        {
            gameObject.SetActive(false);
        }
        if (onTrigger && !onGoing && PlayerPrefs.GetInt("MOVEMENT TUTORIAL") == 0)
        {
            onGoing = true;
            StartCoroutine(startTutorial());
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

    IEnumerator startTutorial()
    {
        PlayerController.Instance.pState.canPause = false;
        UI.SetActive(false);
        Time.timeScale = 1;
        DLG.SetActive(false);
        PlayerController.Instance.pState.isNPC = false;
        dialogue.text = "";
        yield return new WaitForSecondsRealtime(3f);
        PlayerController.Instance.pState.isNPC = true;
        Time.timeScale = 0;
        DLG.SetActive(true);
        dialogue.text = "A AND D FOR MOVEMENT KEYS, LEFT SHIFT TO SPRINT";
        yield return new WaitForSecondsRealtime(3f);
        Time.timeScale = 1;
        DLG.SetActive(false);
        PlayerController.Instance.pState.isNPC = false;
        dialogue.text = "";
        yield return new WaitForSecondsRealtime(3f);
        Time.timeScale = 0;
        PlayerController.Instance.pState.isNPC = true;
        DLG.SetActive(true);
        dialogue.text = "SPACE FOR JUMP AND DOUBLE JUMP";
        yield return new WaitForSecondsRealtime(3f);
        Time.timeScale = 1;
        DLG.SetActive(false);
        PlayerController.Instance.pState.isNPC = false;
        dialogue.text = "";
        yield return new WaitForSecondsRealtime(3f);
        Time.timeScale = 0;
        PlayerController.Instance.pState.isNPC = true;
        DLG.SetActive(true);
        dialogue.text = "E TO INTERACT AND BLOCK/PARRY";
        yield return new WaitForSecondsRealtime(3f);
        Time.timeScale = 1;
        DLG.SetActive(false);
        PlayerController.Instance.pState.isNPC = false;
        dialogue.text = "";
        yield return new WaitForSecondsRealtime(3f);
        Time.timeScale = 0;
        PlayerController.Instance.pState.isNPC = true;
        DLG.SetActive(true);
        dialogue.text = "LEFT CONTROL FOR DODGE";
        yield return new WaitForSecondsRealtime(3f);
        Time.timeScale = 1;
        DLG.SetActive(false);
        PlayerController.Instance.pState.isNPC = false;
        dialogue.text = "";
        PlayerPrefs.SetInt("MOVEMENT TUTORIAL", 1);
        gameObject.SetActive(false);
        UI.SetActive(true);
        PlayerController.Instance.pState.canPause = true;
    }
}

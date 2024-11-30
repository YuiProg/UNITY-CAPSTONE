using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IFUGAONPC1 : MonoBehaviour
{
    bool inTrigger;
    bool isSpeaking = false;
    [SerializeField] GameObject UI;
    [SerializeField] GameObject DIALOGUE;
    [SerializeField] Text dialogue;
    [SerializeField] Text npcName;



    private void Update()
    {
        if (inTrigger && !isSpeaking && Input.GetKeyDown(KeyCode.E))
        {

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
        PlayerController.Instance.pState.isNPC = true;
        DIALOGUE.SetActive(true);

        string[] words = new[]
        {
            "Stop right there! Who are you? Are you one of them?",

        };

        yield return null;

    }



}

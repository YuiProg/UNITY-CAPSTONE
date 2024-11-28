using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TutorialTrigger : MonoBehaviour
{
    bool ontrigger;
    bool onGoing = false;
    [SerializeField] GameObject DLG;
    [SerializeField] Text dialogue;

    private void Update()
    {
        if (ontrigger && !onGoing)
        {
            StartCoroutine(dialogues(3.5f));
        }
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            ontrigger = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            ontrigger = false;
        }
    }

    IEnumerator dialogues(float time)
    {
        onGoing = true;
        PlayerController.Instance.pState.isNPC = true;
        DLG.SetActive(true);
        string[] words = new[]
        {
            "Whose steps disturb the timeless divine.",
            "Because of its power, chaos shall arise, Threatening existence, to our demise.",
            "Only he, the chosen, can stand and fight, To preserve the threads of our day and night.",
            "Find the three artifacts, protect the gate, For the rogue traveler's power before it's too late"
        };

        for (int i = 0; i < words.Length; i++)
        {
            float elapsedtime = 0f;
            dialogue.text = words[i];
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
        onGoing = false;
        DLG.SetActive(false); 
        PlayerController.Instance.pState.isNPC = false;
        Destroy(gameObject);
    }
}

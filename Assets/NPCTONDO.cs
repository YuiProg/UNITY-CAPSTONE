using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NPCTONDO : MonoBehaviour
{
    bool intrigger;
    bool isTalking = false;
    [SerializeField] GameObject DIALOGUE;
    [SerializeField] GameObject BORDER;
    [SerializeField] Text dialogue;
    [SerializeField] Text npcName;
    [SerializeField] GameObject UI;
    [SerializeField] Text notify;


    // Update is called once per frame
    void Update()
    {
        if (PlayerPrefs.GetInt("TONDONPC") == 1) BORDER.SetActive(false);
            if (PlayerPrefs.GetInt("TONDONPC") == 1) notify.text = "";
        if (intrigger && !isTalking && Input.GetKeyDown(KeyCode.E) && PlayerPrefs.GetInt("TONDONPC") != 1)
        {
            StartCoroutine(Dialogue1(4.5f));
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

    IEnumerator Dialogue1(float time)
    {
        notify.text = "";
        UI.SetActive(false);
        PlayerController.Instance.pState.isNPC = true;
        PlayerController.Instance.pState.canPause = false;
        isTalking = true;
        DIALOGUE.SetActive(true);

        string[] words = new[]
        {
            "Where is Sultan?",
            "He's in the other side of the Pasig River, just go straight and you will see the bridge.",
            "Ok just hold on..."
        };

        string[] names = new[]
        {
            "Zieck",
            "Warrior",
            "Zieck"
        };

        for (int i = 0; i < words.Length; i++)
        {
            float elapsedtime = 0f;
            dialogue.text = words[i];
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
        DIALOGUE.SetActive(false);
        QuestTracker.instance.hasQuest = true;
        PlayerPrefs.SetString("Quest", "Find Sultan");
        PlayerPrefs.SetInt("TONDONPC", 1);
        PlayerController.Instance.pState.isNPC = false;
        PlayerController.Instance.pState.canPause = true;
        isTalking = false;
        UI.SetActive(true);
    }
}

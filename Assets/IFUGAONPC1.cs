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

    [SerializeField] Transform loc2;

    private void Update()
    {
        if (PlayerPrefs.GetInt("HK") == 1) transform.position = loc2.transform.position;
        if (PlayerPrefs.GetInt("HK") == 1) gameObject.SetActive(true);
        if (inTrigger && !isSpeaking && Input.GetKeyDown(KeyCode.E) && PlayerPrefs.GetInt("HK") == 1)
        {
            StartCoroutine(Dialogue(4.5f));
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
        isSpeaking = true;
        UI.SetActive(false);
        PlayerController.Instance.pState.isNPC = true;
        DIALOGUE.SetActive(true);

        string[] words = new[]
        {
            "You earn my trust, and I’ll take you to our hideout.",
            "You’re strong, but not strong enough to beat Magellan. It better to hide right now.",
            "Our hideout is just ahead",
            "Use those sheets to boost yourself ahead"
        };

        string[] names = new[]
        {
            "Tausūg Scout",
            "Tausūg Scout",
            "Tausūg Scout",
            "Tausūg Scout"
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
                elapsedtime += Time.deltaTime;
                yield return null;
            }
        }
        PlayerPrefs.SetString("Quest", "Find the hideout");
        isSpeaking = false;
        DIALOGUE.SetActive(false);
        PlayerController.Instance.pState.isNPC = false;
        UI.SetActive(true);
    }



}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MAGELLANNPC : MonoBehaviour
{
    [SerializeField] GameObject DIALOGUE;
    [SerializeField] Text dialogue;
    [SerializeField] Text npcName;


    bool inTrigger;
    bool isTalking = false;

    // Update is called once per frame
    void Update()
    {
        if (PlayerPrefs.GetInt("MAGELLAN") == 1)
        {
            gameObject.SetActive(true);
        }
        else
        {
            gameObject.SetActive(false);
        }
        flip();
        
        if (inTrigger && Input.GetKeyDown(KeyCode.E) && !isTalking)
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
        PlayerController.Instance.pState.isNPC = true;
        DIALOGUE.SetActive(true);
        isTalking = true;

        string[] words = new[]
        {
            "Thank you, young warrior. May the strength of Lapu-Lapu bless you",
            "Today, we have reclaimed what is rightfully ours.",
            "You have led us to victory with strength and wisdom. But it is not merely this battle that has brought us here—it is destiny."
        };
        string[] names = new[]
        {
            "Tausug",
            "Tausug",
            "Tausug"
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
        PlayerController.Instance.pState.SkillBOSS = true;
        PlayerController.Instance.pState.isNPC = false;
        QuestTracker.instance.hasQuest = false;
        PlayerPrefs.DeleteKey("Quest");
        PlayerPrefs.SetInt("MAGELLANNPC", 1);
        PlayerPrefs.SetInt("Tondo", 1);
        DIALOGUE.SetActive(false);
    }
    void flip()
    {
        if (PlayerController.Instance.transform.position.x > transform.position.x)
        {
            transform.eulerAngles = new Vector3(0, 180, 0);
        }
        else
        {
            transform.eulerAngles = new Vector3(0, 0, 0);
        }
    }

}

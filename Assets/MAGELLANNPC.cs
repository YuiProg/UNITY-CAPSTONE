using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MAGELLANNPC : MonoBehaviour
{
    [SerializeField] GameObject DIALOGUE;
    [SerializeField] Text dialogue;
    [SerializeField] Text npcName;
    [SerializeField] GameObject UI;
    //alert
    [SerializeField] Text notif;

    //world map
    [SerializeField] GameObject worldMap;

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
        UI.SetActive(false);
        PlayerController.Instance.pState.isNPC = true;
        PlayerController.Instance.pState.canPause = false;
        notif.text = "";
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
            "Tausug Elder Tribe",
            "Tausug Elder Tribe",
            "Tausug Elder Tribe"
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
        PlayerController.Instance.pState.SkillBOSS = true;
        QuestTracker.instance.hasQuest = false;
        PlayerPrefs.DeleteKey("Quest");
        PlayerPrefs.SetInt("MAGELLANNPC", 1);
        PlayerPrefs.SetInt("SLASH", 1);
        PlayerPrefs.SetInt("Tondo", 1);
        DIALOGUE.SetActive(false);
        yield return new WaitForSeconds(time - 2);
        PlayerController.Instance.pState.Transitioning = true;
        yield return new WaitForSeconds(time - 2);
        PlayerController.Instance.pState.Transitioning = false;
        Cursor.visible = true;
        worldMap.SetActive(true);
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

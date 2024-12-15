using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FINALNPC : MonoBehaviour
{
    [SerializeField] GameObject DIALOGUE;
    [SerializeField] Text dlg;
    [SerializeField] Text npcName;
    [SerializeField] GameObject UI;



    bool inTrigger;
    bool isSpeaking = false;

    // Update is called once per frame
    void Update()
    {
        if (inTrigger && !isSpeaking)
        {
            StartCoroutine(Dialogue(4.5f));
        }
    }


    IEnumerator Dialogue(float time)
    {
        QuestTracker.instance.hasQuest = false;
        PlayerPrefs.DeleteKey("Quest");
        PlayerController.Instance.pState.isNPC = true;
        PlayerController.Instance.pState.canOpenJournal = false;
        PlayerController.Instance.pState.canPause = false;
        UI.SetActive(false);
        DIALOGUE.SetActive(true);
        isSpeaking = true;
        string[] words = new[]
        {
            "The Manunggul Jar. We found it, it’s real.",
            "All that I have been through… it was worth it. Let’s pack it up and leave.",
            "Lift the jar carefully we are going to show it to the world!",
        };

        string[] names = new[]
        {
            "Balweg",
            "Zieck",
            "Christina"
        };


        for (int i = 0; i < words.Length; i++)
        {
            dlg.text = words[i];
            npcName.text = names[i];

            float elapsedtime = 0f;

            while (elapsedtime < time)
            {
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    elapsedtime = time;
                    break;
                }
                else if(Input.GetKeyDown(KeyCode.F))
                {
                    elapsedtime = time;
                }
                elapsedtime += Time.deltaTime;
                yield return null;
            }
        }
        dlg.text = "";
        npcName.text = "";
        DIALOGUE.SetActive(false);
        PlayerController.Instance.pState.Transitioning = true;
        yield return new WaitForSeconds(time - 2);
        PlayerController.Instance.pState.Transitioning = false;
        PlayerController.Instance.transform.position = new Vector2(0, 0);
        Save.instance.saveData();
        PlayerPrefs.SetInt("inIfugao", 1);
        PlayerPrefs.SetInt("inMactan", 0);
        PlayerPrefs.SetInt("inTondo", 0);
        PlayerPrefs.SetInt("inSQ", 0);
        PlayerPrefs.SetInt("inSpace", 0);
        PlayerPrefs.SetInt("inCave", 0);
        Cursor.visible = true;
        LevelManager.instance.loadscene("ENDING CREDITS");

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
}

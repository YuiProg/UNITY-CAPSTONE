using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CUTSCENETRIGGER : MonoBehaviour
{

    bool ontrigger = false;
    bool triggered = false;
    [SerializeField] GameObject NPCTEXT;
    [SerializeField] Text dlg;
    [SerializeField] GameObject jar1;
    [SerializeField] GameObject jar2;
    [SerializeField] GameObject jar3;
    [SerializeField] GameObject jar4;
    [SerializeField] GameObject jar5;
    [SerializeField] Transform player;
    [SerializeField] GameObject border;
    [SerializeField] GameObject GameUI;
    // Update is called once per frame
    void Update()
    {
        if (ontrigger && !triggered)
        {
            StartCoroutine(Dialogue(4.5f));
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
    IEnumerator Dialogue(float time)
    {
        GameUI.SetActive(false);
        triggered = true;
        NPCTEXT.SetActive(true);
        Cursor.visible = true;
        PlayerController.Instance.pState.isNPC = true;
        PlayerPrefs.DeleteKey("Quest");
        QuestTracker.instance.hasQuest = false;
        string[] dialogues1 = new[]
        {
            "This isn’t funny. Where did you guys go?",
            "Hello guys?, Are you in there?",
            "Maybe I should go inside.",
            "",
        };

        for (int i = 0; i < dialogues1.Length; i++)
        {
            dlg.text = dialogues1[i];
            float elapsedtime = 0f;
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

        PlayerController.Instance.pState.Transitioning = true;
        yield return new WaitForSeconds(2f);
        Destroy(jar1);
        Destroy(jar2);
        Destroy(jar3);
        Destroy(jar4);
        Destroy(jar5);
        yield return new WaitForSeconds(time + 1f); 
        PlayerController.Instance.pState.Transitioning = false;

        string[] dialogue2 = new[]
        {
            "Alright, stay calm, Zieck. You’ve been in strange situations before. Just… think. Where could they have gone? and the artifacts also are not here",
            "But i saw before i enter that this place have a indication of the jar.",
            "What the hell is happening?!",
            "...",
            "Maybe I should check them outside the cave.",
        };

        for (int i = 0; i < dialogue2.Length; i++)
        {
            dlg.text = dialogue2[i];
            float elapsedtime = 0f;
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
        dlg.text = "";

        PlayerController.Instance.pState.Transitioning = true;
        yield return new WaitForSeconds(2f);
        PlayerController.Instance.transform.position = player.transform.position;
        yield return new WaitForSeconds(time + 1f);
        PlayerController.Instance.pState.Transitioning = false;
        string[] dialogue3 = new[]
        {
            "Where did they go?",
            "...",
            "maybe i check the forest.",
        };

        for (int i = 0; i < dialogue3.Length; i++)
        {
            dlg.text = dialogue3[i];
            float elapsedtime = 0f;
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

        TeleportToCS.instance.canEnter = true;
        yield return new WaitForSeconds(time);     
        Cursor.visible = false;
        PlayerPrefs.SetString("Quest", "Go to the forest");
        PlayerController.Instance.pState.isNPC = false;
        NPCTEXT.SetActive(false);
        GameUI.SetActive(true);
    }
}

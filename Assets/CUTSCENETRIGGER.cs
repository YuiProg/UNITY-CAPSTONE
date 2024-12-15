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
    [SerializeField] GameObject GameUI;
    // Update is called once per frame

    AudioManager audiomanager;

    private void Start()
    {
        audiomanager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
    }
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
        };

        for (int i = 0; i < dialogues1.Length; i++)
        {
            dlg.text = dialogues1[i];
            float elapsedtime1 = 0f;
            while (elapsedtime1 < time)
            {
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    elapsedtime1 = time;
                    break;
                }
                else if (Input.GetKeyDown(KeyCode.F))
                {
                    elapsedtime1 = time;
                }
                elapsedtime1 += Time.deltaTime;
                yield return null;
            }
        }
        NPCTEXT.SetActive(false);
        PlayerController.Instance.pState.Transitioning = true;
        yield return new WaitForSeconds(2f);
        jar1.SetActive(false);
        jar2.SetActive(false);
        jar3.SetActive(false);
        jar4.SetActive(false);
        jar5.SetActive(false);
        yield return new WaitForSeconds(time + 1f); 
        PlayerController.Instance.pState.Transitioning = false;
        NPCTEXT.SetActive(true);
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
            float elapsedtime2 = 0f;
            while (elapsedtime2 < time)
            {
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    elapsedtime2 = time;
                    break;
                }
                else if (Input.GetKeyDown(KeyCode.F))
                {
                    elapsedtime2 = time;
                }
                elapsedtime2 += Time.deltaTime;
                yield return null;
            }
        }
        dlg.text = "";
        NPCTEXT.SetActive(false);
        PlayerController.Instance.pState.Transitioning = true;
        yield return new WaitForSeconds(2f);
        PlayerController.Instance.transform.position = player.transform.position;
        audiomanager.StopBGFX();
        audiomanager.PlayBGSFX(audiomanager.ForestArea);
        PlayerPrefs.SetInt("inIfugao", 1);
        PlayerPrefs.SetInt("inMactan", 0);
        PlayerPrefs.SetInt("inTondo", 0);
        PlayerPrefs.SetInt("inSQ", 0);
        PlayerPrefs.SetInt("inSpace", 0);
        PlayerPrefs.SetInt("inCave", 0);
        yield return new WaitForSeconds(time + 1f);
        Save.instance.saveData();
        PlayerController.Instance.pState.Transitioning = false;
        NPCTEXT.SetActive(true);
        string[] dialogue3 = new[]
        {
            "Where did they go?",
            "...",
            "maybe i check the forest.",
        };

        for (int i = 0; i < dialogue3.Length; i++)
        {
            dlg.text = dialogue3[i];
            float elapsedtime3 = 0f;
            while (elapsedtime3 < time)
            {
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    elapsedtime3 = time;
                    break;
                }
                else if (Input.GetKeyDown(KeyCode.F))
                {
                    elapsedtime3 = time;
                }
                elapsedtime3 += Time.deltaTime;
                yield return null;
            }
        }

        TeleportToCS.instance.canEnter = true;   
        Cursor.visible = false;
        PlayerPrefs.SetString("Quest", "Go to the forest");
        Save.instance.saveData();
        PlayerController.Instance.pState.isNPC = false;
        NPCTEXT.SetActive(false);
        GameUI.SetActive(true);
    }
}

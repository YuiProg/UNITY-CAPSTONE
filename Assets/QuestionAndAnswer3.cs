using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuestionAndAnswer3 : MonoBehaviour
{
    [SerializeField] GameObject QNA;
    [SerializeField] GameObject InputAnswer;
    [SerializeField] GameObject NPCDIALOGUE;
    [SerializeField] GameObject STATUE1;
    [SerializeField] GameObject STATUE2;
    [SerializeField] Text dialogue;
    [SerializeField] GameObject BORDER;

    //ui
    [SerializeField] GameObject UI;

    //world map
    [SerializeField] GameObject worldMap;

    public bool inTrigger = false;
    bool isTalking = false;
    bool worldlmapactive = false;
    private void Start()
    {
        QNA.SetActive(false);
        InputAnswer.SetActive(false);
    }

    private void Update()
    {
        BORDER.SetActive(PlayerPrefs.GetInt("SideQuest3") != 1);
        if (inTrigger)
        {
            if (!isTalking && Input.GetKeyDown(KeyCode.E) && !worldlmapactive)
            {
                if (PlayerPrefs.GetInt("SpearGirl") == 1)
                {
                    StartCoroutine(winDLG(4.5f));
                    return;
                }
                if (PlayerPrefs.GetInt("SideQuest3") != 1)
                {
                    StartCoroutine(dialogue1(4.5f));
                }
            }
            
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

    IEnumerator dialogue1(float time)
    {
        UI.SetActive(false);
        PlayerController.Instance.pState.isNPC = true;
        PlayerController.Instance.pState.canPause = false;
        PlayerController.Instance.pState.canOpenJournal = false;
        Cursor.visible = true;
        isTalking = true;
        NPCDIALOGUE.SetActive(true);
        string[] words = new[]
        {
            "To proceed, you must prove yourself worthy. Answer this: On what date did the Philippines declare its independence from Spain?"
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
                else if (Input.GetKeyDown(KeyCode.F))
                {
                    elapsedtime = time;
                }
                elapsedtime += Time.deltaTime;
                yield return null;
            }
        }
        QNA.SetActive(true);
        Question();
    }

    public void Question()
    {
        InputAnswer.SetActive(true);
        StopCoroutine(dialogue1(4.5f));
        dialogue.text = "To proceed, you must prove yourself worthy. Answer this: On what date did the Philippines declare its independence from Spain?";
    }

    public void correctAnswer()
    {
        InputAnswer.SetActive(false);
        StartCoroutine(dialogue2(4.5f));
    }

    IEnumerator dialogue2(float time)
    {
        string[] words = new[]
        {
            "You have answered correctly.",
            "The curse is lifted, and the truth of history grants you passage but that is not enough to bring him back, you must bring me his essence."
        };

        for (int i = 0;i < words.Length;i++)
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

        dialogue.text = "";
        PlayerPrefs.SetInt("SideQuest3", 1);
        QuestTracker.instance.hasQuest = true;
        PlayerPrefs.SetString("Quest", "Defeat the area boss");
        NPCDIALOGUE.SetActive(false);
        BORDER.SetActive(false);
        PlayerController.Instance.pState.isNPC = false;
        PlayerController.Instance.pState.canPause = true;
        PlayerController.Instance.pState.canOpenJournal = true;
        isTalking = false;
        UI.SetActive(true);
    }

    IEnumerator winDLG(float time)
    {
        UI.SetActive(false);
        PlayerController.Instance.pState.canOpenJournal = false;
        QuestTracker.instance.hasQuest = false;
        PlayerController.Instance.pState.canPause = false;
        PlayerPrefs.DeleteKey("Quest");
        Cursor.visible = true;
        isTalking = true;
        NPCDIALOGUE.SetActive(true);
        PlayerController.Instance.pState.isNPC = true;
        string[] dialogues = new[]
        {
            "Well done. His sacrifice and his work on our country lives on",
            "..."
        };

        for (int i = 0; i < dialogues.Length; i++)
        {

            dialogue.text = dialogues[i];
            float waitforseconds = 0f;
            while (waitforseconds < time)
            {
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    waitforseconds = time;
                    break;
                }
                else if (Input.GetKeyDown(KeyCode.F))
                {
                    waitforseconds = time;
                }
                waitforseconds += Time.deltaTime;
                yield return null;
            }

        }
        NPCDIALOGUE.SetActive(false);
        PlayerController.Instance.pState.Transitioning = true;
        yield return new WaitForSeconds(time);
        STATUE2.SetActive(true);
        STATUE1.SetActive(false);
        yield return new WaitForSeconds(time - 1);
        PlayerController.Instance.pState.Transitioning = false;
        PlayerPrefs.DeleteKey("Quest");
        NPCDIALOGUE.SetActive(true);
        dialogue.text = "You may now proceed on your adventure";
        yield return new WaitForSeconds(time);
        NPCDIALOGUE.SetActive(false);
        dialogue.text = "";
        PlayerPrefs.SetInt("SIDEQUEST3COMP", 1);
        yield return new WaitForSeconds(time - 2);
        PlayerController.Instance.pState.Transitioning = true;
        yield return new WaitForSeconds(time - 2);
        PlayerController.Instance.pState.Transitioning = false;
        Cursor.visible = true;
        worldlmapactive = true;
        worldMap.SetActive(true);
        
        isTalking = false;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SideQuestLever : MonoBehaviour
{
    bool ontrigger;
    bool activated = false;
    [SerializeField] GameObject BORDER;
    [SerializeField] GameObject DIALOGUE;
    [SerializeField] Text dlg;

    Animator anim;

    private void Start()
    {
        BORDER.SetActive(true);
        anim = GetComponent<Animator>();
        if (PlayerPrefs.GetInt("Lever") == 1)
        {
            anim.Play("lever");
            Destroy(BORDER);
        }
    }

    private void Update()
    {
        if (ontrigger)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                if (!activated)
                {
                    anim.Play("lever");
                    StartCoroutine(dialogue(2f));
                }          
            }
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

    IEnumerator dialogue(float time)
    {
        activated = true;
        PlayerController.Instance.pState.isNPC = true;
        DIALOGUE.SetActive(true);
        dlg.text = "Something was opened";
        yield return new WaitForSeconds(time);
        PlayerController.Instance.pState.isNPC = false;
        DIALOGUE.SetActive(false);
        dlg.text = "";
        PlayerPrefs.SetInt("Lever", 1);
        QuestTracker.instance.hasQuest = true;
        PlayerPrefs.SetString("Quest", "Defeat the area boss");
        Save.instance.saveData();
        Destroy(BORDER);
    }

}

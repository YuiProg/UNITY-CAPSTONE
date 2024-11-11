using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NPC1 : MonoBehaviour
{
    bool ontrigger = false;
    [SerializeField] Text dlg;
    [SerializeField] GameObject npcDLG;

    //npcs
    [SerializeField] GameObject npc1;
    [SerializeField] GameObject npc2;
    //changeloc
    [SerializeField] Transform loc1;
    [SerializeField] Transform loc2;
    //state
    int count = 0;
    // Update is called once per frame
    private void Start()
    {
        npc1.SetActive(true);
        npc2.SetActive(true);
    }
    void Update()
    {
        if (ontrigger && Input.GetKeyDown(KeyCode.E))
        {
            if (count != 1)
            {
                StartCoroutine(Dialogue1(4.5f));
            }
            else
            {
                StartCoroutine(Dialogue2(4.5f));
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

    IEnumerator Dialogue1(float time)
    {
        npcDLG.SetActive(true);
        PlayerController.Instance.pState.isNPC = true;
        Cursor.visible = true;
        dlg.text = "Suki - I can't believe it! We’re finally here. Months of research.";
        yield return new WaitForSeconds(time);
        dlg.text = "Suki - Thanks from that village for helping us here.";
        yield return new WaitForSeconds(time);
        dlg.text = "Jobert - The artifact should be here straight of that cliff. It won’t be easy, though; we’ll need to navigate through this rocky trail and climb up.";
        yield return new WaitForSeconds(time);
        dlg.text = "Alright. Just keep your focus up there, Suki and Jobert. I’ll be right behind you all… carefully.";
        yield return new WaitForSeconds(time);
        dlg.text = "";
        PlayerController.Instance.pState.Transitioning = true;
        yield return new WaitForSeconds(2.5f);
        npc1.transform.position = loc1.transform.position;
        npc2.transform.position = loc2.transform.position;
        yield return new WaitForSeconds(time + 1);
        PlayerController.Instance.pState.Transitioning = false;
        PlayerController.Instance.pState.isNPC = false;
        Cursor.visible = false;
        count++;
        npcDLG.SetActive(false);
    }

    IEnumerator Dialogue2(float time)
    {
        npcDLG.SetActive(true);
        Cursor.visible = true;
        PlayerController.Instance.pState.isNPC = true;
        dlg.text = "Jobert - Here we are. This is it—the entrance to the cave. Somewhere inside is the artifact jar we came all this way for. Everyone ready?";
        yield return new WaitForSeconds(time);
        dlg.text = "Suki - I’ve been ready since we set foot in the forest! Just think, this jar has been hidden here for centuries, and can't wait to show it to the world.";
        yield return new WaitForSeconds(time);
        dlg.text = "Hold that excitement, Jobert. This cave is ancient, and the footing might be tricky.";
        yield return new WaitForSeconds(time);
        PlayerController.Instance.pState.isNPC = false;
        Cursor.visible = false;
        npcDLG.SetActive(false);
    }
}

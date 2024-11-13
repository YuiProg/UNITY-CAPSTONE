using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NPC1 : MonoBehaviour
{
    bool ontrigger = false;
    [SerializeField] Text dlg;
    [SerializeField] Text npcName;
    [SerializeField] GameObject npcDLG;

    //npcs
    [SerializeField] GameObject npc1;
    [SerializeField] GameObject npc2;
    //changeloc
    [SerializeField] Transform loc1;
    [SerializeField] Transform loc2;
    [SerializeField] Transform loc3;
    [SerializeField] Transform loc4;
    [SerializeField] Transform loc5;
    [SerializeField] Transform loc6;
    [SerializeField] Transform loc7;
    [SerializeField] Transform loc8;
    //NPCBORDER
    [SerializeField] GameObject BORDER1;
    [SerializeField] GameObject BORDER2;
    //state
    public int count = 0;
    public int count2 = 0;
    bool talking = true;
    
    private void Start()
    {
        BORDER1.SetActive(true);
        BORDER2.SetActive(true);
        if (PlayerPrefs.GetInt("NPCINCAVE") == 1)
        {
            npc1.transform.position = loc3.transform.position;
            npc2.transform.position = loc4.transform.position;
        }
    }
    void Update()
    {
        if (ontrigger && Input.GetKeyDown(KeyCode.E))
        {
            if (PlayerPrefs.GetInt("NPCINCAVE") != 1)
            {
                switch (count)
                {
                    case 0:
                        if (talking)
                        {
                            StartCoroutine(Dialogue1(4.5f));
                        }
                        break;
                    case 1:
                        if (talking)
                        {
                            StartCoroutine(Dialogue2(4.5f));
                        }
                        break;
                    default:
                        break;
                }
            }
            else
            {
                switch (count2)
                {
                    case 0:
                        if (talking)
                        {
                            StartCoroutine(Dialogue3(4.5f));
                        }
                        break;
                    //case 1:
                    //    if (talking)
                    //    {
                    //        StartCoroutine(Dialogue4(4.5f));
                    //    }
                        //break;
                    default:
                        break;
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

    IEnumerator Dialogue1(float time)
    {
        npcDLG.SetActive(true);
        talking = false;
        PlayerController.Instance.pState.isNPC = true;
        Cursor.visible = true;
        dlg.text = "I can't believe it! We’re finally here. Months of research.";
        npcName.text = "Christina";
        yield return new WaitForSeconds(time);
        dlg.text = "Thanks from that village for helping us here.";
        npcName.text = "Christina";
        yield return new WaitForSeconds(time);
        dlg.text = "The artifact should be here straight of that cliff. It won’t be easy, though; we’ll need to navigate through this rocky trail and climb up.";
        npcName.text = "Balweg";
        yield return new WaitForSeconds(time);
        dlg.text = "Alright. Just keep your focus up there, Christina and Balweg. I’ll be right behind you all… carefully.";
        npcName.text = "";
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
        Destroy(BORDER1);
        talking = true;
        count++;
        npcDLG.SetActive(false);
    }

    IEnumerator Dialogue2(float time)
    {
        npcDLG.SetActive(true);
        Cursor.visible = true;
        talking = false;
        PlayerController.Instance.pState.isNPC = true;
        dlg.text = "Here we are. This is it—the entrance to the cave. Somewhere inside is the artifact jar we came all this way for. Everyone ready?";
        npcName.text = "Balweg";
        yield return new WaitForSeconds(time);
        dlg.text = "I’ve been ready since we set foot in the forest! Just think, this jar has been hidden here for centuries, and can't wait to show it to the world.";
        npcName.text = "Christina";
        yield return new WaitForSeconds(time);
        dlg.text = "Hold that excitement, Balweg. This cave is ancient, and the footing might be tricky.";
        yield return new WaitForSeconds(time);
        dlg.text = "";
        npcName.text = "";
        PlayerController.Instance.pState.Transitioning = true;
        yield return new WaitForSeconds(2.5f);
        npc1.transform.position = loc3.transform.position;
        npc2.transform.position = loc4.transform.position;
        yield return new WaitForSeconds(time + 1);
        PlayerController.Instance.pState.Transitioning = false;
        PlayerController.Instance.pState.isNPC = false;
        Cursor.visible = false;
        talking = true;
        PlayerPrefs.SetInt("NPCINCAVE", 1);
        npcDLG.SetActive(false);
    }

    IEnumerator Dialogue3(float time)
    {
        npcDLG.SetActive(true);
        Cursor.visible = true;
        talking = false;
        PlayerController.Instance.pState.isNPC = true;
        dlg.text = "This cave are notorious for being unpredictable. We should watch for loose rocks or slippery patches. No heroics in here.";
        npcName.text = "Balweg";
        yield return new WaitForSeconds(time);
        dlg.text = "Good point, Balweg. And remember, everyone, they say there should be hidden chambers. Keep a close eye on your surroundings.";
        yield return new WaitForSeconds(time);
        dlg.text = "";
        npcName.text = "";
        PlayerController.Instance.pState.Transitioning = true;
        yield return new WaitForSeconds(2.5f);
        npc1.transform.position = loc7.transform.position;
        npc2.transform.position = loc8.transform.position;
        yield return new WaitForSeconds(time + 1);
        PlayerController.Instance.pState.Transitioning = false;
        PlayerController.Instance.pState.isNPC = false;
        Cursor.visible = false;
        talking = true;
        count2++;
        Destroy(BORDER2);
        npcDLG.SetActive(false);
    }

    //IEnumerator Dialogue4(float time)
    //{
    //    npcDLG.SetActive(true);
    //    Cursor.visible = true;
    //    talking = false;
    //    PlayerController.Instance.pState.isNPC = true;
    //    dlg.text = "i said don't rush it. maybe they are in there now.";
    //    yield return new WaitForSeconds(time);
    //    dlg.text = "Abi - Come on we're so close now!";
    //    yield return new WaitForSeconds(time);
    //    dlg.text = "Balweg - Try to catch up with us if you can.";
    //    yield return new WaitForSeconds(time);
    //    dlg.text = "";
    //    count2++;
    //    PlayerController.Instance.pState.Transitioning = true;
    //    yield return new WaitForSeconds(2.5f);
    //    npc1.transform.position = loc7.transform.position;
    //    npc2.transform.position = loc8.transform.position;
    //    yield return new WaitForSeconds(time + 1);
    //    PlayerController.Instance.pState.Transitioning = false;
    //    PlayerController.Instance.pState.isNPC = false;
    //    Cursor.visible = false;
    //    yield return new WaitForSeconds(time + 2f);
    //    dlg.text = "This isn’t funny. Where did you guys go? no response i should go inside.";
    //    yield return new WaitForSeconds(time);
    //    dlg.text = "";
    //    talking = true;
    //    npcDLG.SetActive(false);
    //}
    //IEnumerator ChamberDialogue(float time)
    //{
    //    npcDLG.SetActive(true);
    //    Cursor.visible = true;
    //    talking = false;
    //    PlayerController.Instance.pState.isNPC = true;
    //    dlg.text = "Alright, stay calm, Zieck. You’ve been in strange situations before. Just… think. Where could they have gone? and the artifacts also are not here.";
    //    yield return new WaitForSeconds(time);
    //    dlg.text = "But i saw before i enter that this place have a indication of the jar.";
    //    yield return new WaitForSeconds(time);
    //    dlg.text = "What the hell is happenning?!";
    //    yield return new WaitForSeconds(time);
    //    dlg.text = "Maybe i should check them outside the cave.";
    //    yield return new WaitForSeconds(time);
    //    Cursor.visible = false;
    //    dlg.text = "";
    //    talking = true;
    //    PlayerController.Instance.pState.isNPC = false;
    //    npcDLG.SetActive(false);
    //}
}

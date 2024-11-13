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
        triggered = true;
        NPCTEXT.SetActive(true);
        Cursor.visible = true;
        PlayerController.Instance.pState.isNPC = true;
        dlg.text = "This isn’t funny. Where did you guys go?";
        yield return new WaitForSeconds(time);
        dlg.text = "Hello guys?, Are you in there?";
        yield return new WaitForSeconds(time);
        dlg.text = "Maybe I should go inside.";
        yield return new WaitForSeconds(time);
        dlg.text = "";
        PlayerController.Instance.pState.Transitioning = true;
        yield return new WaitForSeconds(2f);
        Destroy(jar1);
        Destroy(jar2);
        Destroy(jar3);
        Destroy(jar4);
        Destroy(jar5);
        yield return new WaitForSeconds(time + 1f); 
        PlayerController.Instance.pState.Transitioning = false;
        dlg.text = "Alright, stay calm, Zieck. You’ve been in strange situations before. Just… think. Where could they have gone? and the artifacts also are not here";
        yield return new WaitForSeconds(time);
        dlg.text = "But i saw before i enter that this place have a indication of the jar.";
        yield return new WaitForSeconds(time);
        dlg.text = "What the hell is happening?!";
        yield return new WaitForSeconds(time);
        dlg.text = "...";
        yield return new WaitForSeconds(time);
        dlg.text = "Maybe I should check them outside the cave.";
        yield return new WaitForSeconds(time);
        dlg.text = "";
        PlayerController.Instance.pState.Transitioning = true;
        yield return new WaitForSeconds(2f);
        PlayerController.Instance.transform.position = player.transform.position;
        yield return new WaitForSeconds(time + 1f);
        PlayerController.Instance.pState.Transitioning = false;
        dlg.text = "Where did they go?";
        yield return new WaitForSeconds(time);
        dlg.text = "...";
        yield return new WaitForSeconds(time);
        dlg.text = "maybe i check the forest.";
        TeleporterOPTIONAL.instance.canEnter = true;
        yield return new WaitForSeconds(time);     
        Cursor.visible = false;
        PlayerController.Instance.pState.isNPC = false;
        NPCTEXT.SetActive(false);
    }
}

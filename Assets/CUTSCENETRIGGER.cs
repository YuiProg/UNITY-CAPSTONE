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
        dlg.text = "Alright, stay calm, Zieck. You’ve been in strange situations before. Just… think. Where could they have gone? and the artifacts also are not here.";
        yield return new WaitForSeconds(time);
        dlg.text = "But i saw before i enter that this place have a indication of the jar.";
        yield return new WaitForSeconds(time);
        dlg.text = "What the hell is happenning?!";
        yield return new WaitForSeconds(time);
        dlg.text = "Maybe i should check them outside the cave.";
        yield return new WaitForSeconds(time);
        Cursor.visible = false;
        dlg.text = "";
        PlayerController.Instance.pState.isNPC = false;
        NPCTEXT.SetActive(false);
    }
}

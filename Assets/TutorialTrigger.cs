using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TutorialTrigger : MonoBehaviour
{
    bool ontrigger;
    bool onGoing = false;
    [SerializeField] GameObject DLG;
    [SerializeField] Text dialogue;

    private void Update()
    {
        if (ontrigger && !onGoing)
        {
            StartCoroutine(dialogues(3.5f));
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

    IEnumerator dialogues(float time)
    {
        onGoing = true;
        DLG.SetActive(true);
        dialogue.text = "Left Click for Normal Attack";
        yield return new WaitForSeconds(time);
        dialogue.text = "Right Click for Heavy Attack";
        yield return new WaitForSeconds(time);
        dialogue.text = "E to Block";
        yield return new WaitForSeconds(time);
        dialogue.text = "Left Ctrl to Dodge";
        yield return new WaitForSeconds(time);
        dialogue.text = "Left Alt to Heal";
        yield return new WaitForSeconds(time);
        DLG.SetActive(false);
        Destroy(gameObject);
    }
}

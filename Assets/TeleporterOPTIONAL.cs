using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleporterOPTIONAL : MonoBehaviour
{
    bool onTrigger = false;
    public Transform tpHere;

    private void Update()
    {
        if (onTrigger && Input.GetKeyDown(KeyCode.E))
        {
            StartCoroutine(Transition(5f));
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            onTrigger = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            onTrigger = false;
        }
    }

    IEnumerator Transition(float time)
    {
        PlayerController.Instance.pState.Transitioning = true;
        yield return new WaitForSeconds(2f);
        PlayerController.Instance.transform.position = tpHere.transform.position;
        yield return new WaitForSeconds(time);
        PlayerController.Instance.pState.Transitioning = false;
    }
}

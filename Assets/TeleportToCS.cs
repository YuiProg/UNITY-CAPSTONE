using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TeleportToCS : MonoBehaviour
{
    bool onTrigger = false;
    public Transform tpHere;
    public bool canEnter = false;
    public Text dlg;
    public GameObject dialogue;
    bool triggered = false;
    public static TeleportToCS instance;
    private void Update()
    {
        if (onTrigger && Input.GetKeyDown(KeyCode.E) && canEnter)
        {
            StartCoroutine(Transition(5f));
        }
        else if (onTrigger && Input.GetKeyDown(KeyCode.E) && !canEnter && !triggered)
        {
            StartCoroutine(dlgtext(2f));
        }
    }
    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
        }
    }

    IEnumerator dlgtext(float time)
    {
        triggered = true;
        PlayerController.Instance.pState.isNPC = true;
        dialogue.SetActive(true);
        dlg.text = "Can't enter";
        yield return new WaitForSeconds(time);
        PlayerController.Instance.pState.isNPC = false;
        dlg.text = "";
        triggered = false;
        dialogue.SetActive(false);
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
        Save.instance.saveData();
        LevelManager.instance.loadscene("CUTSCENE2");
    }
}

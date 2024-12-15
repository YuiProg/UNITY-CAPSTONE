using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ZieckNPC : MonoBehaviour
{

    bool inTrigger;
    bool isSpeaking = false;
    [SerializeField] Transform TPHERE;

    void Update()
    {
        if (!isSpeaking && inTrigger && Input.GetKeyDown(KeyCode.E))
        {
            StartCoroutine(transition(4.5f));
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
    IEnumerator transition(float time)
    {
        PlayerController.Instance.pState.Transitioning = true;
        yield return new WaitForSeconds(time - 2);
        PlayerController.Instance.transform.position = TPHERE.position;
        Save.instance.saveData();
        PlayerController.Instance.pState.Transitioning = false;
        PlayerPrefs.SetInt("inIfugao", 0);
        PlayerPrefs.SetInt("inMactan", 0);
        PlayerPrefs.SetInt("inTondo", 0);
        PlayerPrefs.SetInt("inSQ", 0);
        PlayerPrefs.SetInt("inSpace", 0);
        PlayerPrefs.SetInt("inCave", 1);
        QuestTracker.instance.hasQuest = true;
        PlayerPrefs.SetString("Quest", "Find the Jar");
        LevelManager.instance.loadscene("CUTSCENE4");
    }
    
}

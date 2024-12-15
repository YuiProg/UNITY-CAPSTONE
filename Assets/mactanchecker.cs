using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class mactanchecker : MonoBehaviour
{
    bool inTrigger;
    private void Update()
    {
        if (inTrigger)
        {
            QuestTracker.instance.hasQuest = true;
            PlayerPrefs.SetString("Quest", "Encourage the village");
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
}

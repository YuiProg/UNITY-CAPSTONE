using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class MACTANNPC : MonoBehaviour
{
    bool istalking = false;
    bool inTrigger;
    [SerializeField] GameObject NPCDIALOGUE;
    [SerializeField] Text dialogue;
    [SerializeField] Text npcName;
    [SerializeField] GameObject UI;
    //notif
    [SerializeField] Text notiftext;

    //npcs
    [SerializeField] GameObject npc1;
    [SerializeField] GameObject npc2;
    [SerializeField] GameObject npc3;
    [SerializeField] GameObject npc4;


    //boss loc
    [SerializeField] Transform bossLOC;
    private void Update()
    {
        if (inTrigger && Input.GetKeyDown(KeyCode.E))
        {
            if (!istalking)
            {
                
                StartCoroutine(Dialogue1(4.5f));
            }
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

    IEnumerator Dialogue1(float time)
    {
        PlayerController.Instance.pState.canPause = false;
        QuestTracker.instance.hasQuest = false;
        PlayerPrefs.DeleteKey("Quest");
        UI.SetActive(false);
        istalking = true;
        notiftext.text = "";
        PlayerController.Instance.pState.isNPC = true;
        NPCDIALOGUE.SetActive(true);
        string[] words = new[]
        {
            "What happened to Lapu-Lapu",
            "He was killed by Magellan... but not by mortal means. Magellan had the help of the devil. He was no ordinary man.",
            "His armor turned red, radiating a power that made him unstoppable. Lapu-Lapu fought bravely, but he couldn’t withstand it. That’s how we lost our island.",
            "Now, we hide in the shadows, waiting... praying for a miracle to turn the tide.",
        };

        string[] names = new[]
        {
            "",
            "Tausūg Scout",
            "Tausūg Scout",
            "Tausūg Scout"
        };

        for (int i = 0; i < words.Length; i++)
        {
            float elapsedtime = 0f;
            dialogue.text = words[i];
            npcName.text = names[i];
            while (elapsedtime < time)
            {
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    elapsedtime = time;
                    break;
                }
                elapsedtime += Time.deltaTime;
                yield return null;
            }
        }
        NPCDIALOGUE.SetActive(false);
        PlayerController.Instance.pState.Transitioning = true;
        yield return new WaitForSeconds(time);
        npc1.SetActive(true);
        npc2.SetActive(true);
        npc3.SetActive(true);
        npc4.SetActive(true);
        PlayerController.Instance.pState.Transitioning = false;
        yield return new WaitForSeconds(time);
        NPCDIALOGUE.SetActive(true);

        string[] words2 = new[]
        {
            "Lapu-Lapu didn’t give his life so you could hide in the shadows. He fought to give you strength, to show you that this land is worth fighting for.",
            "This is not what he wanted when he stood against Magellan. He believed in all of you, in your courage, your spirit, and your unity.",
            "Stand with me now. Help me wield his words and his legacy, so together, we can reclaim your kingdom!",
            "Our swords are yours, Zeick. We will not let Magellan's darkness rule our land any longer.",
            "For Lapu-Lapu! For our people!",
            "I will take you to the nearest shortcut to Magellan, and we will stop the reinforcement on its way to his camp.",
            "keep in mind he is no longer a man don't hesitate if there is a chance to end this."
        };
        string[] names2 = new[]
        {
            "",
            "",
            "",
            "Tausūg Scout",
            "Tausūg Warrior",
            "Tausūg Scout",
            "Tausūg Scout"
        };

        for (int i = 0; i < words.Length; i++)
        {
            float elapsedtime1 = 0f;
            dialogue.text = words2[i];
            npcName.text = names2[i];
            while (elapsedtime1 < time)
            {
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    elapsedtime1 = time;
                    break;
                }
                elapsedtime1 += Time.deltaTime;
                yield return null;
            }
        }

        NPCDIALOGUE.SetActive(false);
        PlayerController.Instance.pState.Transitioning = true;
        yield return new WaitForSeconds(time - 2f);
        PlayerController.Instance.pState.Transitioning = false;
        PlayerController.Instance.transform.position = bossLOC.transform.position;
        istalking = false;
        PlayerController.Instance.pState.isNPC = false;
        UI.SetActive(true);
        QuestTracker.instance.hasQuest = true;
        PlayerPrefs.SetString("Quest", "Defeat Magellan");
        PlayerController.Instance.pState.canPause = true;
        Save.instance.saveData();
    }
}

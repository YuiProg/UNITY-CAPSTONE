using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;
using UnityEngine.UI;

public class TondoTeleporter : MonoBehaviour
{
    [SerializeField] GameObject DIALOGUE;
    [SerializeField] GameObject UI;
    [SerializeField] Text dlg;
    [SerializeField] Transform tpHere;

    bool inTrigger;
    bool isTalking = false;
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && inTrigger && !isTalking)
        {
            StartCoroutine(dialogue(4.5f));
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

    IEnumerator dialogue(float time)
    {
        PlayerController.Instance.pState.canPause = false;
        PlayerController.Instance.pState.isNPC = true;
        DIALOGUE.SetActive(true);
        UI.SetActive(false);
        Cursor.visible = true;

        string[] words = new[]
        {
            "Trusted by the marked warrior of blood, Whose steps uphold the eternal light.",
            "Because of his peace, order shall thrive, Ensuring existence, keeping us alive.",
            "Only he, the chosen one, can unravel the threads of night. With these artifacts, one mistake will open the gate.",
            "In the chosen one's battle, the rogue traveler's power awaits. Our existence depends on his fate."
        };

        for (int i = 0; i < words.Length; i++)
        {
            float elapsedtime = 0f;
            dlg.text = words[i];
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
        DIALOGUE.SetActive(false);
        PlayerController.Instance.pState.Transitioning = true;
        yield return new WaitForSeconds(time - 2);
        PlayerController.Instance.pState.Transitioning = false;
        PlayerController.Instance.transform.position = tpHere.position;
        Save.instance.saveData();
        LevelManager.instance.loadscene("Cave_1");
    }



}

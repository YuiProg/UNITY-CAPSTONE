using JetBrains.Annotations;
using System.Collections;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class NPC_2_DIALOGUE : MonoBehaviour
{
    [SerializeField] Text dialogue;
    [SerializeField] GameObject dlg;
    [SerializeField] Text item;
    [SerializeField]int count = 0;
    private bool isPlayerInTrigger = false;
    void Start()
    {
        dlg.SetActive(false);
        dialogue.text = " ";
    }

    
    

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            isPlayerInTrigger = true; 
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            isPlayerInTrigger = false; 
        }
    }

    private void Update()
    {
        if (isPlayerInTrigger && Input.GetKeyDown(KeyCode.Q))
        {
            Dialogue();
            count++; 
        }
    }

    void Dialogue()
    {
        switch (count)
        {
            case 0:
                dlg.SetActive(true);
                dialogue.text = "So another one lost in time yeah?";
                break;
            case 1:
                dialogue.text = "My name is Yuri";
                break;
            case 2:
                dialogue.text = "A traveler lost in time just like you";
                break;
            case 3:
                dialogue.text = "If you venture deeper into this place";
                break;
            case 4:
                dialogue.text = "If you venture deeper into this place";
                break;
            case 5:
                dialogue.text = "You will face the demon";
                break;
            case 6:
                dialogue.text = "I can't defeat the demon";
                break;
            case 7:
                dialogue.text = "It's too powerful for me";
                break;
            case 8:
                dialogue.text = "Please do me a favor and kill the demon";
                break;
            case 9:
                dialogue.text = "Here have this key";
                break;
            case 10:
                dialogue.text = "It will lead you to the demon";
                break;
            case 11:
                PlayerController.Instance.pState.hasKey = true;
                item.text = "KEY OBTAINED";
                break;
            case 12:
                dialogue.text = "...";
                break;
            case 13:
                dialogue.text = "I'm sorry trix";
                break;
            case 14:
                dialogue.text = "But I have to do this";
                break;
            case 15:
                dialogue.text = "For us to get out";
                break;
            case 16:
                dialogue.text = "In this forsaken timeline";
                break;
            case 17:
                dlg.SetActive(false);
                count = 0;
                break;
            default:
                break;
        }
    }
}

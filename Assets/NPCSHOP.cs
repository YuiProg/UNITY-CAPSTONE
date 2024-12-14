using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NPCSHOP : MonoBehaviour
{
    bool isTalking = false;
    bool inTrigger = false;
    public bool shopopen = false;
    [SerializeField] GameObject txtBox;
    [SerializeField] GameObject SHOP;
    [SerializeField] Text dlg;
    [SerializeField] Text Name;
    [SerializeField] GameObject ui;
    Animator anim;

    int count = 0;

    private void Start()
    {
        anim = GetComponent<Animator>();
        SHOP.SetActive(false);
    }

    private void Update()
    {
        if (!isTalking && inTrigger)
        {
            if (Input.GetKeyDown(KeyCode.E) && !shopopen)
            {
                count++;
                StartCoroutine(Dialogue(4.5f));
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
    IEnumerator Dialogue(float time)
    {
        txtBox.SetActive(true);
        isTalking = true;
        ui.SetActive(false);
        PlayerController.Instance.pState.isNPC = true;
        PlayerController.Instance.pState.canPause = false;
        Cursor.visible = true;
        string[] dialogue = new[]
        {
            "Hey it's you again!",
            "I sell and upgrade everything you need for your journey!",
            "Feel free to look",
            "You can use that teleporter to go back when you're done!"
        };

        string[] name = new[]
        {
            "Oliver",
            "Oliver",
            "Oliver",
            "Oliver"
        };

        for (int i = 0; i < dialogue.Length; i++)
        {
            dlg.text = dialogue[i];
            Name.text = name[i];
            float elapsedtime = 0f;

            while (elapsedtime < time)
            {
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    elapsedtime = time;
                    break;
                }
                else if (Input.GetKeyDown(KeyCode.F))
                {
                    elapsedtime = time;
                }
                elapsedtime += Time.deltaTime;
                yield return null;
            }
        }
        dlg.text = "";
        Name.text = "";
        txtBox.SetActive(false);
        isTalking = false;
        openShop();

    }

    void openShop()
    {
        shopopen = true;
        SHOP.SetActive(true);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Checkpoint : MonoBehaviour
{
    public Transform respawnpoint;
    [SerializeField] Text saveTXT;
    public GameObject upgradeUI;
    bool onTrigger;
    bool onUI = false;
    bool hasSaved = false;
    private void Update()
    {
        if (onTrigger && Input.GetKeyDown(KeyCode.E))
        {
            if (onUI)
            {
                PlayerController.Instance.pState.isNPC = true;
                upgradeUI.SetActive(true);
                Cursor.visible = true;
                onUI = false;
            }
            else
            {
                PlayerController.Instance.pState.isNPC = false;
                onUI = true;
                Cursor.visible = false;
                upgradeUI.SetActive(false);
            }
        }        
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            PlayerPrefs.SetInt("LOAD", 1);
            onTrigger = true;
            Save.instance.saveData();
            StartCoroutine(save());
            PlayerController.Instance.updatecheckpoint(respawnpoint.position);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            onTrigger = false;
            upgradeUI.SetActive(false);
        }
    }

    private IEnumerator save()
    {
        if (!hasSaved)
        {
            saveTXT.text = "Saving...";
            hasSaved = true;
            yield return new WaitForSeconds(1.5f);
            saveTXT.text = " ";
        }
        
    }
}

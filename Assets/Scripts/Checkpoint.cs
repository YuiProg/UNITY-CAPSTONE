using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Checkpoint : MonoBehaviour
{
    public Transform respawnpoint;
    [SerializeField] Text saveTXT;
    [SerializeField] GameObject UI;
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
                UI.SetActive(false);
                PlayerController.Instance.pState.isNPC = true;
                PlayerController.Instance.pState.canPause = false;
                upgradeUI.SetActive(true);
                Cursor.visible = true;
                onUI = false;
            }
            else
            {
                UI.SetActive(true);
                PlayerController.Instance.pState.isNPC = false;
                PlayerController.Instance.pState.canPause = true;
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
            PlayerController.Instance.health = PlayerController.Instance.maxHealth;
            PlayerController.Instance.HealthBar.fillAmount = PlayerController.Instance.maxHealth;
            PlayerController.Instance.potionCount = PlayerController.Instance.maxPotions;
            PlayerController.Instance.shieldCount = PlayerController.Instance.maxShield;
            PlayerController.Instance.ShieldBar.fillAmount = PlayerController.Instance.maxShield;
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

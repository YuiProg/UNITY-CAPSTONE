using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Shop : MonoBehaviour
{
    [SerializeField] Text remainingCoins;
    [SerializeField] GameObject UI;


    public int purchaseCountEssence;
    public int purchaseCountPotion;
    bool hasFunds;
    public float elapsedTime;
    private Coroutine purchaseCoroutine;
    NPCSHOP npcShop;

    AudioManager audiomanager;
    private void Start()
    {
        audiomanager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
        npcShop = GameObject.FindGameObjectWithTag("Shop").GetComponent<NPCSHOP>();
    }
    private void Update()
    {
        elapsedTime += Time.deltaTime;
        remainingCoins.text =  $"{PlayerController.Instance.barya.ToString()}";

        if (Input.GetKeyDown(KeyCode.E) || Input.GetKeyDown(KeyCode.Escape))
        {
            audiomanager.StopSFX();
            audiomanager.PlaySFX(audiomanager.OLIVERTY);
            Cursor.visible = false;
            gameObject.SetActive(false);
            npcShop.shopopen = false;
            PlayerController.Instance.pState.isNPC = false;
            PlayerController.Instance.pState.canPause = true;
            PlayerController.Instance.pState.canOpenJournal = true;
            UI.SetActive(true);
        }
    }

    IEnumerator PurchaseEssence(float time)
    {
        if (hasFunds)
        {
            audiomanager.PlaySFX(audiomanager.BUTTONCLICK);
        }
        else
        {
            audiomanager.PlaySFX(audiomanager.BUTTONCLICK2);
        }

        yield return new WaitForSeconds(time);

    }
    IEnumerator PurchasePotion(float time)
    {
        if (PlayerController.Instance.potionCount > 8)
        {
            audiomanager.PlaySFX(audiomanager.BUTTONCLICK2);

            yield return new WaitForSeconds(time);
        }
        if (hasFunds)
        {
            if (purchaseCountPotion > 1)
            {
                audiomanager.PlaySFX(audiomanager.BUTTONCLICK);
            }
            else
            {
                audiomanager.PlaySFX(audiomanager.BUTTONCLICK);
            }
        }
        else
        {
            audiomanager.PlaySFX(audiomanager.BUTTONCLICK2);
        }

        yield return new WaitForSeconds(time);


    }

    public void upgradeEffectiveness()
    {
        elapsedTime = 0;

        if (PlayerController.Instance.barya >= 25)
        {
            audiomanager.PlaySFX(audiomanager.BUTTONCLICK);
            hasFunds = true;
            PlayerController.Instance.barya = PlayerController.Instance.barya - 25;
            PlayerController.Instance.potionHealBar = PlayerController.Instance.potionHealBar + 5f;
            Save.instance.saveStats();
        }
        else
        {
            audiomanager.PlaySFX(audiomanager.BUTTONCLICK2);
            hasFunds = false;
        }
    }
    public void BuyEssence()
    {
        elapsedTime = 0;

        if (PlayerController.Instance.barya >= 20)
        {
            hasFunds = true;
            purchaseCountEssence++;
            PlayerController.Instance.barya = PlayerController.Instance.barya - 20; 
            PlayerController.Instance.levels = PlayerController.Instance.levels + 1;
            Save.instance.saveStats();
        }
        else
        {
            hasFunds = false;
        }

        if (purchaseCoroutine != null)
        {
            StopCoroutine(purchaseCoroutine);
        }
        purchaseCoroutine = StartCoroutine(PurchaseEssence(2f));
    }


    public void purchasePotion()
    {
        elapsedTime = 0;
        if (PlayerController.Instance.maxPotions <= 8)
        {
            if (PlayerController.Instance.barya >= 25)
            {
                hasFunds = true;
                purchaseCountPotion++;
                PlayerController.Instance.barya = PlayerController.Instance.barya - 25;
                PlayerController.Instance.maxPotions = PlayerController.Instance.maxPotions + 1;
                PlayerController.Instance.potionCount = PlayerController.Instance.maxPotions;
                Save.instance.saveStats();
            }
            else
            {
                hasFunds = false;
            }
            if (purchaseCoroutine != null)
            {
                StopCoroutine(purchaseCoroutine);
            }
            purchaseCoroutine = StartCoroutine(PurchasePotion(2f));
        }
        else
        {
            audiomanager.PlaySFX(audiomanager.BUTTONCLICK2);
        }
        

    }
}

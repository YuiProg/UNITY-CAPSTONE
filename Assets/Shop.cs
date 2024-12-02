using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Shop : MonoBehaviour
{
    [SerializeField] Text purchased;
    [SerializeField] Text remainingCoins;
    [SerializeField] GameObject UI;

    //shop
    [SerializeField] GameObject essenceShop;
    [SerializeField] GameObject potionShop;

    public int purchaseCount;
    bool hasFunds;
    public float elapsedTime;
    private Coroutine purchaseCoroutine;
    NPCSHOP npcShop;

    private void Start()
    {
        npcShop = FindObjectOfType<NPCSHOP>();
    }
    private void Update()
    {
        elapsedTime += Time.deltaTime;
        remainingCoins.text =  $"{PlayerController.Instance.barya.ToString()}";

        if (Input.GetKeyDown(KeyCode.E))
        {
            gameObject.SetActive(false);
            npcShop.shopopen = false;
            PlayerController.Instance.pState.isNPC = false;
            UI.SetActive(true);
        }
    }

    IEnumerator PurchaseEssence(float time)
    {
        if (hasFunds)
        {
            purchased.text = $"Purchased {purchaseCount} Essence!";
        }
        else
        {
            purchased.text = "Insufficient Funds!";
        }

        yield return new WaitForSeconds(time);

        if (elapsedTime >= time)
        {
            purchased.text = "";
        }
    }
    IEnumerator PurchasePotion(float time)
    {
        if (PlayerController.Instance.potionCount > 8)
        {
            purchased.text = $"You have all the potions!";

            yield return new WaitForSeconds(time);

            if (elapsedTime >= time)
            {
                purchased.text = "";
            }
        }
        if (hasFunds)
        {
            if (purchaseCount > 1)
            {
                purchased.text = $"Purchased {purchaseCount} Potions!";
            }
            else
            {
                purchased.text = $"Purchased {purchaseCount} Potion!";
            }
        }
        else
        {
            purchased.text = "Insufficient Funds!";
        }

        yield return new WaitForSeconds(time);

        if (elapsedTime >= time)
        {
            purchased.text = "";
        }

    }
    public void BuyEssence()
    {
        elapsedTime = 0;

        if (PlayerController.Instance.barya >= 20)
        {
            hasFunds = true;
            purchaseCount++;
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

    public void showPotionShop()
    {
        potionShop.SetActive(true);
        essenceShop.SetActive(false);
    }

    public void showEssenceShop()
    {
        potionShop.SetActive(false);
        essenceShop.SetActive(true);
    }

    public void purchasePotion()
    {
        elapsedTime = 0;
        if (PlayerController.Instance.maxPotions <= 8)
        {
            if (PlayerController.Instance.barya >= 25)
            {
                hasFunds = true;
                purchaseCount++;
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
            purchased.text = "You have all the potions!";
        }
        

    }
}

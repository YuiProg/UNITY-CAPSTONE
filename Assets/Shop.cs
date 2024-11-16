using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Shop : MonoBehaviour
{
    [SerializeField] Text purchased;
    [SerializeField] Text remainingCoins;
    [SerializeField] GameObject UI;
    public int purchaseCount;
    bool hasFunds;
    public float elapsedTime;
    private Coroutine purchaseCoroutine;

    private void Update()
    {
        elapsedTime += Time.deltaTime;
        remainingCoins.text =  $"AMBER - {PlayerController.Instance.barya.ToString()}";

        if (Input.GetKeyDown(KeyCode.E))
        {
            gameObject.SetActive(false);
            NPCSHOP.instance.shopopen = false;
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

    public void BuyEssence()
    {
        elapsedTime = 0;

        if (PlayerController.Instance.barya >= 50)
        {
            hasFunds = true;
            purchaseCount++;
            PlayerController.Instance.barya = PlayerController.Instance.barya - 50; 
            PlayerController.Instance.levels = PlayerController.Instance.levels + 1; 
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
}

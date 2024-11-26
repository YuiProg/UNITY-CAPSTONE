using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MapPickup : MonoBehaviour
{
    [SerializeField] GameObject NPCDLG;
    [SerializeField] Text ITEM;
    bool ongoing = false;
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (!ongoing)
            {
                ongoing = true;
                PlayerPrefs.SetInt("MAP", 1);
                StartCoroutine(ITEMPICKUP());
            }
        }    
    }

    IEnumerator ITEMPICKUP()
    {
        NPCDLG.SetActive(true);
        ITEM.text = "MAP OBTAINED";
        Save.instance.saveData();
        PlayerPrefs.DeleteKey("Quest");
        yield return new WaitForSeconds(3f);
        NPCDLG.SetActive(false);
        ITEM.text = "";
        Destroy(gameObject);
    }

}

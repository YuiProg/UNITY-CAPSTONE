using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FOGBORDERFINAL : MonoBehaviour
{
    bool inTrigger;
    [SerializeField] GameObject DIAOGUE;
    [SerializeField] GameObject UI;
    [SerializeField] Text dlg;
    [SerializeField] GameObject BORDER;
    bool isTalking = false;

    private void Awake()
    {
        gameObject.SetActive(PlayerPrefs.GetInt("FINALAREABORDER") != 1);
        BORDER.SetActive(PlayerPrefs.GetInt("FINALAREABORDER") != 1);
    }
    private void Update()
    {
        if (inTrigger)
        {
            StartCoroutine(Dialogue(4.5f));
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
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
        isTalking = true;
        PlayerController.Instance.pState.isNPC = true;
        UI.SetActive(false);
        DIAOGUE.SetActive(true);
        dlg.text = "Something was destroyed.";
        yield return new WaitForSeconds(time);
        UI.SetActive(true);
        isTalking = false;
        PlayerController.Instance.pState.isNPC = false;
        DIAOGUE.SetActive(false);
        PlayerPrefs.SetInt("FINALAREABORDER", 1);
        Destroy(gameObject);
    }
}

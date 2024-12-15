using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FINALAREATELEPORTER : MonoBehaviour
{
    bool inTrigger;
    bool teleporting = false;
    [SerializeField] Transform tphere;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (PlayerPrefs.GetInt("TONDOMBOSS") != 1) gameObject.SetActive(false);

        if (inTrigger && !teleporting)
        {
            StartCoroutine(teleport(4.5f));
        }
    }

    IEnumerator teleport(float time)
    {
        teleporting = true;
        PlayerController.Instance.pState.canPause = false;
        PlayerController.Instance.pState.isNPC = true;
        PlayerController.Instance.pState.canOpenJournal = false;
        PlayerPrefs.SetInt("inIfugao", 0);
        PlayerPrefs.SetInt("inMactan", 0);
        PlayerPrefs.SetInt("inTondo", 0);
        PlayerPrefs.SetInt("inSQ", 0);
        PlayerPrefs.SetInt("inSpace", 1);
        PlayerPrefs.SetInt("inCave", 0);
        PlayerController.Instance.pState.Transitioning = true;
        yield return new WaitForSeconds(time - 2);
        PlayerController.Instance.transform.position = tphere.position;
        Save.instance.saveData();
        LevelManager.instance.loadscene("Cave_1");
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
}

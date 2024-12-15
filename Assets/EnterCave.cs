using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnterCave : MonoBehaviour
{
    bool ontrigger;
    public Transform tphere;
    bool hasTPD = false;
    [SerializeField] GameObject BORDER;
    // Update is called once per frame
    void Update()
    {
        BORDER.SetActive(PlayerPrefs.GetString("Quest") == "Go to the forest");
        if (ontrigger && PlayerPrefs.GetInt("NPCINCAVE") == 1 && PlayerPrefs.GetString("Quest") != "Go to the forest" && !hasTPD)
        {
            StartCoroutine(tpstart(5f));
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            ontrigger = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            ontrigger = false;
        }
    }

    IEnumerator tpstart(float time)
    {
        hasTPD = true;
        PlayerController.Instance.pState.Transitioning = true;
        yield return new WaitForSeconds(2.5f);
        PlayerController.Instance.transform.position = tphere.transform.position;
        PlayerPrefs.SetInt("inIfugao", 0);
        PlayerPrefs.SetInt("inMactan", 0);
        PlayerPrefs.SetInt("inTondo", 0);
        PlayerPrefs.SetInt("inSQ", 0);
        PlayerPrefs.SetInt("inSpace", 0);
        PlayerPrefs.SetInt("inCave", 1);
        yield return new WaitForSeconds(time - 2.7f);
        Save.instance.saveData();
        PlayerPrefs.SetString("Quest", "Talk to chrstina and balweg");
        PlayerController.Instance.pState.Transitioning = false;
        LevelManager.instance.loadscene("Cave_1");
        
        
    }

}

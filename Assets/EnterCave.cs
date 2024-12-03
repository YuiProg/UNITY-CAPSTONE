using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnterCave : MonoBehaviour
{
    bool ontrigger;
    public Transform tphere;
    [SerializeField] GameObject BORDER;
    // Update is called once per frame
    void Update()
    {
        BORDER.SetActive(PlayerPrefs.GetString("Quest") == "Go to the forest");
        if (ontrigger && PlayerPrefs.GetInt("NPCINCAVE") == 1 && PlayerPrefs.GetString("Quest") != "Go to the forest")
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
        PlayerController.Instance.pState.Transitioning = true;
        yield return new WaitForSeconds(2.5f);
        PlayerController.Instance.transform.position = tphere.transform.position;
        yield return new WaitForSeconds(time - 2);
        Save.instance.saveData();
        PlayerPrefs.SetString("Quest", "Talk to chrstina and balweg");
        PlayerController.Instance.pState.Transitioning = false;
    }

}

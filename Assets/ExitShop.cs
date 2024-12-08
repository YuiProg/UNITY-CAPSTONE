using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitShop : MonoBehaviour
{
    bool inTrigger = false;
    bool teleporting = false;
    // Update is called once per frame
    void Update()
    {
        if (inTrigger && Input.GetKeyDown(KeyCode.E))
        {
            StartCoroutine(TeleportBack());
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
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


    IEnumerator TeleportBack()
    {
        teleporting = true;
        PlayerController.Instance.pState.Transitioning = true;
        yield return new WaitForSeconds(2.5f);
        PlayerController.Instance.transform.position = new Vector3(PlayerPrefs.GetFloat("X"), PlayerPrefs.GetFloat("Y"));
        Save.instance.saveData();
        teleporting = false;
        PlayerController.Instance.pState.Transitioning = false;
        LevelManager.instance.loadscene("Cave_1");
    }
}

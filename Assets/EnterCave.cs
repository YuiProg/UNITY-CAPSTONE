using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnterCave : MonoBehaviour
{
    bool ontrigger;
    public Transform tphere;
    // Update is called once per frame
    void Update()
    {
        if (ontrigger && Input.GetKeyDown(KeyCode.E))
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
        yield return new WaitForSeconds(2f);
        PlayerController.Instance.transform.position = tphere.transform.position;
        yield return new WaitForSeconds(time);
        PlayerController.Instance.pState.Transitioning = false;
    }

}

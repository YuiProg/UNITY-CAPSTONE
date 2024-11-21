using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZoomOutTrigger : MonoBehaviour
{

    bool inTrigger;
    void Update()
    {
        if (inTrigger)
        {
            PlayerController.Instance.pState.inParkourState = true;
        }
        else
        {
            PlayerController.Instance.pState.inParkourState = false;
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
}

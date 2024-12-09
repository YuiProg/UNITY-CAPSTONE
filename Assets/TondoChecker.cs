using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TondoChecker : MonoBehaviour
{
    bool inTrigger;
    bool activated = false;
    void Update()
    {
        if (inTrigger && !activated)
        {
            activated = true;
            PlayerController.Instance.pState.inTondo = true;
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
}

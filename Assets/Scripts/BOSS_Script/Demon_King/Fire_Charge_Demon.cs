using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fire_Charge_Demon : MonoBehaviour
{
    float attacktimer;
    bool inArea = false;
    private void Update()
    {
        attacktimer += Time.deltaTime;
        takeDamage();
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            inArea = true;            
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            inArea = false;
        }
    }

    void takeDamage()
    {
        if (attacktimer > 1 && inArea)
        {
            attacktimer = 0;
            PlayerController.Instance.TakeDamage(50);
        }
    }
}

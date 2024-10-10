using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bonus_Health_Buff : MonoBehaviour
{
    float bonusHP = 30;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            PlayerController.Instance.maxHealth += bonusHP;
            PlayerController.Instance.health += bonusHP;
            Destroy(gameObject);
        }

    }
}

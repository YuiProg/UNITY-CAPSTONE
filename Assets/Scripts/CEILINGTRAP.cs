using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CEILINGTRAP : MonoBehaviour
{
    [SerializeField] float damage;
    private void Attack()
    {
        PlayerController.Instance.TakeDamage(damage);
        PlayerController.Instance.HitStopTime(0, 5, 0.5f);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && !PlayerController.Instance.pState.invincible && !PlayerController.Instance.pState.blocking)
        {
            Attack();
        }
    }
}

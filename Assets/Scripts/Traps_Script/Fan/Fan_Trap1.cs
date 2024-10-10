using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fan_Trap1 : MonoBehaviour
{
    [SerializeField] float damage;
    Animator anim;
    
    void Start()
    {
        anim = GetComponent<Animator>();
    }


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

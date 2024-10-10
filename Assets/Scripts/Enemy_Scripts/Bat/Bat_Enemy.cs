using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Http.Headers;
using System.Threading;
using UnityEngine;


public class Bat_Enemy : Enemy
{
    [SerializeField] private float chaseDistance;
    
    protected override void Start()
    {
        base.Start();
        ChangeStates(EnemyStates.Bat_Idle);
        canAttack = true;
    }

    protected override void UpdateEnemyStates()
    {
        float _dist = Vector2.Distance(transform.position, PlayerController.Instance.transform.position);
        if (health <= 0)
        {
            Destroy(gameObject);
        }
        switch (currentEnemyStates)
        {
            case EnemyStates.Bat_Chase:
                rb.MovePosition(Vector2.MoveTowards(transform.position, PlayerController.Instance.transform.position, Time.deltaTime * speed));
                FlipBat();
                break;
            case EnemyStates.Bat_Idle:
                if (_dist < chaseDistance)
                {
                    ChangeStates(EnemyStates.Bat_Chase);
                }
                break;
            default:
                break;
        }
    }

    void FlipBat()
    {
        sr.flipX = PlayerController.Instance.transform.position.x > transform.position.x;
    }
}

using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class SAMURAI_BOSS : Enemy
{
    [SerializeField] float chaseDistance;
    float comboTimer;
    float canAttackTimer;
    bool spottedPlayer;
    Vector2 spawnPoint;
    Animator anim;

    protected override void Start()
    {
        base.Start();
        canAttack = true;
        anim = GetComponent<Animator>();
    }

    protected override void UpdateEnemyStates()
    {
        float _dist = Vector2.Distance(transform.position, PlayerController.Instance.transform.position);
        canAttackTimer += Time.deltaTime;
        switch (currentEnemyStates)
        {             
            case EnemyStates.SM_Idle:
                if (_dist < chaseDistance)
                {
                    spottedPlayer = true;
                    ChangeStates(EnemyStates.SM_Chase);
                }
                break;
            case EnemyStates.SM_Chase:
                break;
            default:
                break;
        }
    }

    IEnumerator AttackSM()
    {
        canMove = false;
        yield return new WaitForSeconds(1f);
        canMove = true;
    }
    void Walk()
    {
        if (spottedPlayer && canMove)
        {
            if (canMove && Vector2.Distance(transform.position, PlayerController.Instance.transform.position) <= 3f)
            {
                canAttackTimer = 0;
                StartCoroutine(AttackSM());
            }
            else if (canMove && Vector2.Distance(transform.position, PlayerController.Instance.transform.position) >= 4f)
            {
                StopCoroutine(AttackSM());               
                if (canAttackTimer > 1)
                {
                    canMove = false;

                    transform.position = Vector2.MoveTowards
                    (transform.position, new Vector2(PlayerController.Instance.transform.position.x, transform.position.y),
                    speed * Time.deltaTime);
                }
            }
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AXE_ENEMY : Enemy
{
    public float chaseDistance;
    bool spottedPlayer = false;
    bool isAttacking = false;
    Animator anim;

    protected override void Start()
    {
        base.Start();
        canMove = true;
        canAttack = true;
        rb.gravityScale = 12f;
        anim = GetComponent<Animator>();
        ChangeStates(EnemyStates.AXE_IDLE);
    }

    protected override void UpdateEnemyStates()
    {
        float distance = Vector2.Distance(PlayerController.Instance.transform.position, transform.position);
        stateChecker();
        flip(!isAttacking && health >= 0);
        if (canMove)
        {
            switch (currentEnemyStates)
            {
                case EnemyStates.AXE_IDLE:
                    if (distance < chaseDistance)
                    {
                        spottedPlayer = true;
                        ChangeStates(EnemyStates.AXE_CHASE);
                    }
                    break;
                case EnemyStates.AXE_CHASE:
                    if (distanceCheck())
                    {
                        ChangeStates(EnemyStates.AXE_ATTACKBEHAVIOR);
                    }
                    anim.SetBool("Chase", true);
                    transform.position = Vector2.MoveTowards(transform.position, 
                        new Vector2(PlayerController.Instance.transform.position.x, transform.position.y), speed * Time.deltaTime);

                    break;
                case EnemyStates.AXE_ATTACKBEHAVIOR:
                    if (!isAttacking)
                    {
                        anim.SetBool("Chase", false);
                        AttackPattern();
                    }
                    break;
                default:
                    break;
            }
        }


    }

    void stateChecker()
    {
        canMove = !parried;
        canAttack = !parried;
        if (parried) anim.SetTrigger("Parried");
        if (health <= 0)
        {
            canAttack = false;
            canMove = false;
            anim.SetTrigger("Die");
            Destroy(gameObject, 1.5f);
        }
    }
    void AttackPattern()
    {
        int i = Random.Range(0,2);

        switch (i)
        {
            case 0:
                StartCoroutine(Attack1());
                break;
            case 1:
                StartCoroutine(Attack2());
                break;
            default:
                break;
        }
    }

    //attacks

    IEnumerator Attack1()
    {
        isAttacking = true;
        canMove = false;
        anim.SetTrigger("Attack1");
        yield return new WaitForSeconds(1f);
        isAttacking = false;
        canMove = true;
        ChangeStates(EnemyStates.AXE_IDLE);
    }

    IEnumerator Attack2()
    {
        isAttacking = true;
        canMove = false;
        anim.SetTrigger("Attack2");
        yield return new WaitForSeconds(1.3f);
        isAttacking = false;
        canMove = true;
        ChangeStates(EnemyStates.AXE_IDLE);
    }

    void flip(bool canFlip)
    {
        if (canFlip)
        {
            if (PlayerController.Instance.transform.position.x > transform.position.x)
            {
                transform.eulerAngles = new Vector3(0, 180, 0);
                healthBar.transform.eulerAngles = new Vector3(0, 0 ,0);
            }
            else
            {
                transform.eulerAngles = new Vector3(0, 0, 0);
                healthBar.transform.eulerAngles = new Vector3(0, 0, 0);
            }
        }
    }

    public bool distanceCheck()
    {
        float distance = Vector2.Distance(PlayerController.Instance.transform.position, transform.position);
        return distance < 2.8f;
    }
}

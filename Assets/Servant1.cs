using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Servant1 : Enemy
{
    Animator anim;
    bool spottedPlayer = false;
    bool isAttacking = false;
    public float chaseDistance;
    protected override void Start()
    {
        base.Start();
        anim = GetComponent<Animator>();
        rb.gravityScale = 12f;
        ChangeStates(EnemyStates.S1_Idle);
    }


    protected override void UpdateEnemyStates()
    {
        stateCheck();
        flip();

        float distance = Vector2.Distance(PlayerController.Instance.transform.position, transform.position);
        if (canMove && Servant_Enemy.instance.servantscanATK)
        {
            switch (currentEnemyStates)
            {
                case EnemyStates.S1_Idle:
                    if (chaseDistance < distance)
                    {
                        ChangeStates(EnemyStates.S1_Chase);
                    }
                    break;
                case EnemyStates.S1_Chase:
                    anim.SetBool("Running", true);
                    transform.position = Vector2.MoveTowards(transform.position,
                        new Vector2(PlayerController.Instance.transform.position.x, transform.position.y), speed * Time.deltaTime);

                    if (distanceCheck())
                    {
                        ChangeStates(EnemyStates.S1_AttackBehavior);
                    }
                    break;
                case EnemyStates.S1_AttackBehavior:
                    if (!isAttacking)
                    {
                        AttackBehavior();
                    }
                    
                    break;
                default:
                    break;
            }
        }
    }
    void stopattacks()
    {
        StopCoroutine(Attack1());
        StopCoroutine(Attack2());
    }
    void stateCheck()
    {
        canMove = !spottedPlayer;
        canAttack = !parried;
        canMove = !parried;
        if (parried)
        {
            stopattacks();
        }
        if (health <= 0)
        {
            canAttack = false;
            canMove = false;
            Destroy(gameObject);
        }
    }
    bool distanceCheck()
    {
        float distance = Vector2.Distance(transform.position, PlayerController.Instance.transform.position);
        return distance < 1f;
    }

    void AttackBehavior()
    {
        anim.SetBool("Running", false);
        int i = Random.Range(0, 2);
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
        ChangeStates(EnemyStates.S1_Chase);
    }

    IEnumerator Attack2()
    {
        isAttacking = true;
        canMove = false;
        anim.SetTrigger("Attack2");
        yield return new WaitForSeconds(2f);
        isAttacking = false;
        canMove = true;
        ChangeStates(EnemyStates.S1_Chase);
    }
    void flip()
    {
        if (PlayerController.Instance.transform.position.x < transform.position.x)
        {
            transform.eulerAngles = new Vector3(0, 180, 0);
            healthBar.transform.eulerAngles = new Vector3(0, 0, 0);
        }
        else
        {
            transform.eulerAngles = new Vector3(0, 0, 0);
            healthBar.transform.eulerAngles = new Vector3(0, 0, 0);
        }
    }
}

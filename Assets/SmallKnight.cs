using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmallKnight : Enemy
{
    bool spottedPlayer = false;
    bool isAttacking = false;
    public float chaseDistance;

    Animator anim;
    protected override void Start()
    {
        base.Start();
        rb.gravityScale = 12f;
        canMove = true;
        canAttack = true;
        anim = GetComponent<Animator>();
        ChangeStates(EnemyStates.SK_Idle);
    }

    protected override void UpdateEnemyStates()
    {
        float distance = Vector2.Distance(transform.position, PlayerController.Instance.transform.position);
        stateCheck();
        flip(!isAttacking && health > 0);
        if (canMove && PlayerController.Instance.pState.isAlive)
        {
            switch (currentEnemyStates)
            {
                case EnemyStates.SK_Idle:
                    if (distance < chaseDistance)
                    {
                        spottedPlayer = true;
                        ChangeStates(EnemyStates.SK_Chase);
                    }
                    break;
                case EnemyStates.SK_Chase:
                    if (distanceCheck())
                    {
                        ChangeStates(EnemyStates.SK_Attack);
                    }
                    anim.SetBool("Chase", true);
                    transform.position = Vector2.MoveTowards(transform.position, new Vector2(PlayerController.Instance.transform.position.x, transform.position.y), speed * Time.deltaTime);

                    break;
                case EnemyStates.SK_Attack:
                    if (!isAttacking)
                    {
                        StartCoroutine(Attack1());
                    }
                    break;
                default:
                    break;
            }
        }
    }

    void stateCheck()
    {
        if (health <= 0)
        {
            anim.SetTrigger("Death");
            canMove = false;
            canAttack = false;
            spottedPlayer = false;
            Destroy(gameObject, 1f);
        }
    }
    IEnumerator Attack1()
    {
        anim.SetBool("Chase", false);
        canMove = false;
        isAttacking = true;
        anim.SetTrigger("Attack");
        yield return new WaitForSeconds(1f);
        canMove = true;
        isAttacking = false;
        ChangeStates(EnemyStates.SK_Idle);
    }
    bool distanceCheck()
    {
        float distance = Vector2.Distance(transform.position, PlayerController.Instance.transform.position);
        return distance < 2f;
    }

    void flip(bool canflip)
    {
        if (canflip)
        {
            if (PlayerController.Instance.transform.position.x > transform.position.x)
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
}

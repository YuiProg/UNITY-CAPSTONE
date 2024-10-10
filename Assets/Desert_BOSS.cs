using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class Desert_BOSS : Enemy
{
    bool attacking = false;
    bool spottedPlayer = false;
    public float chaseDistance;
    Animator anim;
    protected override void Start()
    {
        base.Start();
        canAttack = true;
        canMove = true;
        anim = GetComponent<Animator>();
        ChangeStates(EnemyStates.DB_Idle);
    }

    protected override void UpdateEnemyStates()
    {
        float _dist = Vector2.Distance(PlayerController.Instance.transform.position, transform.position);
        statusCheck();
        if (canMove && !attacking)
        {
            switch (currentEnemyStates)
            {               
                case EnemyStates.DB_Idle:
                    if (_dist < chaseDistance)
                    {
                        spottedPlayer = true;
                        ChangeStates(EnemyStates.DB_Chase);
                    }
                    break;
                case EnemyStates.DB_Chase:
                    distanceCheck();
                    break;
                case EnemyStates.DB_AttackBehavior:
                    attackBehavior();
                    break;
                default:
                    break;
            }
        }
    }

    void statusCheck()
    {
        canMove = !parried;
        if (!attacking) flip();
    }
    bool inRange()
    {
        float _dist = Vector2.Distance(PlayerController.Instance.transform.position, transform.position);
        return _dist <= 3f;
    }

    void distanceCheck()
    {
        if (inRange())
        {
            anim.SetBool("Run", false);
            ChangeStates(EnemyStates.DB_AttackBehavior);
        }
        else
        {
            anim.SetBool("Run", true);
            transform.position = Vector2.MoveTowards(transform.position, 
                new Vector2(PlayerController.Instance.transform.position.x, transform.position.y), speed * Time.deltaTime);
        }
    }
    void attackBehavior()
    {
        int attacks = Random.Range(0,4);
        if (!attacking)
        {
            switch (attacks)
            {
                case 0:
                    StartCoroutine(Attack1());
                    break;
                case 1:
                    StartCoroutine(Attack2());
                    break;
                case 2:
                    StartCoroutine(Attack3());
                    break;
                case 3:
                    StartCoroutine(Attack4());
                    break;
                default:
                    break;
            }
        }
        
    }

    void flip()
    {
        if (PlayerController.Instance.transform.position.x > transform.position.x)
        {
            transform.eulerAngles = new Vector3(0, 0, 0);
        }
        else
        {
            transform.eulerAngles = new Vector3(0, 180, 0);
        }
    }

    //ATTACKS WALA PHASE 2

    IEnumerator Attack1()
    {
        canMove = false;
        attacking = true;
        anim.SetTrigger("Attack1");
        yield return new WaitForSeconds(1f);
        canMove = true;
        attacking = false;
        ChangeStates(EnemyStates.DB_Idle);
    }

    IEnumerator Attack2()
    {
        canMove = false;
        attacking = true;
        anim.SetTrigger("Attack2");
        yield return new WaitForSeconds(1.8f);
        canMove = true;
        attacking = false;
        ChangeStates(EnemyStates.DB_Idle);
    }
    IEnumerator Attack3()
    {
        canMove = false;
        attacking = true;
        anim.SetTrigger("Attack3");
        yield return new WaitForSeconds(2f);
        canMove = true;
        attacking = false;
        ChangeStates(EnemyStates.DB_Idle);
    }
    IEnumerator Attack4()
    {
        canMove = false;
        attacking = true;
        anim.SetTrigger("Ultimate");
        yield return new WaitForSeconds(2f);
        canMove = true;
        attacking = false;
        ChangeStates(EnemyStates.DB_Idle);
    }
}

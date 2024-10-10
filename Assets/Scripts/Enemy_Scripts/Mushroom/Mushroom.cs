using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mushroom : Enemy
{
    float timer;
    [SerializeField] private float chaseDistance;
    Animator anim;
    Vector2 spawnpoint;
    protected override void Start()
    {
        base.Start();
        canAttack = true;
        canMove = true;
        ChangeStates(EnemyStates.Mushroom_Idle);
        rb.gravityScale = 12f;
        spawnpoint = transform.position;
        anim = GetComponent<Animator>();
    }

    protected override void UpdateEnemyStates()
    {
        float _dist = Vector2.Distance(transform.position, PlayerController.Instance.transform.position);
        if (health <= 0)
        {
            canAttack = false;
            canMove = false;
            anim.SetBool("Dead", true);
            anim.SetBool("Walking", false);
            Destroy(gameObject, 2f);
        }
        if (!PlayerController.Instance.pState.isAlive)
        {
            transform.position = spawnpoint;
            health = maxHealth;
            ChangeStates(EnemyStates.Mushroom_Idle);

        }
        if (parried)
        {
            canMove = false;
        }
        if (canMove && canAttack)
        {
            switch (currentEnemyStates)
            {
                case EnemyStates.Mushroom_Idle:
                    if (_dist < chaseDistance)
                    {
                        ChangeStates(EnemyStates.Mushroom_Chase);
                    }
                    break;
                case EnemyStates.Mushroom_Chase:
                    anim.SetBool("Walking", true);
                    anim.SetBool("Attack", false);

                    transform.position = Vector2.MoveTowards
                    (transform.position, new Vector2(PlayerController.Instance.transform.position.x, transform.position.y),
                    speed * Time.deltaTime);
                    Flip();
                    if (distanceCheck())
                    {
                        ChangeStates(EnemyStates.Mushroom_Attacking);
                    }
                    break;
                case EnemyStates.Mushroom_Attacking:
                    StartCoroutine(Attacking());
                    break;
                default:
                    break;
            }
        }
    }
    bool attacking = false;
    bool distanceCheck()
    {
        float _dist = Vector2.Distance(PlayerController.Instance.transform.position, transform.position);
        return _dist < 1.8f;
    }

    IEnumerator Attacking()
    {
        canMove = false;
        attacking = true;
        anim.SetBool("Attack", true);
        yield return new WaitForSeconds(1f);
        canMove = true;
        anim.SetBool("Attack", false);
        attacking = !attacking;
        ChangeStates(EnemyStates.Mushroom_Idle);
    }
    void Flip()
    {
        if (PlayerController.Instance.transform.position.x < transform.position.x)
        {
            transform.eulerAngles = new Vector3(0, 180 ,0);
            healthBar.transform.eulerAngles = new Vector3(0,0,0);
        }
        else
        {
            transform.eulerAngles = new Vector3(0, 0, 0);
            healthBar.transform.eulerAngles = new Vector3(0, 0, 0);
        }
    }
}
    


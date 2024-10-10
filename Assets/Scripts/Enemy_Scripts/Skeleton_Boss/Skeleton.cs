using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Unity.VisualScripting;
using UnityEngine;


public class Skeleton : Enemy
{
    // Start is called before the first frame update
    [SerializeField] private float chaseDistance;
    Animator anim;
    public bool isAlive = true;
    Vector2 spawnPoint;
    protected override void Start()
    {
        base.Start();
        anim = GetComponent<Animator>();
        rb.gravityScale = 12f;
        ChangeStates(EnemyStates.Skeleton_Idle);
        canAttack = true;
        spawnPoint = transform.position;
    }

    protected override void UpdateEnemyStates()
    {
        float _dist = Vector2.Distance(transform.position, PlayerController.Instance.transform.position);
        if (!PlayerController.Instance.pState.isAlive)
        {
            transform.position = spawnPoint;
            health = maxHealth;
            parrypercent = parrymax;
            parryBar.fillAmount = parrymax;
            ChangeStates(EnemyStates.Skeleton_Idle);
            

        }
        if (parried)
        {
            canMove = false;
        }
        if (isAlive == false)
        {
            PlayerController.Instance.maxHealth += 0.03f;
            PlayerController.Instance.maxstamina += 0.03f;
            PlayerController.Instance.health = PlayerController.Instance.maxHealth;
            PlayerController.Instance.HealthBar.fillAmount = PlayerController.Instance.health;
        }
        if (health <= 0)
        {
            canAttack = false;
            isAlive = false;
            canMove = false;
            anim.SetTrigger("Death");
            Destroy(gameObject, 2f);
        }
        else
        {
            if (canMove && canAttack)
            {
                switch (currentEnemyStates)
                {
                    case EnemyStates.Skeleton_Idle:
                        if (_dist < chaseDistance)
                        {
                            canAttack = true;
                            canMove = true;
                            ChangeStates(EnemyStates.Skeleton_Chase);
                        }
                        break;
                    case EnemyStates.Skeleton_Chase:
                        anim.SetBool("Walking", true);
                        transform.position = Vector2.MoveTowards
                        (transform.position, new Vector2(PlayerController.Instance.transform.position.x, transform.position.y),
                        speed * Time.deltaTime);
                        Flip();
                        if (PlayerController.Instance.takingDamage)
                        {
                            ChangeStates(EnemyStates.Skeleton_Attacking);
                        }

                        break;
                    case EnemyStates.Skeleton_Attacking:
                        isRecoiling = true;
                        attackanim();
                        ChangeStates(EnemyStates.Skeleton_Chase);
                        break;
                    default:
                        break;
                }
            }
            
        }
    }
    void Flip()
    {
        sr.flipX = PlayerController.Instance.transform.position.x < transform.position.x;
    }
    void attackanim()
    {
        if (Vector2.Distance(transform.position, PlayerController.Instance.transform.position) <= 3f)
        {
            anim.SetTrigger("Attack");
        }
    }
}

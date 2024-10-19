using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Ghoul : Enemy
{
    // Start is called before the first frame update
    [SerializeField] private float flipWaitTime;
    [SerializeField] private float ledgeCheckY;
    [SerializeField] private float ledgeCheckX;
    [SerializeField] private LayerMask whatIsGround;
    [SerializeField] private float chaseDistance;
    [Space(5)]

    [Header("HP")]
    [Space(5)]

    float timer;
    bool spottedPlayer = false;
    bool isAttacking = false;
    bool isIdle = true;
    Animator anim;
    protected override void Start()
    {
        base.Start();
        ChangeStates(EnemyStates.Ghoul_Idle);
        rb.gravityScale = 12f;
        anim = GetComponent<Animator>();
    }

    protected override void UpdateEnemyStates()
    {
        float _dist = Vector2.Distance(transform.position, PlayerController.Instance.transform.position);
        healthCheck();
        if (parried)
        {
            canMove = false;
        }
        switch (currentEnemyStates)
        {
            case EnemyStates.Ghoul_Idle:
                if (spottedPlayer) Flip();
                if (canMove)
                {
                    Vector3 _ledgeCheckStart = transform.localScale.x > 0 ? new Vector3(ledgeCheckX, 0) : new Vector3(-ledgeCheckX, 0);
                    Vector2 _wallCheckDir = transform.localScale.x > 0 ? transform.right : -transform.right;

                    if (!spottedPlayer)
                    {
                        if (!Physics2D.Raycast(transform.position + _ledgeCheckStart, Vector2.down, ledgeCheckY, whatIsGround) ||
                        Physics2D.Raycast(transform.position, _wallCheckDir, ledgeCheckX, whatIsGround))
                        {
                            ChangeStates(EnemyStates.Ghoul_Flip);
                        }
                        if (transform.localScale.x > 0)
                        {
                            rb.velocity = new Vector2(speed, rb.velocity.y);
                        }
                        else
                        {
                            rb.velocity = new Vector2(-speed, rb.velocity.y);
                        }
                    }                
                    if (_dist < chaseDistance)
                    {
                        Flip();
                        spottedPlayer = true;
                        isIdle = false;
                        canAttack = true;
                        ChangeStates(EnemyStates.Ghoul_Chase);
                    }

                }
                break;
            case EnemyStates.Ghoul_Chase:
                if (canAttack)
                {
                    Flip();
                    anim.SetBool("Walking", true);
                    isRecoiling = false;
                    transform.position = Vector2.MoveTowards
                    (transform.position, new Vector2(PlayerController.Instance.transform.position.x, transform.position.y),
                    speed * Time.deltaTime);
                    if (distanceCheck())
                    {
                        ChangeStates(EnemyStates.Ghoul_Attacking);
                    }

                }
                break;
            case EnemyStates.Ghoul_Attacking:
                attackanim();
                break;
            case EnemyStates.Ghoul_Flip:
                timer += Time.deltaTime;
                if (timer > flipWaitTime)
                {
                    timer = 0;
                    transform.localScale = new Vector2(transform.localScale.x * -1, transform.localScale.y);
                    ChangeStates(EnemyStates.Ghoul_Idle);
                }
                break;
            default:
                break;
        }
    }

    void healthCheck()
    {
        if (health <= 0)
        {
            anim.SetTrigger("Death");
            canAttack = false;
            canMove = false;
            anim.SetBool("Walking", false);
            Destroy(gameObject, 1f);
        }
    }
    bool distanceCheck()
    {
        float _dist = Vector2.Distance(PlayerController.Instance.transform.position, transform.position);
        return _dist < 1.8;
    }
    void Flip()
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
    void attackanim()
    {
        StartCoroutine(Attack1());
    }

    IEnumerator Attack1()
    {
        isAttacking = true;
        anim.SetTrigger("Attack");
        yield return new WaitForSeconds(0.5f);
        isAttacking = false;
        Flip();
        ChangeStates(EnemyStates.Ghoul_Idle);
    }
}

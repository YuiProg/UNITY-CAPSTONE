using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Crawler : Enemy
{
    float timer;
    [SerializeField] private float flipWaitTime;
    [SerializeField] private float ledgeCheckY;
    [SerializeField] private float ledgeCheckX;
    [SerializeField] private LayerMask whatIsGround;
    private bool thisCanMove = true;
    Animator anim;
    protected override void Start()
    {
        base.Start();
        rb.gravityScale = 12f;
        anim = GetComponent<Animator>();

    }

    protected override void UpdateEnemyStates()
    {
        
        if (health < maxHealth)
        {
            canAttack = true;
            anim.SetTrigger("Idle");
            if (health <= 0)
            {
                
                anim.SetTrigger("Dead");
                thisCanMove = false;
                Destroy(gameObject, 2f);

            }
            if (thisCanMove && canAttack)
            {
                switch (currentEnemyStates)
                {
                    case EnemyStates.Rat_Idle:
                        Vector3 _ledgeCheckStart = transform.localScale.x > 0 ? new Vector3(ledgeCheckX, 0) : new Vector3(-ledgeCheckX, 0);
                        Vector2 _wallCheckDir = transform.localScale.x > 0 ? transform.right : -transform.right;

                        if (!Physics2D.Raycast(transform.position + _ledgeCheckStart, Vector2.down, ledgeCheckY, whatIsGround) ||
                            Physics2D.Raycast(transform.position, _wallCheckDir, ledgeCheckX, whatIsGround))
                        {
                            ChangeStates(EnemyStates.Rat_Flip);
                        }

                        if (transform.localScale.x > 0)
                        {
                            rb.velocity = new Vector2(speed, rb.velocity.y);
                        }
                        else
                        {
                            rb.velocity = new Vector2(-speed, rb.velocity.y);
                        }
                        break;
                    case EnemyStates.Rat_Flip:
                        timer += Time.deltaTime;
                        if (timer > flipWaitTime)
                        {
                            timer = 0;
                            transform.localScale = new Vector2(transform.localScale.x * -1, transform.localScale.y);
                            ChangeStates(EnemyStates.Rat_Idle);
                        }
                        break;
                    default:
                        break;
                }
            }
        }
        
        
    }


}

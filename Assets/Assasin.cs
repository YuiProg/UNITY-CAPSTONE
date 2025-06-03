using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Assasin : Enemy
{
    public float chaseDistance;
    bool spottedPlayer = false;
    bool isAttacking = false;
    Animator anim;
    AudioManager audioManager;
    protected override void Start()
    {
        base.Start();
        canMove = true;
        canAttack = true;
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
        rb.gravityScale = 12f;
        anim = GetComponent<Animator>();
        ChangeStates(EnemyStates.A_Idle);
    }


    protected override void UpdateEnemyStates()
    {
        float distance = Vector2.Distance(PlayerController.Instance.transform.position, transform.position);
        stateChecker();
        flip(spottedPlayer && !isAttacking && health >= 0);
        if (canMove)
        {
            switch (currentEnemyStates)
            {               
                case EnemyStates.A_Idle:
                    if (distance < chaseDistance)
                    {
                        spottedPlayer = true;
                        ChangeStates(EnemyStates.A_Chase);
                    }
                    break;
                case EnemyStates.A_Chase:
                    if (distanceCheck())
                    {
                        
                        ChangeStates(EnemyStates.A_ATTACKBEHAVIOR);
                    }
                    anim.SetBool("Chase", true);
                    transform.position = Vector2.MoveTowards(transform.position, new Vector2(PlayerController.Instance.transform.position.x, transform.position.y), speed * Time.deltaTime);

                    break;
                case EnemyStates.A_ATTACKBEHAVIOR:
                    if (!isAttacking)
                    {
                        anim.SetBool("Chase", false);
                        StartCoroutine(Attack1());
                    }
                    break;
                default:
                    break;
            }
        }
    }
    public void attackSound()
    {
        audioManager.PlaySFX(audioManager.HardAttack);
    }
    void stateChecker()
    {
        canAttack = !parried;
        canMove = !parried;
        if (parried) anim.SetTrigger("Parried");
        if (health <= 0)
        {
            canMove = false;
            canAttack = false;
            anim.SetTrigger("Die");
            Destroy(gameObject, 2.5f);
        }
    }
    IEnumerator Attack1()
    {
        isAttacking = true;
        canMove = false;
        anim.SetTrigger("Attack");
        yield return new WaitForSeconds(1.5f);
        isAttacking = false;
        canMove = true;
        ChangeStates(EnemyStates.A_Idle);
    }
    public bool distanceCheck()
    {
        float distance = Vector2.Distance(PlayerController.Instance.transform.position, transform.position);
        return distance < 2.5f;
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
                healthBar.transform.eulerAngles = new Vector3(0, 0 ,0);
            }
        }
    }
}

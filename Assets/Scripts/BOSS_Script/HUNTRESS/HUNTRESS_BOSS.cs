using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUNTRESS_BOSS : Enemy
{
    [SerializeField] float chaseDistance;
    [SerializeField] GameObject hitBOX;
    [SerializeField] Text Status;
    Vector2 spawnpoint;
    Animator anim;
    bool spottedPlayer = false;

    protected override void Start()
    {
        base.Start();
        rb.gravityScale = 12f;
        canAttack = true;
        anim = GetComponent<Animator>();
        spawnpoint = transform.position;
        ChangeStates(EnemyStates.HT_Idle);
    }

    protected override void UpdateEnemyStates()
    {
        flip();
        timer += Time.deltaTime;
        float _dist = Vector2.Distance(transform.position, PlayerController.Instance.transform.position);
        if (parried)
        {
            canMove = false;
            Status.text = "! PARRIED !";
            StopCoroutine(AttackHT());
        }
        else
        {
            canMove = true;
            Status.text = " ";
        }
        if (health <= 0)
        {
            canMove = false;
            canAttack = false;
            anim.SetTrigger("Death");
            Destroy(gameObject, 1f);
        }
        if (!PlayerController.Instance.pState.isAlive)
        {
            transform.position = spawnpoint;
            health = maxHealth;
            parrypercent = parrymax;
            spottedPlayer = false;
            ChangeStates(EnemyStates.HT_Idle);
        }
        switch (currentEnemyStates)
        {
            case EnemyStates.HT_Idle:
                if (_dist < chaseDistance)
                {
                    spottedPlayer = true;
                    ChangeStates(EnemyStates.HT_Chase);
                }
                break;
            case EnemyStates.HT_Chase:
                Walk();
                break;
            default:
                break;
        }
    }

    void flip()
    {
        if (PlayerController.Instance.transform.position.x > transform.position.x)
        {
            transform.eulerAngles = new Vector3(0, 180, 0);
            healthBar.transform.eulerAngles = new Vector3(0,0,0);
            Status.transform.eulerAngles = new Vector3(0, 0, 0);
        }
        else
        {
            transform.eulerAngles = new Vector3(0, 0, 0);
            healthBar.transform.eulerAngles = new Vector3(0, 0, 0);
            Status.transform.eulerAngles = new Vector3(0, 0, 0);
        }
    }

    IEnumerator AttackHT()
    {
        timer = 0;
        canMove = false;
        anim.SetBool("Attack", true);
        yield return new WaitForSeconds(4.5f);
        canMove = true;
        ChangeStates(EnemyStates.HT_Idle);
    }

    float timer;
    void Walk()
    {
        if (spottedPlayer && canMove)
        {
            if (canMove && Vector2.Distance(transform.position, PlayerController.Instance.transform.position) <= 2f)
            {
                
                StartCoroutine(AttackHT());
            }
            else if (canMove && Vector2.Distance(transform.position, PlayerController.Instance.transform.position) >= 2f)
            {
                StopCoroutine(AttackHT());
                anim.SetBool("Attack", false);
                if (timer > 1f)
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

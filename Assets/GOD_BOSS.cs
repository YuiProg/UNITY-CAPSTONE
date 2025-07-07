using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GOD_BOSS : Enemy
{
    bool spottedPlayer = false;
    public float chaseDistance;
    public bool isAttacking = false;
    bool isSecondPhase = false;
    Animator anim;
    AudioManager audioManager;
    protected override void Start()
    {
        base.Start();
        anim = GetComponent<Animator>();
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
        ChangeStates(EnemyStates.G_IDLE);
    }

    protected override void UpdateEnemyStates()
    {
        float dist = Vector2.Distance(transform.position, PlayerController.Instance.transform.position);
        flip(!isAttacking);
        canMove = !isAttacking;
        if (canMove && PlayerController.Instance.pState.isAlive)
        {
            switch (currentEnemyStates)
            {
                case EnemyStates.G_IDLE:
                    if (dist < chaseDistance)
                    {
                        spottedPlayer = true;
                        if (!isSecondPhase)
                        {
                            ChangeStates(EnemyStates.G_CHASE);
                        }
                    }
                    break;
                case EnemyStates.G_CHASE:
                    if (distanceCheck())
                    {
                        Debug.Log("close to player");
                        ChangeStates(EnemyStates.G_ATTACKBEHAVIOR);
                    }
                    else
                    {
                        anim.SetBool("G_RUN", true);
                        transform.position = Vector2.MoveTowards(transform.position, new Vector2(PlayerController.Instance.transform.position.x, transform.position.y), speed * Time.deltaTime);
                    }
                    break;
                case EnemyStates.G_ATTACKBEHAVIOR:
                    if (!isAttacking)
                    {
                        phase1AttackPattern();
                    }
                    break;
                case EnemyStates.G_TRANSFORM:
                    break;
                case EnemyStates.E_CHASE:
                    break;
                case EnemyStates.E_ATTACKBEHAVIOR:
                    break;
                default:
                    break;
            }
        }
        
    }

    void phase1AttackPattern ()
    {
        anim.SetBool("G_RUN", false);
        int random = Random.Range(0, 5);

        switch (random)
        {
            case 0:
                StartCoroutine(G_ATTACK1());
                break;
            case 1:
                StartCoroutine(G_ATTACK2());
                break;
            case 2:
                StartCoroutine(G_ATTACK3());
                break;
        }
    }

    //phase 1 attacks
    IEnumerator G_ATTACK1 ()
    {
        isAttacking = true;
        anim.SetTrigger("G_ATK_1");
        yield return new WaitForSeconds(.5f);
        isAttacking = false;
        ChangeStates(EnemyStates.G_IDLE);
    }

    IEnumerator G_ATTACK2 ()
    {
        Debug.Log("perform attack");
        isAttacking = true;
        anim.SetTrigger("G_ATK_2");
        yield return new WaitForSeconds(1f);
        isAttacking = false;
        ChangeStates(EnemyStates.G_IDLE);
    }

    IEnumerator G_ATTACK3 ()
    {
        Debug.Log("perform attack");
        isAttacking = true;
        anim.SetTrigger("G_ATK_3");
        yield return new WaitForSeconds(2f);
        isAttacking = false;
        ChangeStates(EnemyStates.G_IDLE);
    }

    bool distanceCheck()
    {
        float dist = Vector2.Distance(transform.position, PlayerController.Instance.transform.position);
        return dist < 5f;
    }

    void flip(bool canFlip)
    {
        if (canFlip)
        {
            if (PlayerController.Instance.transform.position.x < transform.position.x)
            {
                transform.eulerAngles = new Vector3(0, 180, 0);
            }
            else
            {
                transform.eulerAngles = new Vector3(0, 0, 0);
            }
        }
    }
}

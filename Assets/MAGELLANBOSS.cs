using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MAGELLANBOSS : Enemy
{
    public float chaseDistance;
    public bool isSecondPhase;
    private int secondPhaseCount;
    public bool spottedPlayer;

    bool isAttacking = false;
    bool hasTransformed = false;
    Animator anim;

    //bars
    [SerializeField] GameObject HEALTHBAR;
    [SerializeField] GameObject BORDERR;
    [SerializeField] GameObject BORDERL;
    protected override void Start()
    {
        base.Start();
        canMove = true;
        canAttack = true;
        isSecondPhase = false;
        spottedPlayer = false;
        rb.gravityScale = 12f;
        anim = GetComponent<Animator>();
        ChangeStates(EnemyStates.MG_IDLE);

    }

    protected override void UpdateEnemyStates()
    {
        float dist = Vector2.Distance(transform.position, PlayerController.Instance.transform.position);
        stateCheck();
        flip(!isAttacking);
        if (canMove && PlayerController.Instance.pState.isAlive)
        {
            switch (currentEnemyStates)
            {
                case EnemyStates.MG_IDLE:
                    if (dist < chaseDistance)
                    {
                        spottedPlayer = true;
                        if (health > health / 2)
                        {
                            ChangeStates(EnemyStates.MG_CHASE);
                        }
                        else
                        {
                            isSecondPhase = true;
                            ChangeStates(EnemyStates.MG_TRANSFORM);
                        }
                    }
                    break;
                case EnemyStates.MG_CHASE:
                    if (distanceCheck())
                    {                      
                        ChangeStates(EnemyStates.MG_ATTACKBEHAVIOR);
                    }
                    anim.SetBool("MF_RUN", true);
                    transform.position = Vector2.MoveTowards(transform.position, 
                        new Vector2(PlayerController.Instance.transform.position.x, transform.position.y), speed * Time.deltaTime);

                    break;
                case EnemyStates.MG_ATTACKBEHAVIOR:
                    if (!isAttacking)
                    {
                        FirstPhaseAttackPattern();
                    }
                    break;
                case EnemyStates.MG_TRANSFORM:
                    if (!hasTransformed)
                    {
                        hasTransformed = true;
                        StartCoroutine(Transform(2f));
                    }
                    else
                    {
                        ChangeStates(EnemyStates.MG_E_IDLE);
                    }
                    break;
                case EnemyStates.MG_E_IDLE:
                    if (dist < chaseDistance)
                    {
                        ChangeStates(EnemyStates.MG_E_CHASE);
                    }
                    break;
                case EnemyStates.MG_E_CHASE:
                    break;
                case EnemyStates.MG_E_ATTACKBEHAVIOR:
                    break;
                default:
                    break;
            }
        }
    }

    void stateCheck()
    {
        canMove = !parried;
        BORDERL.SetActive(spottedPlayer && health != 0);
        BORDERR.SetActive(spottedPlayer && health != 0);
        HEALTHBAR.SetActive(spottedPlayer && health != 0);
        //if (health <= 0)
        //{
        //    secondPhaseCount++;
        //}
    }

    //transformation
    IEnumerator Transform(float time)
    {
        canMove = false;
        anim.SetTrigger("Transform");
        yield return new WaitForSeconds(time);
        canMove = true;
        ChangeStates(EnemyStates.MG_E_IDLE);
    }

    //first phase attacks

    void FirstPhaseAttackPattern()
    {
        int i = Random.Range(0, 4);

        switch (i)
        {
            case 0:
                StartCoroutine(MF_Attack1());
                break;
            case 1:
                StartCoroutine(MF_Attack2());
                break;
            case 2:
                StartCoroutine(MF_Attack3());
                break;
            case 3:
                StartCoroutine(MF_Attack4());
                break;
            default:
                break;
        }
    }

    IEnumerator MF_Attack1()
    {
        anim.SetBool("MF_RUN", false);
        isAttacking = true;
        canMove = false;
        anim.SetTrigger("MF_Attack1");
        yield return new WaitForSeconds(1.5f);
        isAttacking = false;
        canMove = true;
        ChangeStates(EnemyStates.MG_IDLE);
    }

    IEnumerator MF_Attack2()
    {
        anim.SetBool("MF_RUN", false);
        isAttacking = true;
        canMove = false;
        anim.SetTrigger("MF_Attack2");
        yield return new WaitForSeconds(2.5f);
        isAttacking = false;
        canMove = true;
        ChangeStates(EnemyStates.MG_IDLE);
    }

    IEnumerator MF_Attack3()
    {
        anim.SetBool("MF_RUN", false);
        isAttacking = true;
        canMove = false;
        anim.SetTrigger("MF_Attack3");
        yield return new WaitForSeconds(2.5f);
        isAttacking = false;
        canMove = true;
        ChangeStates(EnemyStates.MG_IDLE);
    }

    IEnumerator MF_Attack4()
    {
        anim.SetBool("MF_RUN", false);
        isAttacking = true;
        canMove = false;
        anim.SetTrigger("MF_Attack4");
        yield return new WaitForSeconds(2.7f);
        isAttacking = false;
        canMove = true;
        ChangeStates(EnemyStates.MG_IDLE);
    }

    public bool distanceCheck()
    {
        float dist = Vector2.Distance(transform.position, PlayerController.Instance.transform.position);
        return dist < 7.5f;
    }

    void flip(bool canflip)
    {
        if (canflip)
        {
            if (PlayerController.Instance.transform.position.x < transform.position.x)
            {
                transform.eulerAngles = new Vector3(0, 180, 0);
                HEALTHBAR.transform.eulerAngles = new Vector3(0, 0, 0);
            }
            else
            {
                transform.eulerAngles = new Vector3(0, 0, 0);
                HEALTHBAR.transform.eulerAngles = new Vector3(0, 0, 0);
            }
        }
    }
}

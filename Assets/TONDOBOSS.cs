using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TONDOBOSS : Enemy
{
    public float chaseDistance;
    public bool spottedPlayer = false;
    bool isAttacking = false;
    bool isSecondPhase = false;
    bool hasTransform = false;
    [SerializeField] GameObject HEALTBAR;
    [SerializeField] GameObject Amber;
    [SerializeField] Transform amberLOC;
    [SerializeField] GameObject BORDERR;
    [SerializeField] GameObject BORDERL;
    Animator anim;
    AudioManager audiomanager;
    protected override void Start()
    {
        base.Start();
        canMove = true;
        canAttack = true;
        rb.gravityScale = 12f;
        anim = GetComponent<Animator>();
        audiomanager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
        ChangeStates(EnemyStates.TB_IDLE);
    }

    protected override void UpdateEnemyStates()
    {
        float distance = Vector2.Distance(transform.position, PlayerController.Instance.transform.position);
        stateCheck();
        flip(!isAttacking && health >= 0);
        isSecondPhase = health <= maxHealth / 2;
        if (canMove && PlayerController.Instance.pState.isAlive)
        {
            switch (currentEnemyStates)
            {
                case EnemyStates.TB_IDLE:
                    if (distance < chaseDistance)
                    {
                        spottedPlayer = true;
                        if (!isSecondPhase)
                        {
                            ChangeStates(EnemyStates.TB_CHASE);
                        }
                    }
                    break;
                case EnemyStates.TB_CHASE:

                    if (distanceCheck())
                    {
                        ChangeStates(EnemyStates.TB_ATTACKBEHAVIOR);
                    }
                    anim.SetBool("TB_CHASE", true);
                    transform.position = Vector2.MoveTowards(transform.position, 
                        new Vector2(PlayerController.Instance.transform.position.x, transform.position.y), speed * Time.deltaTime);

                    break;
                case EnemyStates.TB_ATTACKBEHAVIOR:
                    if (!isAttacking)
                    {
                        Phase1AttackPattern();
                    }
                    break;
                case EnemyStates.TB_TRANSFORM:
                    if (!hasTransform)
                    {
                        hasTransform = true;
                        StartCoroutine(Transform(3f));
                    }
                    break;
                case EnemyStates.TB_E_IDLE:
                    if (distance < chaseDistance)
                    {
                        ChangeStates(EnemyStates.TB_E_CHASE);
                    }
                    break;
                case EnemyStates.TB_E_CHASE:
                    if (distanceCheck())
                    {
                        ChangeStates(EnemyStates.TB_E_ATTACKBEHAVIOR);
                    }
                    anim.SetBool("TB_T_CHASE", true);
                    transform.position = Vector2.MoveTowards(transform.position, new Vector2(PlayerController.Instance.transform.position.x, transform.position.y), speed * Time.deltaTime);
                    break;
                case EnemyStates.TB_E_ATTACKBEHAVIOR:
                    if (!isAttacking)
                    {
                        Phase2AttackPattern();
                    }
                    break;
                default:
                    break;
            }
        }
    }
    void stateCheck()
    {
        canMove = !parried || health >= 0;
        canAttack = !parried || health >= 0;
        BORDERL.SetActive(spottedPlayer && health >= 0);
        BORDERR.SetActive(spottedPlayer && health >= 0);
        HEALTBAR.SetActive(spottedPlayer && PlayerController.Instance.pState.isAlive && health >= 0);
        if (health <= 0)
        {

        }

    }
    IEnumerator Transform(float time)
    {
        canMove = false;
        canAttack = false;
        anim.SetTrigger("Transform");
        yield return new WaitForSeconds(time);
        canMove = true;
        canAttack = true;
        ChangeStates(EnemyStates.TB_E_CHASE);
    }
    void Phase1AttackPattern()
    {
        int i = Random.Range(0, 4);
        switch (i)
        {
            case 0:
                StartCoroutine(TB_Attack1());
                break;
            case 1:
                StartCoroutine(TB_Attack2());
                break;
            case 2:
                StartCoroutine(TB_Skill1());
                break;
            case 3:
                StartCoroutine(TB_Skill2());
                break;
            default:
                break;
        }
    }

    void Phase2AttackPattern()
    {
        int i = Random.Range(0, 4);

        switch (i)
        {
            case 0:
                StartCoroutine(TB_T_Attack1());
                break;
            case 1:
                StartCoroutine(TB_T_Attack2());
                break;
            case 2:
                StartCoroutine(TB_T_Skill1());
                break;
            case 3:
                StartCoroutine(TB_T_Skill2());
                break;
            default:
                break;
        }
    }

    //phase 1 attacks pare

    IEnumerator TB_Attack1()
    {
        anim.SetBool("TB_CHASE", false);
        isAttacking = true;
        canMove = false;
        anim.SetTrigger("TB_ATTACK1");
        yield return new WaitForSeconds(1f);
        isAttacking = false;
        canMove = true;
        ChangeStates(EnemyStates.TB_IDLE);
    }
    IEnumerator TB_Attack2()
    {
        anim.SetBool("TB_CHASE", false);
        isAttacking = true;
        canMove = false;
        anim.SetTrigger("TB_ATTACK2");
        yield return new WaitForSeconds(1f);
        isAttacking = false;
        canMove = true;
        ChangeStates(EnemyStates.TB_IDLE);
    }

    IEnumerator TB_Skill1()
    {
        anim.SetBool("TB_CHASE", false);
        isAttacking = true;
        canMove = false;
        anim.SetTrigger("TB_SKILL1");
        yield return new WaitForSeconds(2f);
        isAttacking = false;
        canMove = true;
        ChangeStates(EnemyStates.TB_IDLE);
    }

    IEnumerator TB_Skill2()
    {
        anim.SetBool("TB_CHASE", false);
        isAttacking = true;
        canMove = false;
        anim.SetTrigger("TB_SKILL2");
        yield return new WaitForSeconds(2f);
        isAttacking = false;
        canMove = true;
        ChangeStates(EnemyStates.TB_IDLE);
    }

    //phase 2 attacks
    IEnumerator TB_T_Attack1()
    {
        anim.SetBool("TB_T_CHASE", false);
        isAttacking = true;
        canMove = false;
        anim.SetTrigger("TB_T_ATTACK1");
        yield return new WaitForSeconds(1f);
        isAttacking = false;
        canMove = true;
        ChangeStates(EnemyStates.TB_E_CHASE);
    }
    IEnumerator TB_T_Attack2()
    {
        anim.SetBool("TB_T_CHASE", false);
        isAttacking = true;
        canMove = false;
        anim.SetTrigger("TB_T_ATTACK2");
        yield return new WaitForSeconds(1f);
        isAttacking = false;
        canMove = true;
        ChangeStates(EnemyStates.TB_E_CHASE);
    }
    IEnumerator TB_T_Skill1()
    {
        anim.SetBool("TB_T_CHASE", false);
        isAttacking = true;
        canMove = false;
        anim.SetTrigger("TB_T_SKILL1");
        yield return new WaitForSeconds(1f);
        isAttacking = false;
        canMove = true;
        ChangeStates(EnemyStates.TB_E_CHASE);
    }
    IEnumerator TB_T_Skill2()
    {
        anim.SetBool("TB_T_CHASE", false);
        isAttacking = true;
        canMove = false;
        anim.SetTrigger("TB_T_SKILL2");
        yield return new WaitForSeconds(1f);
        isAttacking = false;
        canMove = true;
        ChangeStates(EnemyStates.TB_E_CHASE);
    }

    bool distanceCheck()
    {
        float distance = Vector2.Distance(transform.position, PlayerController.Instance.transform.position);
        return distance < 5.5f;
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

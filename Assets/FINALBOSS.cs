using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FINALBOSS : Enemy
{
    [SerializeField] GameObject HEALTHBAR;

    Animator anim;
    public bool spottedPlayer;
    public float chaseDistance;
    bool isSecondPhase = false;
    bool isAttacking = false;
    bool hasTransformed = false;
    AudioManager audioManager;
    protected override void Start()
    {
        base.Start();
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
        canMove = true;
        canAttack = true;
        anim = GetComponent<Animator>();
        rb.gravityScale = 12f;
        ChangeStates(EnemyStates.FB_IDLE);
    }

    protected override void UpdateEnemyStates()
    {
        isSecondPhase = health <= maxHealth / 2;
        flip(!isAttacking);
        stateCheck();
        if (isSecondPhase && !hasTransformed)
        {
            hasTransformed = true;
            StartCoroutine(Transform(5f));
        }
        float dist = Vector2.Distance(transform.position, PlayerController.Instance.transform.position);
        if (canMove && PlayerController.Instance.pState.isAlive)
        {
            switch (currentEnemyStates)
            {
                case EnemyStates.FB_IDLE:
                    if (dist < chaseDistance)
                    {
                        spottedPlayer = true;
                        if (!isSecondPhase)
                        {
                            ChangeStates(EnemyStates.FB_CHASE);
                        }                   
                    }
                    break;
                case EnemyStates.FB_CHASE:
                    if (distanceCheck())
                    {
                        ChangeStates(EnemyStates.FB_ATTACKBEHAVIOR);
                    }
                    anim.SetBool("FB_CHASE", true);
                    transform.position = Vector2.MoveTowards(transform.position, 
                        new Vector2(PlayerController.Instance.transform.position.x, transform.position.y), speed * Time.deltaTime);
                    break;
                case EnemyStates.FB_ATTACKBEHAVIOR:
                    if (!isAttacking)
                    {
                        Phase1AttackPattern();
                    }
                    break;
                case EnemyStates.FB_TRANSFORM:
                    if (!hasTransformed)
                    {
                        hasTransformed = true;
                        StartCoroutine(Transform(3f));
                    }
                    else
                    {
                        ChangeStates(EnemyStates.FB_E_IDLE);
                    }
                    break;
                case EnemyStates.FB_E_IDLE:
                    anim.Play("FB_A_IDLE");
                    ChangeStates(EnemyStates.FB_E_CHASE);
                    break;
                case EnemyStates.FB_E_CHASE:
                    if (distanceCheck())
                    {
                        ChangeStates(EnemyStates.FB_E_ATTACKBEHAVIOR);
                    }
                    
                    anim.SetBool("FB_A_CHASE", true);
                    transform.position = Vector2.MoveTowards(transform.position,
                        new Vector2(PlayerController.Instance.transform.position.x, transform.position.y), speed * Time.deltaTime);
                    break;
                case EnemyStates.FB_E_ATTACKBEHAVIOR:
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
    IEnumerator Transform(float time)
    {
        canMove = false;
        isAttacking = true;
        anim.SetTrigger("Transform");
        audioManager.PlaySFX(audioManager.Transform);
        yield return new WaitForSeconds(time);
        canMove = true;
        isAttacking = false;
        ChangeStates(EnemyStates.FB_E_CHASE);
    }
    void stateCheck()
    {
        canMove = !parried;
        canAttack = !canAttack;
        HEALTHBAR.SetActive(spottedPlayer && health >= 0 && PlayerController.Instance.pState.isAlive);
    }
    void Phase1AttackPattern()
    {
        int i = Random.Range(0, 4);
        switch (i)
        {
            case 0:
                StartCoroutine(FB_Attack1());
                break;
            case 1:
                StartCoroutine(FB_Attack2());
                break;
            case 2:
                StartCoroutine(FB_Skill1());
                break;
            case 3:
                StartCoroutine(FB_Skill2());
                break;
            default:
                break;
        }
    }

    void Phase2AttackPattern()
    {
        int i = Random.Range(0,4);
        switch (i)
        {
            case 0:
                StartCoroutine(FB_E_Attack1());
                break;
            case 1:
                StartCoroutine(FB_E_Attack2());
                break;
            case 2:
                StartCoroutine(FB_E_Attack3());
                break;
            case 3:
                StartCoroutine(FB_E_Attack4());
                break;
            default:
                break;
        }
    }
    //sounds
    void attackSound()
    {
        audioManager.PlaySFX(audioManager.FB_Attack);
    }

    void thundersound()
    {
        audioManager.PlaySFX(audioManager.FB_Thunder);
    }
    //phase 1 attacks


    IEnumerator FB_Attack1()
    {
        anim.SetBool("FB_CHASE", false);
        isAttacking = true;
        canMove = false;
        anim.SetTrigger("FB_Attack1");
        yield return new WaitForSeconds(1f);
        isAttacking = false;
        canMove = true;
        ChangeStates(EnemyStates.FB_IDLE);
    }
    IEnumerator FB_Attack2()
    {
        anim.SetBool("FB_CHASE", false);
        isAttacking = true;
        canMove = false;
        anim.SetTrigger("FB_Attack2");
        yield return new WaitForSeconds(1f);
        isAttacking = false;
        canMove = true;
        ChangeStates(EnemyStates.FB_IDLE);
    }
    IEnumerator FB_Skill1()
    {
        anim.SetBool("FB_CHASE", false);
        isAttacking = true;
        canMove = false;
        anim.SetTrigger("FB_Skill1");
        yield return new WaitForSeconds(2f);
        isAttacking = false;
        canMove = true;
        ChangeStates(EnemyStates.FB_IDLE);
    }
    IEnumerator FB_Skill2()
    {
        anim.SetBool("FB_CHASE", false);
        isAttacking = true;
        canMove = false;
        anim.SetTrigger("FB_Skill2");
        yield return new WaitForSeconds(4f);
        isAttacking = false;
        canMove = true;
        ChangeStates(EnemyStates.FB_IDLE);
    }

    //phase 2 attacks pare


    IEnumerator FB_E_Attack1()
    {
        anim.SetBool("FB_A_CHASE", false);
        isAttacking = true;
        canMove = false;
        anim.SetTrigger("FB_A_Attack1");
        yield return new WaitForSeconds(1f);
        isAttacking = false;
        canMove = true;
        ChangeStates(EnemyStates.FB_E_CHASE);
    }
    IEnumerator FB_E_Attack2()
    {
        anim.SetBool("FB_A_CHASE", false);
        isAttacking = true;
        canMove = false;
        anim.SetTrigger("FB_A_Attack2");
        yield return new WaitForSeconds(1f);
        isAttacking = false;
        canMove = true;
        ChangeStates(EnemyStates.FB_E_CHASE);
    }
    IEnumerator FB_E_Attack3()
    {
        anim.SetBool("FB_A_CHASE", false);
        isAttacking = true;
        canMove = false;
        anim.SetTrigger("FB_A_Attack3");
        yield return new WaitForSeconds(2f);
        isAttacking = false;
        canMove = true;
        ChangeStates(EnemyStates.FB_E_CHASE);
    }
    IEnumerator FB_E_Attack4()
    {
        anim.SetBool("FB_A_CHASE", false);
        isAttacking = true;
        canMove = false;
        anim.SetTrigger("FB_A_Attack4");
        yield return new WaitForSeconds(3f);
        isAttacking = false;
        canMove = true;
        ChangeStates(EnemyStates.FB_E_CHASE);
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

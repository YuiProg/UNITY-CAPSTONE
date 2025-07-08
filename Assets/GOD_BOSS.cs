using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class GOD_BOSS : Enemy
{
    bool spottedPlayer = false;
    public float chaseDistance;
    public bool isAttacking = false;
    bool isSecondPhase = false;
    bool hasTransformed = false;
    Animator anim;
    AudioManager audioManager;

    [SerializeField] GameObject HPBAR;
    [SerializeField] GameObject Projectile1;
    [SerializeField] GameObject Projectile2;
    [SerializeField] GameObject Projectile3;
    [SerializeField] GameObject QuickProjectile;

    //projectile pos
    [SerializeField] Transform proj_pos1;
    [SerializeField] Transform proj_pos2;
    [SerializeField] Transform proj_pos3;
    protected override void Start()
    {
        base.Start();
        anim = GetComponent<Animator>();
        rb.gravityScale = 12f;
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
        ChangeStates(EnemyStates.G_IDLE);
    }

    protected override void UpdateEnemyStates()
    {
        float dist = Vector2.Distance(transform.position, PlayerController.Instance.transform.position);
        HPBAR.SetActive(spottedPlayer && health > 0);
        flip(!isAttacking && health > 0);
        canMove = !isAttacking && !parried;
        isSecondPhase = health <= maxHealth / 2;
        canAttack = !parried;
        anim.SetBool("G_DEFEND", parried);
        if (health <= 0)
        {
            spottedPlayer = false;
            canAttack = false;
            canMove = false;
            anim.Play("DEATH");
        }
        if (parried && !isSecondPhase)
        {
            int shield_chance = Random.Range(0,3);
            if (shield_chance == 2) StartCoroutine(Defend());
        } 
        if (isSecondPhase && !hasTransformed)
        {
            hasTransformed = true;
            StartCoroutine(TRANSFORM());
        }
        if (canAttack && canMove && PlayerController.Instance.pState.isAlive && !isRecoiling)
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
                    if (!isAttacking && !parried)
                    {
                        phase1AttackPattern();
                    }
                    break;
                case EnemyStates.G_TRANSFORM:
                    break;
                case EnemyStates.E_CHASE:
                    if (distanceCheck() && !parried)
                    {
                        phase2AttackPatterns();
                    } else if (distanceCheck() && PlayerController.Instance.pState.jumping)
                    {
                        StartCoroutine(G_ATTACK4());
                    } else
                    {
                        anim.SetBool("E_CHASE", true);
                        transform.position = Vector2.MoveTowards(transform.position, new Vector2(PlayerController.Instance.transform.position.x, transform.position.y), speed * Time.deltaTime);
                    }
                    break;
                case EnemyStates.E_ATTACKBEHAVIOR:
                    break;
                default:
                    break;
            }
        }
        
    }
    public void cameraShake()
    {
        CameraShake.Instance.ShakeCamera();
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
            case 3:
                StartCoroutine(G_SKILL1());
                break;
            case 4:
                StartCoroutine(G_SKILL2());
                break;
        }
    }

    void phase2AttackPatterns ()
    {
        anim.SetBool("E_CHASE", false);
        int random = Random.Range(0,5);
        switch (random)
        {
            case 0:
                StartCoroutine(E_ATTACK1());
                break;
            case 1:
                StartCoroutine(E_ATTACK2());
                break;
            case 2:
                StartCoroutine(E_ATTACK3());
                break;
            case 3:
                StartCoroutine(E_ATTACK4());
                break;
            case 4:
                StartCoroutine(E_SKILL1());
                break;
        }
    }

    //phase 1 attacks
    IEnumerator Defend ()
    {
        canBeDamaged = false;
        isAttacking = true;
        yield return new WaitForSeconds(2f);
        isAttacking = false;
        canBeDamaged = true;
        ChangeStates(EnemyStates.G_IDLE);
    }
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
        isAttacking = true;
        anim.SetTrigger("G_ATK_2");
        yield return new WaitForSeconds(1f);
        isAttacking = false;
        ChangeStates(EnemyStates.G_IDLE);
    }

    IEnumerator G_ATTACK3 ()
    {
        isAttacking = true;
        anim.SetTrigger("G_ATK_3");
        yield return new WaitForSeconds(2f);
        isAttacking = false;
        ChangeStates(EnemyStates.G_IDLE);
    }

    IEnumerator G_ATTACK4 ()
    {
        isAttacking = true;
        anim.SetTrigger("G_ATK_4");
        yield return new WaitForSeconds(1f);
        isAttacking = false;
        ChangeStates(EnemyStates.G_IDLE);
    }

    IEnumerator G_SKILL1 ()
    {
        isAttacking = true;
        anim.SetTrigger("G_SKILL_1");
        yield return new WaitForSeconds(2.5f);
        isAttacking = false;
        ChangeStates(EnemyStates.G_IDLE);
    }

    IEnumerator G_SKILL2()
    {
        isAttacking = true;
        anim.SetTrigger("G_SKILL_2");
        yield return new WaitForSeconds(2.5f);
        isAttacking = false;
        ChangeStates(EnemyStates.G_IDLE);
    }

    IEnumerator TRANSFORM ()
    {
        canMove = false;
        canAttack = false;
        anim.SetTrigger("Transform");
        yield return new WaitForSeconds(3f);
        canMove = true;
        canAttack = true;
        ChangeStates(EnemyStates.E_CHASE);
    }

    public void Release_Projectile ()
    {
        Instantiate(QuickProjectile, proj_pos2);
    }

    //phase 2 attacks

    IEnumerator E_ATTACK1 ()
    {
        isAttacking = true;
        anim.SetTrigger("E_ATK_1");
        yield return new WaitForSeconds(1f);
        isAttacking = false;
        ChangeStates(EnemyStates.E_CHASE);
    }

    IEnumerator E_ATTACK2 ()
    {
        isAttacking = true;
        anim.SetTrigger("E_ATK_2");
        yield return new WaitForSeconds(1f);
        isAttacking = false;
        ChangeStates(EnemyStates.E_CHASE);
    }

    IEnumerator E_ATTACK3()
    {
        isAttacking = true;
        anim.SetTrigger("E_ATK_3");
        yield return new WaitForSeconds(1.5f);
        isAttacking = false;
        ChangeStates(EnemyStates.E_CHASE);
    }

    IEnumerator E_ATTACK4()
    {
        isAttacking = true;
        anim.SetTrigger("E_ATK_4");
        yield return new WaitForSeconds(1f);
        isAttacking = false;
        ChangeStates(EnemyStates.E_CHASE);
    }

    IEnumerator E_SKILL1()
    {
        isAttacking = true;
        anim.SetTrigger("E_SKILL_1");
        yield return new WaitForSeconds(1f);
        Instantiate(Projectile1, proj_pos1);
        Instantiate(Projectile2, proj_pos2);
        Instantiate(Projectile3, proj_pos3);
        yield return new WaitForSeconds(1f);
        isAttacking = false;
        ChangeStates(EnemyStates.E_CHASE);
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

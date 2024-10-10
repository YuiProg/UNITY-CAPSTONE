using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpearGirl : Enemy
{
    Animator anim;
    public bool spottedPlayer = false;
    [SerializeField] float chaseDistance;
    [SerializeField] GameObject HealthBar;
    [SerializeField] GameObject Music;
    [SerializeField] public GameObject bullet;
    [SerializeField] public Transform bulletpos;
    [SerializeField] GameObject borderR;
    [SerializeField] GameObject borderL;
    bool attacking;
    float projectileTimer;
    public float verticalBounce;
    float transitionTime;
    bool isDead = false;
    public static SpearGirl instance;
    protected override void Start()
    {
        base.Start();
        anim = GetComponent<Animator>();
        rb.gravityScale = 12f;
        canMove = true;
        canAttack = true;
        attacking = false;
        Music.SetActive(false);
        HealthBar.SetActive(false);
        borderL.SetActive(false);
        borderR.SetActive(false);
        ChangeStates(EnemyStates.SG_Idle);
    }
    private void Awake()
    {
        loadCheck();
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
        }
    }

    void loadCheck()
    {
        int SG = PlayerPrefs.GetInt("SpearGirl");
        if (SG == 1)
        {
            HealthBar.SetActive(false);
            gameObject.SetActive(false);
            borderL.SetActive(false);
            borderR.SetActive(false);
        }
    }

    protected override void UpdateEnemyStates()
    {
        float _dist = Vector2.Distance(transform.position, PlayerController.Instance.transform.position);
        healthCheck();
        if (!attacking && spottedPlayer && !isDead)
        {
            flip();
        }
        
        if (parried)
        {
            stopAllAttacks();
            canMove = false;
            anim.SetBool("PARRIED", true);
        }
        else
        {
            anim.SetBool("PARRIED", false);
            canMove = true;
        }
        if (health <= 0)
        {
            PlayerPrefs.SetInt("SpearGirl", 1);
            isDead = true;
            canMove = false;
            canAttack = false;
        }
        if (canMove && !attacking && !isDead)
        {
            switch (currentEnemyStates)
            {
                case EnemyStates.SG_Idle:
                    if (_dist < chaseDistance)
                    {
                        HealthBar.SetActive(true);
                        Music.SetActive(true);
                        borderL.SetActive(true);
                        borderR.SetActive(true);
                        spottedPlayer = true;
                        if (health < maxHealth / 2)
                        {
                            Debug.Log("PHASE 2");
                            transitionTime += Time.deltaTime;
                            canMove = false;
                            canAttack = false;
                            anim.SetBool("Transition", true);
                            if (transitionTime > 2)
                            {                               
                                anim.SetBool("Transition", false);
                                canAttack = true;
                                canMove = true;
                                ChangeStates(EnemyStates.SG_Phase2);
                            }
                            
                        }
                        else
                        {
                            Debug.Log("PHASE 1");
                            ChangeStates(EnemyStates.SG_Phase1);
                        }                      
                    }
                    break;
                case EnemyStates.SG_Phase1:
                    chasePhase1();
                    break;
                case EnemyStates.SG_Phase2:
                    projectileTimer += Time.deltaTime;
                    if (projectileTimer > 7)
                    {
                        projectileTimer = 0;
                        StartCoroutine(ProjectileFire());
                    }
                    else
                    {
                        chasePhase2();
                    }                  
                    break;
                default:
                    break;
            }
        }
        
    }

    void chasePhase1()
    {
        float _dist = Vector2.Distance(transform.position, PlayerController.Instance.transform.position);
        if (!attacking)
        {
            if (canMove && _dist <= 3f)
            {
                Debug.Log("TEST");
                anim.SetBool("Walk", false);
                attackBehaviorPhase1();
            }
            else if (canMove && _dist >= 3f)
            {
                Debug.Log("TEST1");
                anim.SetBool("Walk", true);
                transform.position = Vector2.MoveTowards
                (transform.position, new Vector2(PlayerController.Instance.transform.position.x, transform.position.y),
                speed * Time.deltaTime);
            }
        }
    }

    void chasePhase2()
    {
        float _dist = Vector2.Distance(transform.position, PlayerController.Instance.transform.position);
        if (!attacking)
        {
            if (canMove && _dist <= 3f)
            {
                anim.SetBool("Run", false);
                attackBehaviorPhase2();
            }
            else if (canMove && _dist >= 3f)
            {
                anim.SetBool("Run", true);
                transform.position = Vector2.MoveTowards
                (transform.position, new Vector2(PlayerController.Instance.transform.position.x, transform.position.y),
                14 * Time.deltaTime);
            }
        }

    }

    void healthCheck()
    {
        if (health <= 0)
        {
            stopAllAttacks();
            anim.SetBool("Walk", false);
            anim.SetBool("Run", false);
            anim.SetTrigger("DEAD");
            canMove = false;
            isDead = true;
            canAttack = false;
            borderL.SetActive(false);
            borderR.SetActive(false);
            HealthBar.SetActive(false);
            spottedPlayer = false;
            Destroy(gameObject, 3f);
        }
    }
    void stopAllAttacks()
    {
        StopCoroutine(AttackP11());
        StopCoroutine(AttackP12());
        StopCoroutine(AttackP21());
        StopCoroutine(AttackP22());
        StopCoroutine(PounceAttack());
        StopCoroutine(PounceAttackPhase2());
        StopCoroutine(AttackBarrageP1());
        StopCoroutine(AttackBarrageP2());
    }

    //PHASE 1 ATTACKS
    IEnumerator AttackP11()
    {
        attacking = true;
        canMove = false;
        anim.SetTrigger("Attack1Phase1");
        yield return new WaitForSeconds(2f);
        attacking = false;
        canMove = true;
        ChangeStates(EnemyStates.SG_Idle);
    }

    IEnumerator AttackP12()
    {
        attacking = true;
        canMove = false;
        anim.SetTrigger("Attack2Phase1");
        yield return new WaitForSeconds(1f);
        attacking = false;
        canMove = true;
        ChangeStates(EnemyStates.SG_Idle);
    }

    IEnumerator AttackBarrageP1()
    {
        attacking = true;
        canMove = false;
        anim.SetTrigger("ComboPhase1");
        yield return new WaitForSeconds(2f);
        attacking = false;
        canMove = true;
        ChangeStates(EnemyStates.SG_Idle);
    }
    public float first;
    public float second;
    public float third;
    IEnumerator PounceAttack()
    {
        attacking = true;
        canMove = false;
        anim.SetTrigger("Pounce");
        CameraShake.Instance.ShakeCamera();
        rb.AddForce(new Vector2(0, verticalBounce), ForceMode2D.Impulse);
        yield return new WaitForSeconds(0.3f);
        rb.velocity = Vector2.zero;
        rb.gravityScale = 0;
        yield return new WaitForSeconds(0.4f);
        rb.gravityScale = 12f;
        CameraShake.Instance.ShakeCamera();
        yield return new WaitForSeconds(2);
        attacking = false;
        canMove = true;
        ChangeStates(EnemyStates.SG_Idle);
    }

    //PHASE 2 ATTACKS

    IEnumerator AttackP21()
    {
        attacking = true;
        canMove = false;
        anim.SetTrigger("Attack1Phase2");
        yield return new WaitForSeconds(2f);
        attacking = false;
        canMove = true;
        ChangeStates(EnemyStates.SG_Phase2);
    }

    IEnumerator AttackP22()
    {
        attacking = true;
        canMove = false;
        anim.SetTrigger("Attack2Phase2");
        yield return new WaitForSeconds(1.5f);
        attacking = false;
        canMove = true;
        ChangeStates(EnemyStates.SG_Phase2);
    }

    IEnumerator AttackBarrageP2()
    {
        attacking = true;
        canMove = false;
        anim.SetTrigger("ComboPhase2");
        yield return new WaitForSeconds(2.3f);
        attacking = false;
        canMove = true;
        ChangeStates(EnemyStates.SG_Phase2);
    }

    IEnumerator PounceAttackPhase2()
    {
        attacking = true;
        canMove = false;
        anim.SetTrigger("Pounce");
        CameraShake.Instance.ShakeCamera();
        rb.AddForce(new Vector2(0, verticalBounce), ForceMode2D.Impulse);
        yield return new WaitForSeconds(0.3f);
        rb.velocity = Vector2.zero;
        rb.gravityScale = 0;
        yield return new WaitForSeconds(0.4f);
        rb.gravityScale = 12f;
        CameraShake.Instance.ShakeCamera();
        yield return new WaitForSeconds(2);
        attacking = false;
        canMove = true;
        ChangeStates(EnemyStates.SG_Phase2);
    }
    IEnumerator QuickPounce()
    {
        float distancefromplayer = PlayerController.Instance.transform.position.x - transform.position.x;
        attacking = true;
        canMove = false;
        anim.SetTrigger("Phase2Pounce");
        CameraShake.Instance.ShakeCamera();
        rb.AddForce(new Vector2(distancefromplayer, 40), ForceMode2D.Impulse);
        yield return new WaitForSeconds(0.2f);
        rb.velocity = Vector2.zero;
        rb.gravityScale = 0;
        yield return new WaitForSeconds(0.3f);
        rb.gravityScale = 12f;
        CameraShake.Instance.ShakeCamera();
        yield return new WaitForSeconds(1);
        attacking = false;
        canMove = true;
        ChangeStates(EnemyStates.SG_Phase2);
    }

    IEnumerator ProjectileFire()
    {
        attacking = true;
        canMove = false;
        anim.SetBool("Run", false);
        anim.SetTrigger("Cast");
        yield return new WaitForSeconds(0.5f);
        Projectile();
        attacking = false;
        canMove = false; 
        ChangeStates(EnemyStates.SG_Phase2);
    }

    void Projectile()
    {
        Instantiate(bullet, bulletpos.position, Quaternion.identity);
    }
    void attackBehaviorPhase1()
    {
        int random = Random.Range(0,4);
        if (!attacking)
        {
            switch (random)
            {
                case 0:
                    StartCoroutine(AttackP11());
                    break;
                case 1:
                    StartCoroutine(AttackP12());
                    break;
                case 2:
                    StartCoroutine(AttackBarrageP1());
                    break;
                case 3:
                    StartCoroutine(PounceAttack());
                    break;
                default:
                    break;
            }
        }
    }

    void attackBehaviorPhase2()
    {
        int random = Random.Range(0, 5);
        if (!attacking)
        {
            switch (random)
            {
                case 0:
                    StartCoroutine(AttackP21());
                    break;
                case 1:
                    StartCoroutine(AttackP22());
                    break;
                case 2:
                    StartCoroutine(AttackBarrageP2());
                    break;
                case 3:
                    StartCoroutine(PounceAttackPhase2());
                    break;
                case 4:
                    StartCoroutine(QuickPounce());
                    break;
                default:
                    break;
            }
        }
    }

    public bool lookingRight = false;
    public bool lookingLeft = false;
    void flip()
    {
        if (PlayerController.Instance.transform.position.x < transform.position.x)
        {
            transform.eulerAngles = new Vector3(0, 180, 0);
            lookingLeft = true;
            lookingRight = false;
        }
        else
        {
            transform.eulerAngles = new Vector3(0, 0, 0);
            lookingLeft = false;
            lookingRight = true;
        }
    }
}

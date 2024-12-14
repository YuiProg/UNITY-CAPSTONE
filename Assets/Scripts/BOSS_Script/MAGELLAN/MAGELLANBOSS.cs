using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MAGELLANBOSS : Enemy
{
    public float chaseDistance;
    public bool isSecondPhase;
    public bool spottedPlayer;

    bool isAttacking = false;
    public bool hasTransformed = false;
    float UltiTimer;
    Animator anim;
    //essence
    [SerializeField] Transform essenceLOC;
    [SerializeField] GameObject Essence;

    //bars
    [SerializeField] GameObject HEALTHBAR;
    [SerializeField] GameObject BORDERR;
    [SerializeField] GameObject BORDERL;


    [SerializeField] GameObject NPC;
    AudioManager audioManager;
    public static MAGELLANBOSS instance;
    protected override void Start()
    {
        base.Start();
        canMove = true;
        canAttack = true;
        isSecondPhase = false;
        spottedPlayer = false;
        rb.gravityScale = 12f;
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
        anim = GetComponent<Animator>();
        ChangeStates(EnemyStates.MG_IDLE);

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
    int count;
    void loadCheck()
    {
        if (PlayerPrefs.GetInt("MAGELLAN") != 1)
        {
            gameObject.SetActive(true);
        }
        else
        {
            gameObject.SetActive(false);
            BORDERL.SetActive(false);
            BORDERR.SetActive(false);
            HEALTHBAR.SetActive(false);
        }
    }
    protected override void UpdateEnemyStates()
    {
        float dist = Vector2.Distance(transform.position, PlayerController.Instance.transform.position);
        stateCheck();
        flip(!isAttacking);
        
        isSecondPhase = health <= maxHealth / 2;
        
        if (isSecondPhase && !hasTransformed)
        {
            hasTransformed = true;
            StartCoroutine(Transform(3f));
        }
        if (canMove && PlayerController.Instance.pState.isAlive)
        {
            switch (currentEnemyStates)
            {
                case EnemyStates.MG_IDLE:
                    if (dist < chaseDistance)
                    {
                        spottedPlayer = true;
                        if (!isSecondPhase)
                        {
                            ChangeStates(EnemyStates.MG_CHASE);
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
                        StartCoroutine(Transform(3f));
                    }
                    else
                    {
                        ChangeStates(EnemyStates.MG_E_IDLE);
                    }
                    break;
                case EnemyStates.MG_E_IDLE:
                    anim.Play("MF_E_IDLE");
                    ChangeStates(EnemyStates.MG_E_CHASE);
                    break;
                case EnemyStates.MG_E_CHASE:
                    UltiTimer += Time.deltaTime;
                    if (distanceCheck())
                    {
                        ChangeStates(EnemyStates.MG_E_ATTACKBEHAVIOR);

                    }
                    anim.SetBool("MF_E_RUN", true);
                    transform.position = Vector2.MoveTowards(transform.position,
                        new Vector2(PlayerController.Instance.transform.position.x, transform.position.y), speed * Time.deltaTime);
                    break;
                case EnemyStates.MG_E_ATTACKBEHAVIOR:
                    if (!isAttacking)
                    {
                        if (UltiTimer >= 8f)
                        {
                            UltiTimer = 0f;
                            StartCoroutine(MF_E_ULTIMATE());
                        }
                        else
                        {
                            SecondPhaseAttackPattern();
                        }
                    }
                    break;
                default:
                    break;
            }
        }
    }
    //sounds

    void AttackSounds()
    {
        audioManager.PlaySFX(audioManager.M_Attack);
    }

    void explosionSound()
    {
        audioManager.PlaySFX(audioManager.M_Explosion);
    }

    void ShakeCamera()
    {
        CameraShake.Instance.ShakeCamera();
    }
    void stopallattacks()
    {
        StopAllCoroutines();
    }
    bool banner = false;
    void stateCheck()
    {
        canMove = !parried;
        BORDERL.SetActive(spottedPlayer && health >= 0);
        BORDERR.SetActive(spottedPlayer && health >= 0);
        HEALTHBAR.SetActive(spottedPlayer && health >= 0 && PlayerController.Instance.pState.isAlive);
        if (health <= 0)
        {
            NPC.SetActive(true);
            PlayerPrefs.SetInt("MAGELLAN", 1);
            canMove = false;
            isAttacking = true;
            canAttack = false;
            spottedPlayer = false;          
            anim.SetTrigger("MF_E_DEATH");
            dropEssence();
            PlayerPrefs.SetString("Quest", "Talk to the Tausug");
            Destroy(gameObject, 3f);
        }

        if (health <= 0 && !banner)
        {
            banner = true;
            PlayerController.Instance.pState.newJournalChapter = true;
        }
    }
    void dropEssence()
    {
        if (count != 5)
        {
            count++;
            Instantiate(Essence, essenceLOC.transform.position, Quaternion.identity);
        }
    }

    //transformation
    IEnumerator Transform(float time)
    {
        canMove = false;
        isAttacking = true;
        anim.SetTrigger("Transform");
        yield return new WaitForSeconds(time);
        canMove = true;
        isAttacking = false;
        ChangeStates(EnemyStates.MG_E_CHASE);
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

    void SecondPhaseAttackPattern()
    {
        int i = Random.Range(0, 4);

        switch (i)
        {
            case 0:
                StartCoroutine(MF_E_Attack1());
                break;
            case 1:
                StartCoroutine(MF_E_Attack2());
                break;
            case 2:
                StartCoroutine(MF_E_Attack3());
                break;
            case 3:
                StartCoroutine(MF_E_Attack4());
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
    //phase 2 attacks

    IEnumerator MF_E_Attack1()
    {
        anim.SetBool("MF_E_RUN", false);
        isAttacking = true;
        canMove = false;
        anim.SetTrigger("MF_E_Attack1");
        yield return new WaitForSeconds(1f);
        isAttacking = false;
        canMove = true;
        ChangeStates(EnemyStates.MG_E_CHASE);
    }

    IEnumerator MF_E_Attack2()
    {
        anim.SetBool("MF_E_RUN", false);
        isAttacking = true;
        canMove = false;
        anim.SetTrigger("MF_E_Attack2");
        yield return new WaitForSeconds(1.5f);
        isAttacking = false;
        canMove = true;
        ChangeStates(EnemyStates.MG_E_CHASE);
    }

    IEnumerator MF_E_Attack3()
    {
        anim.SetBool("MF_E_RUN", false);
        isAttacking = true;
        canMove = false;
        anim.SetTrigger("MF_E_Attack3");
        yield return new WaitForSeconds(2f);
        isAttacking = false;
        canMove = true;
        ChangeStates(EnemyStates.MG_E_CHASE);
    }

    IEnumerator MF_E_Attack4()
    {
        anim.SetBool("MF_E_RUN", false);
        isAttacking = true;
        canMove = false;
        anim.SetTrigger("MF_E_Attack4");
        yield return new WaitForSeconds(1f);
        isAttacking = false;
        canMove = true;
        ChangeStates(EnemyStates.MG_E_CHASE);
    }

    IEnumerator MF_E_ULTIMATE()
    {
        anim.SetBool("MF_E_RUN", false);
        isAttacking = true;
        canMove = false;
        anim.SetTrigger("MF_E_ULTIMATE");
        yield return new WaitForSeconds(2f);
        isAttacking = false;
        canMove = true;
        ChangeStates(EnemyStates.MG_E_CHASE);
    }

    public bool distanceCheck()
    {
        float dist = Vector2.Distance(transform.position, PlayerController.Instance.transform.position);
        return dist < 6f;
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

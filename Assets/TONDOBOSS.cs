using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TONDOBOSS : Enemy
{
    public float chaseDistance;
    public bool spottedPlayer = false;
    bool isAttacking = false;
    bool isSecondPhase = false;
    bool hasTransformed = false;
    [SerializeField] GameObject HEALTBAR;
    [SerializeField] GameObject PARRYBAR;
    [SerializeField] GameObject Amber;
    [SerializeField] GameObject Essence;
    [SerializeField] Transform amberLOC;
    [SerializeField] GameObject BORDERR;
    [SerializeField] GameObject BORDERL;
    Animator anim;
    AudioManager audiomanager;


    //dialogue
    [SerializeField] GameObject DIALOGUE;
    [SerializeField] Text dialogue;
    [SerializeField] GameObject UI;
    [SerializeField] Transform tpHere;



    public static TONDOBOSS instance;
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
        BORDERL.SetActive(PlayerPrefs.GetInt("TONDOMBOSS") != 1);
        BORDERR.SetActive(PlayerPrefs.GetInt("TONDOMBOSS") != 1);
        HEALTBAR.SetActive(PlayerPrefs.GetInt("TONDOMBOSS") != 1);
        gameObject.SetActive(PlayerPrefs.GetInt("TONDOMBOSS") != 1);
    }

    protected override void UpdateEnemyStates()
    {
        float distance = Vector2.Distance(transform.position, PlayerController.Instance.transform.position);
        stateCheck();
        flip(!isAttacking && health > 0 && !parried);
        isSecondPhase = health <= maxHealth / 2;
        if (isSecondPhase && !hasTransformed)
        {
            hasTransformed = true;
            StartCoroutine(Transform(5f));
        }
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
                    if (!hasTransformed)
                    {
                        hasTransformed = true;
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

    //sounds

    public void AttackSound()
    {
        audiomanager.PlaySFX(audiomanager.TB_Attack);
    }

    public void TransformSound()
    {
        audiomanager.PlaySFX(audiomanager.TB_Transform);
    }

    public void SlamSound()
    {
        audiomanager.PlaySFX(audiomanager.TB_SLAM);
    }

    public void CameraShakeFX()
    {
        CameraShake.Instance.ShakeCamera();
    }
    bool banner = false;
    bool dropped = false;
    bool startedDialogue = false;
    void stateCheck()
    {
        canMove = !parried && health > 0;
        canAttack = !parried && health > 0;
        BORDERL.SetActive(spottedPlayer && health > 0);
        BORDERR.SetActive(spottedPlayer && health > 0);
        HEALTBAR.SetActive(spottedPlayer && PlayerController.Instance.pState.isAlive && health > 0);
        if (health <= 0)
        {
            QuestTracker.instance.hasQuest = true;
            PlayerPrefs.SetString("Quest", "Search the area");
            PlayerPrefs.SetInt("TONDOMBOSS", 1);
            spottedPlayer = false;
            PARRYBAR.SetActive(false);
            anim.SetTrigger("Death");
        }
        if (health <= 0 && !banner)
        {
            banner = true;
            PlayerController.Instance.pState.killedABoss = true;
        }
        if (health <= 0 && !dropped)
        {
            amberDrop();
        }
        if (health <= 0 && !startedDialogue)
        {
            startedDialogue = true;
            StartCoroutine(Dialogue1(4.5f));
        }
    }

    //dialogue tas cutscene

    IEnumerator Dialogue1(float time)
    {
        yield return new WaitForSeconds(time + 1.5f);
        UI.SetActive(false);
        PlayerController.Instance.pState.canPause = false;
        PlayerController.Instance.pState.isNPC = true;
        DIALOGUE.SetActive(true);
        UI.SetActive(false);
        Cursor.visible = true;
        Save.instance.saveData();
        LevelManager.instance.loadscene("CUTSCENE3");
    }
    
    int Ambercount;
    int Essencecount;
    void amberDrop()
    {
        if (Ambercount != 15)
        {
            Ambercount++;
            Instantiate(Amber, amberLOC.position, Quaternion.identity);
        }
        if (Essencecount != 4)
        {
            Essencecount++;
            Instantiate(Essence, amberLOC.position, Quaternion.identity);
        }
        if (Ambercount == 15 && Essencecount == 4) dropped = true;
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
        yield return new WaitForSeconds(2f);
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
        yield return new WaitForSeconds(2f);
        isAttacking = false;
        canMove = true;
        ChangeStates(EnemyStates.TB_E_CHASE);
    }

    bool distanceCheck()
    {
        float distance = Vector2.Distance(transform.position, PlayerController.Instance.transform.position);
        return distance < 6f;
    }

    void flip(bool canFlip)
    {
        if (canFlip)
        {
            if (PlayerController.Instance.transform.position.x < transform.position.x)
            {
                transform.eulerAngles = new Vector3(0, 180, 0);
                HEALTBAR.transform.eulerAngles = new Vector3(0,0,0);
            }
            else
            {
                transform.eulerAngles = new Vector3(0, 0, 0);
                HEALTBAR.transform.eulerAngles = new Vector3(0, 0, 0);
            }
        }
    }
}

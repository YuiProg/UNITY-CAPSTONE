using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HorseBOSS : Enemy
{
    public float chaseDistance;
    public bool spottedPlayer = false;
    bool isAttacking = false;
    float dashtimer;
    Animator anim;
    AudioManager audiomanager;
    

    [SerializeField] Transform DASHL;
    [SerializeField] Transform DASHR;

    //border and health
    [SerializeField] GameObject BORDERL;
    [SerializeField] GameObject BORDERR;
    [SerializeField] GameObject HEALTHBAR;

    //drops
    [SerializeField] GameObject Amber;
    [SerializeField] Transform AmberLOC;

    //looking at

    public bool LookingLeft;
    public bool LookingRight;

    public static HorseBOSS instance;
    protected override void Start()
    {
        base.Start();
        canMove = true;
        canAttack = true;
        rb.gravityScale = 12f;
        audiomanager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
        anim = GetComponent<Animator>();
        ChangeStates(EnemyStates.H_Idle);

    }


    private void Awake()
    {
        loadcheck();
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
        }
    }

    void loadcheck()
    {
        if (PlayerPrefs.GetInt("HORSEBOSS") == 1)
        {
            HEALTHBAR.SetActive(false);
            BORDERL.SetActive(false);
            BORDERR.SetActive(false);
            gameObject.SetActive(false);
        }
    }
    protected override void UpdateEnemyStates()
    {
        float distance = Vector2.Distance(transform.position, PlayerController.Instance.transform.position);
        statecheck();
        flip(!isAttacking && health >= 0);
        if (canMove && PlayerController.Instance.pState.isAlive)
        {
            switch (currentEnemyStates)
            {
                case EnemyStates.H_Idle:
                    if (distance < chaseDistance)
                    {
                        spottedPlayer = true;
                        PlayerPrefs.SetString("Quest", "Defeat The Knight");
                        ChangeStates(EnemyStates.H_Chase);
                    }
                    break;
                case EnemyStates.H_Chase:
                    dashtimer += Time.deltaTime;
                    if (dashtimer >= 3)
                    {
                        dashtimer = 0f;
                        StartCoroutine(DashAttack());
                    }
                    if (distanceCheck())
                    {
                        ChangeStates(EnemyStates.H_AttackBehavior);
                    }
                    anim.SetBool("Chase", true);
                    transform.position = Vector2.MoveTowards(transform.position, new Vector2(PlayerController.Instance.transform.position.x, transform.position.y), speed * Time.deltaTime);
                    break;
                case EnemyStates.H_Charge:
                    break;
                case EnemyStates.H_AttackBehavior:
                    if (!isAttacking)
                    {
                        StartCoroutine(Attack1());
                    }
                    break;
                default:
                    break;
            }
        }
    }

    //sounds
    public void spearSound()
    {
        audiomanager.PlaySFX(audiomanager.H_Attack);
    }
    public void horseChaseFX()
    {
        audiomanager.PlaySFX(audiomanager.H_Chase);
    }

    public void stopFX()
    {
        audiomanager.StopSFX();
    }

    int count;
    bool hasdropped = false;
    void amberDrop()
    {
        if (count != 20)
        {
            count++;
            Instantiate(Amber, AmberLOC.position, Quaternion.identity);
        }
        if (count == 20)
        {
            hasdropped = true;
        }
    }
    bool banner = false;
    void statecheck()
    {
        BORDERL.SetActive(spottedPlayer && health > 0);
        BORDERR.SetActive(spottedPlayer && health > 0);
        HEALTHBAR.SetActive(spottedPlayer && health > 0);
        canMove = !parried && health >= 0;
        canAttack = !parried;
        if (health <= 0)
        {
            PlayerPrefs.SetString("Quest", "Find Sultan");
            BORDERL.SetActive(false);
            BORDERR.SetActive(false);
            HEALTHBAR.SetActive(false);
            spottedPlayer = false;
            anim.SetTrigger("Death");
            PlayerPrefs.SetInt("HORSEBOSS", 1);
            PlayerPrefs.SetInt("HEAL", 1);
            Destroy(gameObject, 2f);
        }
        if (health <= 0 && !banner)
        {
            banner = true;
            PlayerController.Instance.pState.SkillBOSS = true;
        }
        if (health <= 0 && !hasdropped)
        {
            amberDrop();
        }
    }
    IEnumerator DashAttack()
    {
        float elapsedtime = 0f;
        float dashduration = 2f;
        speed = 18f;
        canMove = false;
        isAttacking = true;
        anim.SetTrigger("Dash");
        yield return new WaitForSeconds(.8f);
        canMove = true;
        Vector2 targetPosition = new Vector2(PlayerController.Instance.transform.position.x, transform.position.y);
        while (elapsedtime < dashduration)
        {
            if (LookingLeft)
            {
                anim.SetBool("Chase", false);
                transform.position = Vector2.MoveTowards(transform.position, DASHL.position, speed * Time.deltaTime);
            }
            else
            {
                anim.SetBool("Chase", false);
                transform.position = Vector2.MoveTowards(transform.position, DASHR.position, speed * Time.deltaTime);
            }
            elapsedtime += Time.deltaTime;
            yield return null;
        }
        yield return new WaitForSeconds(.6f);
        isAttacking = false;
        speed = 12f;
        ChangeStates(EnemyStates.H_Idle);
    }
    IEnumerator Attack1()
    {
        anim.SetBool("Chase", false);
        isAttacking = true;
        canMove = false;
        anim.SetTrigger("Attack");
        yield return new WaitForSeconds(1f);
        isAttacking = false;
        canMove = true;
        ChangeStates(EnemyStates.H_Idle);
    }

    bool distanceCheck()
    {
        float distance = Vector2.Distance(transform.position, PlayerController.Instance.transform.position);
        return distance < 5f;
    }
    
    void flip(bool canflip)
    {
        if (canflip)
        {
            if (PlayerController.Instance.transform.position.x < transform.position.x)
            {
                transform.eulerAngles = new Vector3(0, 180, 0);
                LookingLeft = true;
                LookingRight = false;
            }
            else
            {
                transform.eulerAngles = new Vector3(0, 0, 0);
                LookingLeft = false;
                LookingRight = true;
            }
        }
        
    }
}

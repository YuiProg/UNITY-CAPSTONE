using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Paladin_BOSS : Enemy
{
    //BORDER
    [SerializeField] GameObject BORDERR;
    [SerializeField] GameObject BORDERL;

    //AMBER
    [SerializeField] Transform AmberLOC;
    [SerializeField] GameObject AMBER;

    //AOE ATTACK
    [SerializeField] GameObject StartAOE;

    //HITBOX
    [SerializeField] GameObject HITLEFT;
    [SerializeField] GameObject HITRIGHT;

    //UI
    [SerializeField] GameObject BossHP;

    //BOSS ULTIMATE
    [SerializeField] float ultitimer;
    [SerializeField] GameObject THUNDERULTI;
    bool isUlti = false;

    //music
    [SerializeField] AudioSource music;
    bool musicPlaying = false;

    public bool spottedPlayer = false;
    Animator anim;
    [SerializeField] float chaseDistance;
    bool secondphase = false;
    public bool isAlive;
    public static Paladin_BOSS instance;

    Vector2 spawnPoint;
    protected override void Start()
    {
        base.Start();
        spawnPoint = transform.position;
        BossHP.SetActive(false);
        StartAOE.SetActive(false);
        anim = GetComponent<Animator>();
        HITLEFT.SetActive(false);
        HITRIGHT.SetActive(false);
        THUNDERULTI.SetActive(false);
        ChangeStates(EnemyStates.PL_Idle);
        canAttack = false;
        canMove = false;
        isAlive = true;
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
        if (PlayerPrefs.GetInt("PALADIN") == 1)
        {
            gameObject.SetActive(false);
            BossHP.SetActive(false);
            BORDERL.SetActive(false);
            BORDERR.SetActive(false);
            print("PALADIN DEAD");
        }
        else if (PlayerPrefs.GetInt("PALADIN") == 0)
        {
            gameObject.SetActive(true);
            print("PALADIN ALIVE");
        }
    }
    bool banner = false;
    bool amberdropped = false;
    protected override void UpdateEnemyStates()
    {
        ultitimer += Time.deltaTime;
        speed = canMove ? 5 : 0;
        float _dist = Vector2.Distance(transform.position, PlayerController.Instance.transform.position);
        Flip();
        canMove = spottedPlayer;
        canAttack = spottedPlayer;
        BORDERL.SetActive(spottedPlayer);
        BORDERR.SetActive(spottedPlayer);
        if (spottedPlayer && !musicPlaying)
        {
            musicPlaying = true;
            music.Play();
        }
        if (health <= 0 && !banner)
        {
            banner = true;
            PlayerController.Instance.pState.killedABoss = true;
        }
        if (health <= 0 && !amberdropped)
        {
            amberdrop();
        }
        if (health <= 0)
        {
            PlayerPrefs.SetInt("PALADIN", 1);
            anim.SetBool("Walking", false);
            music.volume -= Time.deltaTime;
            anim.SetTrigger("Dead");
            canAttack = false;
            canMove = false;
            isAlive = false;
            BORDERL.SetActive(false);
            BORDERR.SetActive(false);
            BossHP.SetActive(false);
            Destroy(gameObject, 5f);
        }
        
        switch (currentEnemyStates)
        {            
            case EnemyStates.PL_Idle:
                if (_dist < chaseDistance)
                {
                    spottedPlayer = true;
                    ChangeStates(EnemyStates.PL_Stage1);
                }
                break;
            case EnemyStates.PL_Stage1:
                BossHP.SetActive(true);
                FirstPhase();
                break;
            case EnemyStates.PL_Stage2:
                secondPhase();
                break;
            default:
                break;
        }
    }
    int count;
    void amberdrop()
    {
        if (count != 10)
        {
            count++;
            Instantiate(AMBER, AmberLOC.position, Quaternion.identity);
        }
        if (count == 10)
        {
            amberdropped = true;
        }
    }
    void FirstPhase()
    {
        if (spottedPlayer)
        {
            
            Walk();
            if (health < maxHealth / 2)
            {
                secondphase = true;
                ChangeStates(EnemyStates.PL_Stage2);
            }
        }
        
    }
    void secondPhase()
    {
        if (ultitimer > 13)
        {
            ultitimer = 0;
            Debug.Log("ULTIMATE");
            StartCoroutine(ULTIMATE());
        }
        else if(!isUlti)
        {
            Walk2();
        }
    }
    void Walk2()
    {
        if (spottedPlayer && canMove && PlayerController.Instance.pState.isAlive && !attacking && !isUlti)
        {
            if (spottedPlayer && Vector2.Distance(transform.position, PlayerController.Instance.transform.position) <= 4f)
            {
                Debug.Log("ATTACK");
                anim.SetBool("Walk", false);
                canMove = false;
                StartCoroutine(AttackPL2());
            }
            else
            {
                Debug.Log("WALK");
                anim.SetBool("Walk", true);
                transform.position = Vector2.MoveTowards
                (transform.position, new Vector2(PlayerController.Instance.transform.position.x, transform.position.y),
                speed * Time.deltaTime);
            }
        }
    }

    void Walk()
    {
        if (spottedPlayer && canMove && PlayerController.Instance.pState.isAlive && !attacking && !isUlti)
        {
            if (spottedPlayer && Vector2.Distance(transform.position, PlayerController.Instance.transform.position) <= 4f)
            {
                Debug.Log("ATTACK");
                anim.SetBool("Walk", false);
                canMove = false;
                StartCoroutine(AttackPL());
            }
            else
            {
                Debug.Log("WALK");
                anim.SetBool("Walk", true);
                transform.position = Vector2.MoveTowards
                (transform.position, new Vector2(PlayerController.Instance.transform.position.x, transform.position.y),
                speed * Time.deltaTime);
            }
        }
    }
    bool attacking = false;
    void resetAllAttacks()
    {
        StopCoroutine(AttackPL2());
        StopCoroutine(AttackPL());
    }

    

    IEnumerator ULTIMATE()
    {
        
        anim.SetBool("Walk", false);
        isUlti = true;
        if (isUlti)
        {
            anim.SetTrigger("Thunder");
        }      
        yield return new WaitForSeconds(0.2f);
        if (isUlti)
        {
            CameraShake.Instance.ShakeCamera();
            THUNDERULTI.SetActive(true);
            
        }
        yield return new WaitForSeconds(1f);
        if (isUlti)
        {
            StartAOE.SetActive(true);
        }
        yield return new WaitForSeconds(3f);
        isUlti = false;
        ChangeStates(EnemyStates.PL_Idle);
    }
    IEnumerator AttackPL2()
    {
        attacking = true;
        yield return new WaitForSeconds(1);
        anim.SetTrigger("Attack");
        yield return new WaitForSeconds(2 - 0.2f);
        CameraShake.Instance.ShakeCamera();
        StartAOE.SetActive(true);
        HITLEFT.SetActive(true);
        HITRIGHT.SetActive(true);
        yield return new WaitForSeconds(1 - 0.5f);

        HITLEFT.SetActive(false);
        HITRIGHT.SetActive(false);
        resetAllAttacks();       
        
        yield return new WaitForSeconds(1.4f);
        canMove = true;
        attacking = false;

    }

    IEnumerator AttackPL()
    {
        attacking = true;
        yield return new WaitForSeconds(1);
        anim.SetTrigger("Attack");
        yield return new WaitForSeconds(2 - 0.2f);
        CameraShake.Instance.ShakeCamera();
        HITLEFT.SetActive(true);
        HITRIGHT.SetActive(true);
        yield return new WaitForSeconds(1 - 0.5f);

        HITLEFT.SetActive(false);
        HITRIGHT.SetActive(false);
        resetAllAttacks();

        yield return new WaitForSeconds(1.4f);
        canMove = true;
        attacking = false;
    }

    void Flip()
    {
        sr.flipX = PlayerController.Instance.transform.position.x < transform.position.x;
    }
}

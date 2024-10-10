using System.Collections;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class Demon_King : Enemy
{
    //Spawn Point
    Vector2 spawnpoint;

    //BOSS BORDER
    [SerializeField] GameObject Border_R;
    [SerializeField] GameObject Border_L;

    //HITBOX
    [SerializeField] float chaseDistace;
    [SerializeField] GameObject AttackHitBox;
    [SerializeField] GameObject AttackHitBox2;
    [SerializeField] GameObject HealthBar;

    //Projectile
    [SerializeField] Transform projectileLocation;
    [SerializeField] GameObject projectile;
    [SerializeField] GameObject groundAttack;
    [SerializeField] Transform gAttackHPOSL;
    [SerializeField] Transform gAttackHPOSR;
    [SerializeField] Transform gAttackPOSR;
    [SerializeField] Transform gAttackPOSL;

    //BOSS ULTI CHARGE
    [SerializeField] GameObject PreFire;
    [SerializeField] GameObject FireCharge;
    [SerializeField]float ultitimer;

    //TIMERS & STATES
    Animator anim;
    float secondtimerphase;
    bool spottedPlayer = false;
    float secondspeed = 10;
    bool secondphase = false;
    int secondcount = 0;
    float projectileTimer;
    bool underattack;
    bool playerDead;
    bool isAlive = true;
    [SerializeField]bool performUlti = false;
    [SerializeField] float fireTimer;
    protected override void Start()
    {
        base.Start();
        spawnpoint = transform.position;
        anim = GetComponent<Animator>();
        canAttack = true;
        canMove = true;
        secondphase = false;
        rb.gravityScale = 12f;
        HealthBar.SetActive(false); 
        ChangeStates(EnemyStates.DK_Idle);
        AttackHitBox.SetActive(false);
        AttackHitBox2.SetActive(false);
        PreFire.SetActive(false);
        FireCharge.SetActive(false);
        Border_L.SetActive(false);
        Border_R.SetActive(false);
        
    }
    private void Awake()
    {
        loadcheck();
    }

    void loadcheck()
    {
        if (PlayerPrefs.GetInt("DEMON") == 1)
        {
            gameObject.SetActive(false);
            HealthBar.SetActive(false);
            Border_L.SetActive(false);
            Border_R.SetActive(false);
            print("DEMON IS DEAD");
        }
        else if (PlayerPrefs.GetInt("DEMON") == 0)
        {
            gameObject.SetActive(true);
            print("DEMON IS ALIVE");
        }
    }
    protected override void UpdateEnemyStates()
    {
        projectileTimer += Time.deltaTime;
        fireTimer += Time.deltaTime;
        float _dist = Vector2.Distance(transform.position, PlayerController.Instance.transform.position);
        Flip();
        if (!canMove)
        {
            speed = 0;
        }
        else
        {
            speed = 5;
        }
        if (isAlive == false)
        {
            PlayerController.Instance.maxHealth += 0.03f;
            PlayerController.Instance.maxstamina += 0.03f;
            PlayerController.Instance.health = PlayerController.Instance.maxHealth;
            PlayerController.Instance.HealthBar.fillAmount = PlayerController.Instance.health;
        }
        if (spottedPlayer)
        {
            Border_R.SetActive(true);
            Border_L.SetActive(true);
        }
        if (parried)
        {
            canMove = false;
        }
        if (health <= 0)
        {
            PlayerPrefs.SetInt("DEMON", 1);
            isAlive = false;
            HealthBar.SetActive(false);
            canAttack = false;
            canMove = false;
            anim.SetTrigger("Dead");
            Destroy(gameObject, 5f);
            Border_R.SetActive(false);
            Border_L.SetActive(false);         
        }
        if (PlayerController.Instance.pState.isAlive)
        {           
            switch (currentEnemyStates)
            {
                case EnemyStates.DK_Idle:
                    if (_dist < chaseDistace)
                    {
                        spottedPlayer = true;
                        ChangeStates(EnemyStates.DK_Stage1);
                    }
                    break;
                case EnemyStates.DK_Stage1:
                    secondphase = false;
                    firstPhase();
                    HealthBar.SetActive(true);
                    
                    break;
                case EnemyStates.DK_Stage2:
                    secondphase = true;
                    secondPhase();
                    break;              
                default:
                    break;
            }

        }
        else if(!PlayerController.Instance.pState.isAlive)
        {
            ChangeStates(EnemyStates.DK_Idle);
            secondphase = false;
            ultitimer = 0;
            fireTimer = 0;
            HealthBar.SetActive(false);
            parrypercent = parrymax;
            parryBar.fillAmount = parrymax;
            health = maxHealth;
            transform.position = spawnpoint;
            spottedPlayer = false;
            AttackHitBox.SetActive(false);
            AttackHitBox2.SetActive(false);
            PreFire.SetActive(false);
            FireCharge.SetActive(false);
            Border_R.SetActive(false);
            Border_L.SetActive(false);
            secondtimerphase = 0;   
        }
        
    }

    void firstPhase()
    {      
        if (spottedPlayer)
        {
            Walk();
            if (health < maxHealth / 2)
            {
                
                secondphase = true;
                ChangeStates(EnemyStates.DK_Stage2);
            }

        }
        
    }

    void secondPhase()
    {
        ultitimer += Time.deltaTime;
        secondtimerphase += Time.deltaTime;
        
        if (secondtimerphase < 1.5)
        {
            Debug.Log("TRASITION");
            anim.SetBool("DK_Power_UP", true);

            canAttack = false;
        }
        if (secondphase && secondtimerphase > 1.5 && !performUlti)
        {
            anim.SetBool("DK_Power_UP", false);
            canAttack = true;
            Walk_2();
            if (ultitimer > 15)
            {
                StartCoroutine(bossUlti());
            }
            else if (fireTimer > 10)
            {
                fireAttack();
            }
        }

    }
    IEnumerator fireAttack()
    {
        Instantiate(groundAttack, gAttackPOSL.position, Quaternion.identity);
        Instantiate(groundAttack, gAttackHPOSR.position, Quaternion.identity);
        yield return new WaitForSeconds(0.8f);
        Instantiate(groundAttack, gAttackHPOSL.position, Quaternion.identity);
        Instantiate(groundAttack, gAttackPOSR.position, Quaternion.identity);
    }


    void ResetAllAttack()
    {
        StopCoroutine(DK_Attack());
        StopCoroutine(bossUlti());
    }

    IEnumerator DK_Attack()
    {
        yield return new WaitForSeconds(1);
        anim.SetTrigger("Attack");
        yield return new WaitForSeconds(1 - 0.3f);
        CameraShake.Instance.ShakeCamera();
        AttackHitBox.SetActive(true);
        AttackHitBox2.SetActive(true);
        yield return new WaitForSeconds(1 - 0.5f);
        AttackHitBox.SetActive(false);
        AttackHitBox2.SetActive(false);
        canMove = true;
        ResetAllAttack();
    }
    IEnumerator DK_Attack2()
    {
        yield return new WaitForSeconds(1);
        anim.SetTrigger("Attack");        
        yield return new WaitForSeconds(1 - 0.3f);
        StartCoroutine(fireAttack());
        CameraShake.Instance.ShakeCamera();
        AttackHitBox.SetActive(true);
        AttackHitBox2.SetActive(true);
        yield return new WaitForSeconds(1 - 0.5f);
        AttackHitBox.SetActive(false);
        AttackHitBox2.SetActive(false);
        canMove = true;
        ResetAllAttack();
    }
    void Walk()
    {
        if (spottedPlayer && canMove && canAttack && PlayerController.Instance.pState.isAlive)
        {
            Debug.Log("FIRST PHASE");
            if (spottedPlayer && Vector2.Distance(transform.position, PlayerController.Instance.transform.position) <= 12f)
            {
                anim.SetBool("Walk", false);
                canMove = false;
                StartCoroutine(DK_Attack());
            }
            else
            {
                anim.SetBool("Walk", true);
                transform.position = Vector2.MoveTowards
                (transform.position, new Vector2(PlayerController.Instance.transform.position.x, transform.position.y),
                speed * Time.deltaTime);
            }
            
        }                        
    }
    [SerializeField] Transform moveHere;
    float speedulti = 10;
    IEnumerator bossUlti()
    {
        performUlti = true;
        ultitimer = 0;
        if (performUlti)
        {
            Debug.Log("Boss Ultimate started");
            sr.flipX = PlayerController.Instance.transform.position.x < transform.position.x;

            if (performUlti)
            {
                anim.SetBool("Walk", true);
                transform.position = Vector2.MoveTowards
                (transform.position, new Vector2(moveHere.position.x, transform.position.y),
                speedulti * Time.deltaTime);
            }


            anim.SetBool("DK_Power_UP", true);
            yield return new WaitForSeconds(1); //TAKBO

            //anim.SetBool("Walk", false);
            if (performUlti)
            {

                PreFire.SetActive(true);                
            }            
            yield return new WaitForSeconds(2); //LASER BEAM 

            if (performUlti)
            {
                CameraShake.Instance.ShakeCamera();
                FireCharge.SetActive(true);
                Instantiate(groundAttack, gAttackPOSL.position, Quaternion.identity);
                Instantiate(groundAttack, gAttackHPOSR.position, Quaternion.identity);

                Instantiate(groundAttack, gAttackHPOSL.position, Quaternion.identity);
                Instantiate(groundAttack, gAttackPOSR.position, Quaternion.identity);
            }
            

            yield return new WaitForSeconds(7);
            anim.SetBool("DK_Power_UP", false);
            PreFire.SetActive(false);
            FireCharge.SetActive(false);

            performUlti = false;
            canMove = true;
            canAttack = true;
            ChangeStates(EnemyStates.DK_Idle);
            Debug.Log("Boss Ultimate ended");
            
        }
        
    }

    void Walk_2()
    {       
        if (spottedPlayer && canMove && canAttack && !performUlti)
        {
            
            if (spottedPlayer && Vector2.Distance(transform.position, PlayerController.Instance.transform.position) <= 12f)
            {
                canMove = false;
                anim.SetBool("Walk", false);               
                StartCoroutine(DK_Attack2());
            }           
            else if (spottedPlayer && Vector2.Distance(transform.position, PlayerController.Instance.transform.position) >= 20f)
            {
                anim.SetBool("Walk", false);
                if (projectileTimer > 7)
                {
                    projectileTimer = 0;
                    shoot();
                }
            }            
            else
            {
                anim.SetBool("Walk", true);
                transform.position = Vector2.MoveTowards
                (transform.position, new Vector2(PlayerController.Instance.transform.position.x, transform.position.y),
                secondspeed * Time.deltaTime);
            }

        }
    }
    void shoot()
    {
        Instantiate(projectile, projectileLocation.position, Quaternion.identity);
    }
    void Flip()
    {
        sr.flipX = PlayerController.Instance.transform.position.x > transform.position.x;
    }
}

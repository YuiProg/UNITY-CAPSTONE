using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    [Header("Horizontal Movement Settings:")]
    [SerializeField] public float walkSpeed;
    [Space(5)]

    [Header("Vertical Movement Settings")]
    [SerializeField] private float jumpForce = 45f;
    private int jumpBufferCounter = 0;
    [SerializeField] private int jumpBufferFrames;
    private float coyoteTimeCounter = 0;
    [SerializeField] private float coyoteTime;
    private int airJumpCounter = 0;
    [SerializeField] private int maxAirJumps;
    [Space(5)]

    [Header("Ground Check Settings:")]
    [SerializeField] private Transform groundCheckPoint;
    [SerializeField] private float groundCheckY = 0.2f;
    [SerializeField] private float groundCheckX = 0.5f;
    [SerializeField] private LayerMask whatIsGround;
    [Space(5)]

    [Header("Dash Settings")]
    [SerializeField] private float dashSpeed;
    [SerializeField] private float dashTime;
    [SerializeField] private float dashCooldown;
    [Space(5)]


    [Header("Attacking")]
    [SerializeField] public float damage;
    [SerializeField] public float hdamage;
    [SerializeField] public float Cdamage;
    [SerializeField] public float normal_damage;
    [SerializeField] public float normal_hdamage;
    [SerializeField] public float normal_slash_Damage;
    [SerializeField] public float spearDamage;
    [SerializeField] public float normal_spear_damage;
    [SerializeField] public Text parryCounter;
    [SerializeField] public Text comboCounter;
    [SerializeField] public Text SpearTimer;
    bool attack = true;


    //skill
    bool combo = true;
    bool DashSpear = true;
    [SerializeField] float comboTimer;
    public float SpearDashTimer;
    [SerializeField] ParticleSystem comboFX;

    //heal skill
    float healAmount = 20f;
    float healTimer;

    [HideInInspector]public PlayerState pState;
    private Rigidbody2D rb;
    private float xAxis, yAxis;
    private float gravity;
    Animator anim;
    private bool canDash = true;
    private bool dashed;


    [Header("Attack")] //pag nag level 10, 20 and 30 mas mataas dapat lifesteal
    [SerializeField] float normalAttackLS;
    [SerializeField] float hardAttackLS;
    [SerializeField] float jumpAttackLS;
    float parryDamageBonus = 0;
    bool canjump = true;
    bool attacking = false;
    bool canrun = true;
    bool HardAttack = false;
    [SerializeField]float timeBetweenAttack;
    [SerializeField]float timeSinceAttack;
    [SerializeField] Transform sideAttack, upAttack, downAttack;
    [SerializeField] Vector2 sideAttackArea, upAttackArea, downAttackArea;
    [SerializeField] LayerMask attackable;
    [Space(5)]

    [Header("Stamina Settings")]
    public Image Stamina;
    public Image ShieldBar;
    public float stamina, maxstamina;
    public float shieldCount, maxShield;
    [SerializeField] private float jumpCost;
    [SerializeField] private float runCost;
    [SerializeField] private float blockCost;
    [SerializeField] private float attackCost;
    [SerializeField] private float comboCost;
    [SerializeField] private float hardattackCost;
    [SerializeField] private float dashCost;
    private Coroutine recharge;
    [SerializeField] private float chargeRate;
    [Space(5)]

    [Header("Health Settings")]
    [SerializeField] public GameObject blood;
    [SerializeField] public GameObject blockFx;
    public Image HealthBar;
    public float health;
    public float maxHealth;
    public Enemy Edamage;
    public int potionCount;
    public int maxPotions;
    public float potionHealBar = 10f;
    bool canHeal = false;
    [Space(5)]

    [SerializeField] float hitFlashSpeed;

    [Header("Recoil Settings:")]
    [SerializeField] private int recoilXSteps = 5;
    [SerializeField] private int recoilYSteps = 5;
    [SerializeField] private float recoilXSpeed = 100; 
    [SerializeField] private float recoilYSpeed = 100; 
    private int stepsXRecoiled, stepsYRecoiled;
    [Space(5)]



    //statistics
    public int barya;
    public int levels;
    public int mainLevel;
    bool restoreTime;
    float restoreTimeSpeed;
    public bool takingDamage;
    [SerializeField]float parryTimer;
    [SerializeField] public ParticleSystem parryFX;
    public static PlayerController Instance;
    private SpriteRenderer sr;
    public ParticleSystem dust;
    [SerializeField] public Text text;
    [SerializeField] public Text Hp;
    [SerializeField] public Text StaminaCount;
    [SerializeField] public GameObject DeathScreen;
    Vector2 checkpointpos;

    //audio
    AudioManager audiomanager;
    private void Awake()
    {

        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
        
        health = maxHealth;
        stamina = maxstamina;
        Time.timeScale = 1;
        float x = PlayerPrefs.GetFloat("X");
        float y = PlayerPrefs.GetFloat("Y");
        transform.position = new Vector2(x, y);

        
    }

   
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        audiomanager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
        anim = GetComponent<Animator>();

        pState = GetComponent<PlayerState>();

        checkpointpos = transform.position;
        gravity = rb.gravityScale;
        sr = GetComponent<SpriteRenderer>();
        text.text = $"FLASK: {potionCount}";
        parryCounter.text = "";

        stamina = maxstamina;
        shieldCount = maxShield;
        potionCount = maxPotions;
        HealthBar.fillAmount = health / maxHealth;
        comboTimer = 5;
        healTimer = 30;
        SpearDashTimer = 5;
        Save.instance.loadStats();
        statCheck();
    }

    void Update()
    {
        //timers
        comboTimer += Time.deltaTime;
        healTimer += Time.deltaTime;
        timeSinceAttack += Time.deltaTime;
        SpearDashTimer += Time.deltaTime;

        //methods
        GetInputs();
        checkSkills();
        if (pState.dashing) return;
        RestoreTimeScale();
        Flip();
        Recoil();
        Attack();
        Block();
        runCheck();
        Move();
        Jump();
        StartDash();               
        RegenPotion();
        FlashWhileInvincible();
        pState.canAttack = timeSinceAttack > timeBetweenAttack; //lantaran if else dito pano to
        if (pState.Transitioning || pState.blocking || pState.isNPC)
        {
            rb.drag = 1000;
            anim.SetBool("Walking", false);
            anim.SetBool("Running", false);
        }
        else
        {
            rb.drag = 0.05f;
            
        }
        if (rb.velocity.x == 0 && rb.velocity.y == 0)
        {
            pState.running = false;
            pState.jumping = false;
            pState.dashing = false;
        }
        if (combo && pState.SLASH)
        {
            if (comboTimer > 5)
            {
                StartCoroutine(ComboAttack());
            }
            
        }
        if (DashSpear && pState.SpearDash)
        {
            if (SpearDashTimer >= 5)
            {
                StartCoroutine(DashAttack());
            }
        }
        //skill timers
        if (SpearDashTimer > 5)
        {
            SpearDashTimer = 5;
        }
        if (comboTimer > 5)
        {
            comboTimer = 5;
        }
        if (healTimer > 30)
        {
            healTimer = 30;
        }         
        if (pState.running == false)
        {
            walkSpeed = 7;
        }
        if (stamina <= 0)
        {
            canrun = false;
            canjump = false;
            walkSpeed = 7;
        }
        if (health >= maxHealth)
        {
            
            health = maxHealth;
            canHeal = false;
        }
        if (health < maxHealth)
        {
            canHeal = true;
        }
        text.text = $"POTION: {potionCount}";
        if (potionCount <= 0) text.text = $"POTION: EMPTY";
        if (health <= 0 || HealthBar.fillAmount == 0 && !hasDied)
        {
            
            DeathScreen.SetActive(true);
            pState.canMove = false;
            pState.jumping = false;
            pState.running = false;
            pState.dashing = false;
            pState.isAlive = false;
            health = PlayerPrefs.GetFloat("Max Health");
            stamina = PlayerPrefs.GetFloat("Max Stamina");
            shieldCount = PlayerPrefs.GetFloat("Max Shield");
            potionCount = PlayerPrefs.GetInt("MaxPotion");
            Save.instance.saveStats();
            rb.drag = 1000;
            rb.velocity = Vector2.zero;
            Dead();
            hasDied = true;
            return;
        }
        Hp.text = $"{Mathf.RoundToInt(health)}/{Mathf.RoundToInt(maxHealth)}";
        StaminaCount.text = $"{Mathf.RoundToInt(stamina)}/{Mathf.RoundToInt(maxstamina)}";
        parryTimer += Time.deltaTime;
        if (parryTimer > 0.3f)
        {
            parryTimer = 0;
            pState.parry = false;
        }
        comboCounter.text = $"{Math.Round(comboTimer)}";
        healtxtCount.text = $"{Math.Round(healTimer)}";
        SpearTimer.text = $"{Mathf.Round(SpearDashTimer)}";
    }

    bool hasDied = false;
    bool isRespawning = false;

    void statCheck()
    {
        if (PlayerPrefs.GetInt("LOAD") >= 1)
        {
            normal_hdamage = PlayerPrefs.GetFloat("H_Damage");
            normal_damage = PlayerPrefs.GetFloat("N_Damage");
            normal_slash_Damage = PlayerPrefs.GetFloat("C_Damage");
            normal_spear_damage = PlayerPrefs.GetFloat("Spear Damage");
            maxHealth = PlayerPrefs.GetFloat("Max Health");
            maxstamina = PlayerPrefs.GetFloat("Max Stamina");
            maxShield = PlayerPrefs.GetFloat("Max Shield");
            potionCount = PlayerPrefs.GetInt("Potion");
            maxPotions = PlayerPrefs.GetInt("MaxPotion");
            damage = normal_damage;
            hdamage = normal_hdamage;
            Cdamage = normal_slash_Damage;
            spearDamage = normal_spear_damage;
            mainLevel = PlayerPrefs.GetInt("MainLevel");
            Debug.Log("HAS STATS");
        }
        else if (PlayerPrefs.GetInt("LOAD") == 0 || !PlayerPrefs.HasKey("LOAD"))
        {
            normal_damage = 2;
            normal_hdamage = 4;
            normal_slash_Damage = 10;
            normal_spear_damage = 15;
            spearDamage = normal_spear_damage;
            damage = normal_damage;
            hdamage = normal_hdamage;
            Cdamage = normal_slash_Damage;
            maxHealth = 50;
            maxstamina = 100;
            maxShield = 50;
            mainLevel = 0;
            maxPotions = 3;
            Debug.Log("NO STATS");
        }
    }
    IEnumerator Respawn(float wait)
    {
        anim.SetTrigger("Death");
        yield return new WaitForSeconds(wait);       
        anim.SetBool("Walking", false);        
        pState.canMove = true;
        pState.isAlive = true;
        isRespawning = false;
        hasDied = false;
        LevelManager.instance.loadscene("Cave_1");
        
    }
    //health Bars
    void Dead()
    {
        if (!isRespawning)
        {
            pState.Transitioning = true;
            StartCoroutine(Respawn(4f));
            isRespawning = true;
            return;
        }
               
    }
    bool doubleJump = true;
    void Jump()
    {
        if (canjump && !pState.isNPC)
        {
            anim.SetBool("Jumping", !Grounded());
            if (Grounded() && !Input.GetButtonDown("Jump"))
            {
                doubleJump = true;
            }
            if (Input.GetButtonDown("Jump"))
            {
                
                if (Grounded() || doubleJump)
                {
                    rb.velocity = new Vector2(rb.velocity.x, jumpForce);
                    doubleJump = !doubleJump;
                }
                if (recharge != null) StopCoroutine(recharge);
                recharge = StartCoroutine(RechargeStamina());
                stamina -= jumpCost;
                Stamina.fillAmount = stamina / maxstamina;
            }
        }
        else if (Grounded())
        {
            anim.SetBool("Jumping", false);
        }         
    }
    public void updatecheckpoint(Vector2 pos)
    {
        checkpointpos = pos;
    }
    void RegenPotion()
    {
        if (potionCount > 8) potionCount = 8;
        if (Input.GetButtonDown("Heal") && potionCount > 0 && canHeal)
        {
            health += potionHealBar;
            HealthBar.fillAmount = health / maxHealth;
            potionCount--;
        }
        else if (potionCount <= 0)
        {
            potionCount = 0;
            canHeal = false;
        }
    }
    void Recoil()
    {
        if (pState.recoilingX)
        {
            if (pState.lookingRight)
            {
                rb.velocity = new Vector2(-recoilXSpeed, 0);
            }
            else
            {
                rb.velocity = new Vector2(recoilXSpeed, 0);
            }
        }

        if (pState.recoilingY)
        {
            rb.gravityScale = 0;
            if (yAxis < 0)
            {
                rb.velocity = new Vector2(rb.velocity.x, recoilYSpeed);
            }
            else
            {
                rb.velocity = new Vector2(rb.velocity.x, -recoilYSpeed);
            }
            airJumpCounter = 0;
        }
        else
        {
            rb.gravityScale = gravity;
        }

        //stop recoil
        if (pState.recoilingX && stepsXRecoiled < recoilXSteps)
        {
            stepsXRecoiled++;
        }
        else
        {
            StopRecoilX();
        }
        if (pState.recoilingY && stepsYRecoiled < recoilYSteps)
        {
            stepsYRecoiled++;
        }
        else
        {
            StopRecoilY();
        }

        if (Grounded())
        {
            StopRecoilY();
        }
    }
    void StopRecoilX()
    {
        stepsXRecoiled = 0;
        pState.recoilingX = false;
    }
    void StopRecoilY()
    {
        stepsYRecoiled = 0;
        pState.recoilingY = false;
    }
    public void HitStopTime(float _newTimeScale, int _restoreSpeed, float _delay)
    {
        restoreTimeSpeed = _restoreSpeed;
        Time.timeScale = _newTimeScale;
        if (_delay > 0)
        {
            StopCoroutine(StartTimeAgain(_delay));
            StartCoroutine(StartTimeAgain(_delay));
        }
        else
        {
            restoreTime = true;
        }
    }

    IEnumerator StartTimeAgain(float _delay)
    {
        restoreTime = true;
        yield return new WaitForSeconds(_delay);
    }

    void RestoreTimeScale ()
    {
        if (restoreTime)
        {
            if (Time.timeScale < 1)
            {
                Time.timeScale += Time.deltaTime * restoreTimeSpeed;
            }
            else
            {
                Time.timeScale = 1;
                restoreTime = false;
            }
        }
    }
    void GetInputs()
    {
        xAxis = Input.GetAxisRaw("Horizontal");
        yAxis = Input.GetAxisRaw("Vertical");
        attack = Input.GetMouseButtonDown(0);
        combo = Input.GetKeyDown(KeyCode.R);
        DashSpear = Input.GetKeyDown(KeyCode.R);
        HardAttack = Input.GetMouseButtonDown(1);
        
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            pState.SLASH = true;
            pState.HPBUFF = false;
            pState.SpearDash = false;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            pState.SLASH = false;
            pState.HPBUFF = true;
            pState.SpearDash = false;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha1) && pState.obtainedSpear)
        {
            pState.SLASH = false;
            pState.HPBUFF = false;
            pState.SpearDash = true;
        }
        if (Input.GetKeyDown(KeyCode.R) && pState.HPBUFF)
        {
            if (healTimer > 30)
            {
                health = maxHealth;
                HealthBar.fillAmount = health;
                healTimer = 0;
            }           
        }
    }
    [SerializeField] GameObject SlashIMG;
    [SerializeField] GameObject HealIMG;
    [SerializeField] ParticleSystem HealFX;
    [SerializeField] Text healtxtCount;
    [SerializeField] GameObject DASHIMG;
    void checkSkills()
    {
        if (!pState.SLASH)
        {
            SlashIMG.SetActive(false);
        }
        else
        {
            SlashIMG.SetActive(true);
        }
        if (!pState.HPBUFF)
        {
            HealIMG.SetActive(false);
        }
        else
        {
            HealIMG.SetActive(true);
        }
        if (!pState.SpearDash)
        {
            DASHIMG.SetActive(false);
        }
        else
        {
            DASHIMG.SetActive(true);
        }
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(sideAttack.position, sideAttackArea);
        Gizmos.DrawWireCube(upAttack.position, upAttackArea);
        Gizmos.DrawWireCube(downAttack.position, downAttackArea);
    }
    void parryDamagePlus()
    {
        damage = damage + parryDamageBonus;
        hdamage = hdamage + parryDamageBonus;
        Cdamage = Cdamage + parryDamageBonus;
        spearDamage = spearDamage + parryDamageBonus;
    }
    public void TakeDamage(float _damage)
    {
        if (!pState.blocking && !pState.invincible)
        {
            audiomanager.PlaySFX(audiomanager.Hurt);
            parryDamageBonus = 0;
            parryCounter.text = "";
            damage = normal_damage;
            hdamage = normal_hdamage;
            Cdamage = normal_slash_Damage;
            spearDamage = normal_spear_damage;
            print("Player is taking damage.");
            GameObject _enemyBlood = Instantiate(blood, transform.position, Quaternion.identity);
            Destroy(_enemyBlood, 5.5f);
            takingDamage = true;
            health -= Mathf.RoundToInt(_damage);
            HealthBar.fillAmount = health / maxHealth;
            StartCoroutine(StopTakingDamage());
        }
        else if (pState.parry)
        {
            audiomanager.PlaySFX(audiomanager.Parry);
            Color color = Color.yellow;
            Color white = Color.white;
            parryFX.Play();
            parryDamageBonus = parryDamageBonus + 1;
            parryCounter.text = $"{parryDamageBonus}";
            parryDamagePlus();
            print("Player is parrying damage.");                     
            if (parryDamageBonus > 5)
            {
                parryCounter.color = color;
            }
            else
            {
                parryCounter.color = white;
            }
            CameraShake.Instance.ShakeCamera();
        }
        else
        {
            audiomanager.PlaySFX(audiomanager.Block);
            parryDamageBonus = 0;
            parryCounter.text = "";
            damage = normal_damage;
            hdamage = normal_hdamage;
            Cdamage = normal_slash_Damage;
            print("Player is blocking damage.");
            GameObject _enemyBlood = Instantiate(blockFx, transform.position, Quaternion.identity);
            Destroy(_enemyBlood, 5.5f);
            shieldCount -= Mathf.RoundToInt(_damage);
            ShieldBar.fillAmount = shieldCount / maxShield;
            CameraShake.Instance.ShakeCamera();
        }       
    }

    IEnumerator StopTakingDamage()
    {
        if (!pState.blocking)
        {
            pState.invincible = true;
            canDash = false;
            anim.SetTrigger("TakeDamage");
            ClampHealth();
            yield return new WaitForSeconds(0.2f);
            pState.invincible = false;
            canDash = true;
            takingDamage = false;
        }
        else if (pState.blocking)
        {
            pState.invincible = true;
            canDash = false;
            ClampHealth();
            yield return new WaitForSeconds(0.2f);
            pState.invincible = false;
            canDash = true;
            takingDamage = false;
        }
    }

    void Block()
    {
        
        if (!pState.jumping && !pState.running && Grounded())
        {
            if (Input.GetKeyDown(KeyCode.E))
            {               
                if (parryTimer < 0.3f)
                {
                    pState.parry = true;
                    parryTimer = 0;                    
                }
                canDash = false;
                pState.walking = false;
                anim.SetBool("Blocking", true);
                anim.SetBool("Walking", false);
                pState.blocking = true;
                print("BLOCKING");
                pState.canMove = false;
            }
            if (Input.GetKeyUp(KeyCode.E))
            {
                canDash = true;
                anim.SetBool("Blocking", false);
                pState.canMove = true;
                pState.blocking = false;
                
            }
        }
        if (shieldCount <= 0)
        {
            anim.SetBool("Blocking", false);
            shieldCount = 0;
            pState.blocking = false;
        }
        if (shieldCount > maxShield)
        {
            shieldCount = maxShield;
        }
    }
    public void blockCheck(float damage)
    {
        if (pState.blocking)
        {
            shieldCount -= damage;
            ShieldBar.fillAmount = shieldCount / maxShield;
        }       
    }
    void FlashWhileInvincible()
    {
        sr.material.color = pState.invincible ? Color.Lerp(Color.white, Color.black, Mathf.PingPong(Time.time * hitFlashSpeed, 1.0f)) : Color.white;
    }
    void ClampHealth()
    {
        health = Mathf.Clamp(health, 0, maxHealth);
    }

    void Attack()
    {
        if (pState.canAttack && pState.isAlive && !pState.isPaused && !pState.isNPC)
        {
            
            if (attack && pState.isAlive && stamina > 10 && !pState.blocking)
            {
                audiomanager.PlaySFX(audiomanager.NormalAttack);
                timeSinceAttack = 0;
                pState.canAttack = false;
                anim.SetTrigger("Attacking");
                if (recharge != null) StopCoroutine(recharge);
                recharge = StartCoroutine(RechargeStamina());
                stamina -= attackCost;
                Stamina.fillAmount = stamina / maxstamina;


                if (yAxis == 0 || yAxis < 0 && Grounded())
                {
                    int _lookingLeftorRight = pState.lookingRight ? 1 : -1;
                    print("sideAttackArea: " + sideAttackArea);
                    NormalHit(sideAttack, sideAttackArea, ref pState.recoilingX, Vector2.right * -_lookingLeftorRight, -recoilXSpeed);
                }
                else if (yAxis > 0)
                {
                    print("upattackarea: " + upAttackArea);
                    NormalHit(upAttack, upAttackArea, ref pState.recoilingX, Vector2.up, recoilXSpeed);
                }
                else if (yAxis < 0 && !Grounded())
                {
                    print("downattackarea: " + downAttackArea);
                    NormalHit(downAttack, downAttackArea, ref pState.recoilingY, Vector2.down, recoilYSpeed);
                }
            }
            else if (HardAttack && stamina > 30 && !pState.blocking)
            {
                HardAttackk();
            }
        }                         
    }

    IEnumerator DashAttack()
    {
        if (lookingleft && Grounded())
        {
            if (DashSpear && pState.isAlive && stamina > 10 && !pState.blocking && !pState.isNPC)
            {
                audiomanager.PlaySFX(audiomanager.NormalAttack);
                canDash = false;
                SpearDashTimer = 0;
                pState.invincible = true;

                timeSinceAttack = 0;
                pState.canAttack = false;
                anim.SetTrigger("SpearDash");
                rb.AddForce(new Vector2(-.5f, 0), ForceMode2D.Force);

                if (yAxis == 0 || yAxis < 0 && Grounded())
                {
                    int _lookingLeftorRight = pState.lookingRight ? 1 : -1;
                    print("COMBO ATTACK");
                    SpearHit(sideAttack, sideAttackArea, ref pState.recoilingX, Vector2.right * -_lookingLeftorRight, -recoilXSpeed);
                }
                else if (yAxis > 0)
                {
                    print("upattackarea: " + upAttackArea);
                    SpearHit(upAttack, upAttackArea, ref pState.recoilingX, Vector2.up, recoilXSpeed);
                }
                else if (yAxis < 0 && !Grounded())
                {
                    print("downattackarea: " + downAttackArea);
                    SpearHit(downAttack, downAttackArea, ref pState.recoilingY, Vector2.down, recoilYSpeed);
                }
            }
            yield return new WaitForSeconds(0.3f);


            if (pState.isAlive && stamina > 10 && !pState.blocking && !pState.isNPC)
            {
                audiomanager.PlaySFX(audiomanager.NormalAttack);
                timeSinceAttack = 0;
                pState.canAttack = false;
                anim.SetTrigger("SpearDash");
                rb.AddForce(new Vector2(-.5f, 0), ForceMode2D.Force);
                if (yAxis == 0 || yAxis < 0 && Grounded())
                {
                    int _lookingLeftorRight = pState.lookingRight ? 1 : -1;
                    print("COMBO ATTACK");
                    SpearHit(sideAttack, sideAttackArea, ref pState.recoilingX, Vector2.right * -_lookingLeftorRight, -recoilXSpeed);
                }
                else if (yAxis > 0)
                {
                    print("upattackarea: " + upAttackArea);
                    SpearHit(upAttack, upAttackArea, ref pState.recoilingX, Vector2.up, recoilXSpeed);
                }
                else if (yAxis < 0 && !Grounded())
                {
                    print("downattackarea: " + downAttackArea);
                    SpearHit(downAttack, downAttackArea, ref pState.recoilingY, Vector2.down, recoilYSpeed);
                }
            }
            yield return new WaitForSeconds(0.3f);


            if (pState.isAlive && stamina > 10 && !pState.blocking && !pState.isNPC)
            {
                audiomanager.PlaySFX(audiomanager.NormalAttack);
                timeSinceAttack = 0;
                pState.canAttack = false;

                anim.SetTrigger("SpearDash");
                rb.AddForce(new Vector2(-.5f, 0), ForceMode2D.Force);
                if (yAxis == 0 || yAxis < 0 && Grounded())
                {
                    int _lookingLeftorRight = pState.lookingRight ? 1 : -1;
                    print("COMBO ATTACK");
                    SpearHit(sideAttack, sideAttackArea, ref pState.recoilingX, Vector2.right * -_lookingLeftorRight, -recoilXSpeed);
                }
                else if (yAxis > 0)
                {
                    print("upattackarea: " + upAttackArea);
                    SpearHit(upAttack, upAttackArea, ref pState.recoilingX, Vector2.up, recoilXSpeed);
                }
                else if (yAxis < 0 && !Grounded())
                {
                    print("downattackarea: " + downAttackArea);
                    SpearHit(downAttack, downAttackArea, ref pState.recoilingY, Vector2.down, recoilYSpeed);
                }
            }
            yield return new WaitForSeconds(0.3f);
            pState.invincible = false;
            canDash = true;

        }
        else if (lookingright && Grounded())
        {
            if (DashSpear && pState.isAlive && stamina > 10 && !pState.blocking && !pState.isNPC)
            {
                audiomanager.PlaySFX(audiomanager.NormalAttack);
                canDash = false;
                SpearDashTimer = 0;
                pState.invincible = true;

                timeSinceAttack = 0;
                pState.canAttack = false;
                anim.SetTrigger("SpearDash");
                rb.AddForce(new Vector2(.5f, 0), ForceMode2D.Force);

                if (yAxis == 0 || yAxis < 0 && Grounded())
                {
                    int _lookingLeftorRight = pState.lookingRight ? 1 : -1;
                    print("COMBO ATTACK");
                    SpearHit(sideAttack, sideAttackArea, ref pState.recoilingX, Vector2.right * -_lookingLeftorRight, -recoilXSpeed);
                }
                else if (yAxis > 0)
                {
                    print("upattackarea: " + upAttackArea);
                    SpearHit(upAttack, upAttackArea, ref pState.recoilingX, Vector2.up, recoilXSpeed);
                }
                else if (yAxis < 0 && !Grounded())
                {
                    print("downattackarea: " + downAttackArea);
                    SpearHit(downAttack, downAttackArea, ref pState.recoilingY, Vector2.down, recoilYSpeed);
                }
            }
            yield return new WaitForSeconds(0.3f);


            if (pState.isAlive && stamina > 10 && !pState.blocking && !pState.isNPC)
            {
                audiomanager.PlaySFX(audiomanager.NormalAttack);
                timeSinceAttack = 0;
                pState.canAttack = false;
                anim.SetTrigger("SpearDash");
                rb.AddForce(new Vector2(.5f, 0), ForceMode2D.Force);
                if (yAxis == 0 || yAxis < 0 && Grounded())
                {
                    int _lookingLeftorRight = pState.lookingRight ? 1 : -1;
                    print("COMBO ATTACK");
                    SpearHit(sideAttack, sideAttackArea, ref pState.recoilingX, Vector2.right * -_lookingLeftorRight, -recoilXSpeed);
                }
                else if (yAxis > 0)
                {
                    print("upattackarea: " + upAttackArea);
                    SpearHit(upAttack, upAttackArea, ref pState.recoilingX, Vector2.up, recoilXSpeed);
                }
                else if (yAxis < 0 && !Grounded())
                {
                    print("downattackarea: " + downAttackArea);
                    SpearHit(downAttack, downAttackArea, ref pState.recoilingY, Vector2.down, recoilYSpeed);
                }
            }
            yield return new WaitForSeconds(0.3f);


            if (pState.isAlive && stamina > 10 && !pState.blocking && !pState.isNPC)
            {
                audiomanager.PlaySFX(audiomanager.NormalAttack);
                timeSinceAttack = 0;
                pState.canAttack = false;

                anim.SetTrigger("SpearDash");
                rb.AddForce(new Vector2(.5f, 0), ForceMode2D.Force);
                if (yAxis == 0 || yAxis < 0 && Grounded())
                {
                    int _lookingLeftorRight = pState.lookingRight ? 1 : -1;
                    print("COMBO ATTACK");
                    SpearHit(sideAttack, sideAttackArea, ref pState.recoilingX, Vector2.right * -_lookingLeftorRight, -recoilXSpeed);
                }
                else if (yAxis > 0)
                {
                    print("upattackarea: " + upAttackArea);
                    SpearHit(upAttack, upAttackArea, ref pState.recoilingX, Vector2.up, recoilXSpeed);
                }
                else if (yAxis < 0 && !Grounded())
                {
                    print("downattackarea: " + downAttackArea);
                    SpearHit(downAttack, downAttackArea, ref pState.recoilingY, Vector2.down, recoilYSpeed);
                }
            }
            yield return new WaitForSeconds(0.3f);
            pState.invincible = false;
            canDash = true;

        }

    }
    IEnumerator ComboAttack()
    {
        if (combo && pState.isAlive && stamina > 10 && !pState.blocking && !pState.isNPC)
        {
            audiomanager.PlaySFX(audiomanager.NormalAttack);
            canDash = false;
            comboTimer = 0;
            rb.drag = 1000;
            pState.invincible = true;

            comboFX.Play();
            timeSinceAttack = 0;
            pState.canAttack = false;
            anim.SetTrigger("Combo");

            if (yAxis == 0 || yAxis < 0 && Grounded())
            {
                int _lookingLeftorRight = pState.lookingRight ? 1 : -1;
                print("COMBO ATTACK");
                ComboHit(sideAttack, sideAttackArea, ref pState.recoilingX, Vector2.right * -_lookingLeftorRight, -recoilXSpeed);
            }
            else if (yAxis > 0)
            {
                print("upattackarea: " + upAttackArea);
                ComboHit(upAttack, upAttackArea, ref pState.recoilingX, Vector2.up, recoilXSpeed);
            }
            else if (yAxis < 0 && !Grounded())
            {
                print("downattackarea: " + downAttackArea);
                ComboHit(downAttack, downAttackArea, ref pState.recoilingY, Vector2.down, recoilYSpeed);
            }
        }
        yield return new WaitForSeconds(0.3f);


        if (pState.isAlive && stamina > 10 && !pState.blocking && !pState.isNPC)
        {
            audiomanager.PlaySFX(audiomanager.NormalAttack);
            comboFX.Play();
            timeSinceAttack = 0;
            pState.canAttack = false;


            if (yAxis == 0 || yAxis < 0 && Grounded())
            {
                int _lookingLeftorRight = pState.lookingRight ? 1 : -1;
                print("COMBO ATTACK");
                ComboHit(sideAttack, sideAttackArea, ref pState.recoilingX, Vector2.right * -_lookingLeftorRight, -recoilXSpeed);
            }
            else if (yAxis > 0)
            {
                print("upattackarea: " + upAttackArea);
                ComboHit(upAttack, upAttackArea, ref pState.recoilingX, Vector2.up, recoilXSpeed);
            }
            else if (yAxis < 0 && !Grounded())
            {
                print("downattackarea: " + downAttackArea);
                ComboHit(downAttack, downAttackArea, ref pState.recoilingY, Vector2.down, recoilYSpeed);
            }
        }
        yield return new WaitForSeconds(0.3f);


        if (pState.isAlive && stamina > 10 && !pState.blocking && !pState.isNPC)
        {
            audiomanager.PlaySFX(audiomanager.NormalAttack);
            comboFX.Play();
            timeSinceAttack = 0;
            pState.canAttack = false;



            if (yAxis == 0 || yAxis < 0 && Grounded())
            {
                int _lookingLeftorRight = pState.lookingRight ? 1 : -1;
                print("COMBO ATTACK");
                ComboHit(sideAttack, sideAttackArea, ref pState.recoilingX, Vector2.right * -_lookingLeftorRight, -recoilXSpeed);
            }
            else if (yAxis > 0)
            {
                print("upattackarea: " + upAttackArea);
                ComboHit(upAttack, upAttackArea, ref pState.recoilingX, Vector2.up, recoilXSpeed);
            }
            else if (yAxis < 0 && !Grounded())
            {
                print("downattackarea: " + downAttackArea);
                ComboHit(downAttack, downAttackArea, ref pState.recoilingY, Vector2.down, recoilYSpeed);
            }
        }
        yield return new WaitForSeconds(0.3f);
        rb.drag = 0;
        pState.invincible = false;
        canDash = true;


    }
    private void SpearHit(Transform _attackTransform, Vector2 _attackArea, ref bool _recoilBool, Vector2 _recoilDir, float _recoilStrength)
    {
        Collider2D[] objectstohit = Physics2D.OverlapBoxAll(_attackTransform.position, _attackArea, 0, attackable);
        List<Enemy> hitenemies = new List<Enemy>();
        if (objectstohit.Length > 0)
        {
            print("HIT");
            _recoilBool = true;
        }
        for (int i = 0; i < objectstohit.Length; i++)
        {
            if (objectstohit[i].GetComponent<Enemy>() != null)
            {
                Enemy e = objectstohit[i].GetComponent<Enemy>();
                if (e && !hitenemies.Contains(e))
                {
                    e.EnemyHit(Cdamage, (transform.position - objectstohit[i].transform.position).normalized, _recoilStrength);
                    hitenemies.Add(e);
                }
                health += normalAttackLS;
                HealthBar.fillAmount = health / maxHealth;

                shieldCount += normalAttackLS + 2;
                ShieldBar.fillAmount = shieldCount / maxShield;
            }
        }
    }
    private void ComboHit(Transform _attackTransform, Vector2 _attackArea, ref bool _recoilBool, Vector2 _recoilDir, float _recoilStrength)
    {
        Collider2D[] objectstohit = Physics2D.OverlapBoxAll(_attackTransform.position, _attackArea, 0, attackable);
        List<Enemy> hitenemies = new List<Enemy>();
        if (objectstohit.Length > 0)
        {
            print("HIT");
            _recoilBool = true;
        }
        for (int i = 0; i < objectstohit.Length; i++)
        {
            if (objectstohit[i].GetComponent<Enemy>() != null)
            {
                Enemy e = objectstohit[i].GetComponent<Enemy>();
                if (e && !hitenemies.Contains(e))
                {
                    e.EnemyHit(spearDamage, (transform.position - objectstohit[i].transform.position).normalized, _recoilStrength);
                    hitenemies.Add(e);
                }
                health += normalAttackLS;
                HealthBar.fillAmount = health / maxHealth;

                shieldCount += normalAttackLS + 2;
                ShieldBar.fillAmount = shieldCount / maxShield;
            }
        }
    }
    private void NormalHit(Transform _attackTransform, Vector2 _attackArea, ref bool _recoilBool, Vector2 _recoilDir, float _recoilStrength)
    {
        Collider2D[] objectstohit = Physics2D.OverlapBoxAll(_attackTransform.position, _attackArea, 0, attackable);
        List<Enemy> hitenemies = new List<Enemy>();
        if (objectstohit.Length > 0)
        {
            print("HIT");
            _recoilBool = true;
        }
        for (int i = 0; i < objectstohit.Length; i++)
        {
            if (objectstohit[i].GetComponent<Enemy>() != null)
            {
                Enemy e = objectstohit[i].GetComponent<Enemy>();
                if (e && !hitenemies.Contains(e))
                {
                    e.EnemyHit(damage, (transform.position - objectstohit[i].transform.position).normalized, _recoilStrength);
                    hitenemies.Add(e);  
                }
                health += normalAttackLS;
                HealthBar.fillAmount = health / maxHealth;

                shieldCount += normalAttackLS + 2;
                ShieldBar.fillAmount = shieldCount / maxShield;
            }
        }
    }
    private void HardHit(Transform _attackTransform, Vector2 _attackArea, ref bool _recoilBool, Vector2 _recoilDir, float _recoilStrength)
    {
        Collider2D[] objectstohit = Physics2D.OverlapBoxAll(_attackTransform.position, _attackArea, 0, attackable);
        List<Enemy> hitenemies = new List<Enemy>();
        if (objectstohit.Length > 0)
        {
            print("HIT");
            _recoilBool = true;
        }
        for (int i = 0; i < objectstohit.Length; i++)
        {
            if (objectstohit[i].GetComponent<Enemy>() != null)
            {
                Enemy e = objectstohit[i].GetComponent<Enemy>();
                if (e && !hitenemies.Contains(e))
                {
                    e.EnemyHit(hdamage, (transform.position - objectstohit[i].transform.position).normalized, _recoilStrength);
                    hitenemies.Add(e);
                }
                health += normalAttackLS;
                HealthBar.fillAmount = health / maxHealth;

                shieldCount += normalAttackLS + 2;
                ShieldBar.fillAmount = shieldCount / maxShield;
            }
        }
    }
   

    void HardAttackk()
    {
        if (pState.canAttack && pState.isAlive)
        {           
            if (HardAttack && timeSinceAttack >= timeBetweenAttack && pState.isAlive)
            {
                audiomanager.PlaySFX(audiomanager.NormalAttack);
                timeSinceAttack = 0;
                pState.canAttack = false;
                anim.SetTrigger("Hard Attack");
                if (recharge != null) StopCoroutine(recharge);
                recharge = StartCoroutine(RechargeStamina());
                stamina -= hardattackCost;
                Stamina.fillAmount = stamina / maxstamina;
            }
            if (yAxis == 0 || yAxis < 0 && Grounded())
            {
                int _lookingLeftorRight = pState.lookingRight ? 1 : -1;
                print("sideAttackArea: " + sideAttackArea);
                HardHit(sideAttack, sideAttackArea, ref pState.recoilingX, Vector2.right * -_lookingLeftorRight, -recoilXSpeed);
            }
            else if (yAxis > 0)
            {
                print("upattackarea: " + upAttackArea);
                HardHit(upAttack, upAttackArea, ref pState.recoilingX, Vector2.up, recoilXSpeed);
            }
            else if (yAxis < 0 && !Grounded())
            {
                print("downattackarea: " + downAttackArea);
                HardHit(downAttack, downAttackArea, ref pState.recoilingY, Vector2.down, recoilYSpeed);
            }
        }
        
    }
    public bool lookingleft;
    public bool lookingright;
    void Flip()
    {
        if (pState.isAlive && !pState.isPaused && !pState.isNPC)
        {
            if (Grounded())
            {
                dust.Play();
            }
            if (xAxis < 0)
            {
                lookingleft = true;
                lookingright = false;
                transform.localScale = new Vector2(-1.672545f, transform.localScale.y);
                pState.lookingRight = false;
            }
            else if (xAxis > 0)
            {
                lookingleft = false;
                lookingright = true;
                transform.localScale = new Vector2(1.672545f, transform.localScale.y);
                pState.lookingRight = true;
            }
        }               
    }

    private void Move()
    {
        if (pState.canMove && pState.isAlive && !pState.isNPC && !pState.isPaused)
        {
            pState.walking = true;
            rb.velocity = new Vector2(walkSpeed * xAxis, rb.velocity.y);
            anim.SetBool("Walking", rb.velocity.x != 0 && Grounded());
            if (stamina > 0 && canrun == true && Input.GetButtonDown("Run"))
            {
                walkSpeed = 13;
                pState.running = true;
                pState.walking = false;
            }
            else if (Input.GetButtonUp("Run"))
            {
                walkSpeed = 7;
                pState.running = false;
                pState.walking = true;
            }
            
        }
        else if (pState.canMove == false && pState.isAlive == false)
        {
            anim.SetBool("Death", true);
            //rb.MovePosition(rb.position);
            rb.velocity = Vector2.zero;
        }
        
             
    }
    void runCheck()
    {
        if (pState.running == true)
        {
            if (stamina <= 0)
            if (stamina <= 0)
            {
                stamina = 0;
            }
            dust.Play();
            anim.SetBool("Running", pState.running && Grounded());

            
            if (stamina <= 0 && pState.running == false)
            {
                canrun = false;
            }
        }
        else if (pState.walking == true)
        {
            dust.Stop();
            anim.SetBool("Walking", true);
            anim.SetBool("Running", false);
        }

    }


    void StartDash()
    {
        if (stamina > 0 && pState.isAlive && Input.GetButtonDown("Dash") && canDash && !dashed && !pState.isNPC)
        {
            StartCoroutine(Dash());
            dashed = true;
            if (recharge != null) StopCoroutine(recharge);
            recharge = StartCoroutine(RechargeStamina());
            stamina -= dashCost;
            Stamina.fillAmount = stamina / maxstamina;
        }

        if (Grounded())
        {
            dashed = false;
        }
    }

    IEnumerator Dash() 
    {
        canDash = false;
        pState.dashing = true;
        dust.Play();
        pState.invincible = true;
        anim.SetTrigger("Dashing");
        rb.velocity = new Vector2(transform.localScale.x * dashSpeed, 0);
        yield return new WaitForSeconds(dashTime);
        rb.gravityScale = gravity;
        pState.dashing = false;
        dust.Stop();
        yield return new WaitForSeconds(dashCooldown);
        canDash = true;
        pState.invincible = false;
    }

    public bool Grounded()
    {
        if (Physics2D.Raycast(groundCheckPoint.position, Vector2.down, groundCheckY, whatIsGround)
            || Physics2D.Raycast(groundCheckPoint.position + new Vector3(groundCheckX, 0, 0), Vector2.down, groundCheckY, whatIsGround)
            || Physics2D.Raycast(groundCheckPoint.position + new Vector3(-groundCheckX, 0, 0), Vector2.down, groundCheckY, whatIsGround))
        {
            return true;
        }
        else
        {
            return false;
        }
    }

   
    private IEnumerator RechargeStamina()
    {
        yield return new WaitForSeconds(1f);
        while (stamina < maxstamina)
        {
            stamina += chargeRate / 10f;
            if (stamina > maxstamina) stamina = maxstamina;
            Stamina.fillAmount = stamina / maxstamina;
            canrun = true;
            canjump = true;
            yield return new WaitForSeconds(.1f);
        }
    }

    private IEnumerator RechargeShield()
    {
        yield return new WaitForSeconds(1f);
        while (shieldCount < maxShield)
        {
            shieldCount += chargeRate / 10f;
            if (shieldCount > maxShield) shieldCount = maxShield;
            ShieldBar.fillAmount = shieldCount / maxShield;
            yield return new WaitForSeconds(.1f);
        }

    }

}
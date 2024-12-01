using UnityEngine;
using UnityEngine.UI;
public class Enemy : MonoBehaviour
{
    [SerializeField] protected float health;
    [SerializeField] protected float maxHealth;
    [SerializeField] protected float recoilLength;
    [SerializeField] protected float recoilFactor;
    [SerializeField] protected bool isRecoiling = false;

    [SerializeField] protected float speed;
    [SerializeField] protected float damage;
    [SerializeField] protected bool canAttack = false;

    [SerializeField] protected GameObject blood;
    [SerializeField] protected float attackSpeed;
    [SerializeField] public float parrypercent;
    [SerializeField] public float parrymax;

    protected float recoilTimer;
    protected Rigidbody2D rb;
    protected int potionChance;
    protected bool canMove = true;
    protected SpriteRenderer sr;
    [SerializeField] protected Image healthBar;
    [SerializeField] public Image parryBar;
    [SerializeField] protected float stunTimer;
    [SerializeField] protected float attacktimer;
    [SerializeField] public bool parried = false;
    [SerializeField] public float stunduration;
    protected enum EnemyStates
    {
        //Crawler
        Rat_Idle,
        Rat_Flip,
        Rat_Attaking,

        //Bat
        Bat_Chase,
        Bat_Idle,

        //Skeleton
        Skeleton_Idle,
        Skeleton_Chase,
        Skeleton_Attacking,

        //Ghoul
        Ghoul_Idle,
        Ghoul_Chase,
        Ghoul_Attacking,
        Ghoul_Flip,

        //Reaper
        Reaper_Idle,
        Reaper_Chase,
        Reaper_Attacking,

        //Mushroom
        Mushroom_Idle,
        Mushroom_Chase,
        Mushroom_Attacking,

        //Water Princess
        water_idle,
        water_normal_attack,
        water_hard_attack,
        water_chase,

        //Water Princess Phase 2
        water_attack,

        //HUNTRESS
        HT_Chase,
        HT_Idle,

        //WARRIOR BOSS
        warrior_idle,
        warrior_chase,
        warrior_jump,
        warrior_ultimate,
        warrior_attack,

        //Demon_King BOSS
        DK_Idle,
        DK_Stage1,
        DK_Stage2,

        //PALADIN BOSS
        PL_Idle,
        PL_Stage1,
        PL_Stage2,

        //SAMURAI BOSS(easy)
        SM_Idle,
        SM_Chase,

        //BOD BOSS
        BOD_Idle,
        BOD_Chase,

        //WIZARD1
        W1_Idle,
        W1_Attack_Behavior,
        W1_MoveAway,

        //LAPULAPU BOSS
        LP_Idle,
        LP_Chase,
        LP_Attack_Behavior,

        //MAGE CLOSE COMBAT
        MCC_Idle,
        MCC_Chase,
        MCC_MoveAway,
        MCC_Attack_Behavior,

        //SPEAR GIRL BOSS
        SG_Idle,
        SG_Phase1,
        SG_Phase2,

        //BANDIT BOSS

        B_Idle,
        B_Chase,
        B_AttackBehavior,

        //DESERT BOSS
        DB_Idle,
        DB_Chase,
        DB_AttackBehavior,

        //SERVANT ENEMY DIALOGUE
        S_Idle,
        S_Dialogue,
        S_Chase,
        S_AttackBehavior,

        //SERVANTS
        S1_Idle,
        S1_Chase,
        S1_AttackBehavior,

        //servantDIALOGUEMAP
        S2_Idle,
        S2_Chase,
        S2_AttackBehavior,

        //GOLEM BOSS
        G_Idle,
        G_Chase,
        G_Roll,
        G_AttackBehavior,

        //archer enemy
        ARCH_Idle,
        ARCH_Attack,

        //mactan archer
        MA_Idle,
        MA_Attack,

        //HUGE KNIGHTS
        HK_Idle,
        HK_Chase,
        HK_AttackBehavior,

        //MAGELLAN
        MG_IDLE,
        MG_CHASE,
        MG_ATTACKBEHAVIOR,
        MG_TRANSFORM,
        MG_E_IDLE,
        MG_E_CHASE,
        MG_E_ATTACKBEHAVIOR,

    }

    protected EnemyStates currentEnemyStates;

    protected virtual void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
    }
    protected virtual void Update()
    {
        attacktimer += Time.deltaTime;
        if (parried)
        {
            stunTimer += Time.deltaTime;
        }
        if (stunTimer > stunduration)
        {
            parrypercent = parrymax;
            parryBar.fillAmount = parrymax;
            parried = false;
            stunTimer = 0;
            canAttack = true;
            canMove = true;
        }
        if (isRecoiling)
        {
            if (recoilTimer < recoilLength)
            {
                recoilTimer += Time.deltaTime;
                
            }
            else
            {
                isRecoiling = false;
                recoilTimer = 0;
            }
        }
        else
        {
            UpdateEnemyStates();
        }
    }
    protected virtual void UpdateEnemyStates() { }

    

    protected void ChangeStates(EnemyStates _newStates)
    {
        currentEnemyStates = _newStates;
        
    }

    public virtual void EnemyHit(float _damageDone, Vector2 _hitDirection, float _hitForce)
    {       

        print("taking damage");
        health -= _damageDone;
        healthBar.fillAmount = health / maxHealth;
        if (!isRecoiling)
        {            
            GameObject _enemyBlood = Instantiate(blood, transform.position, Quaternion.identity);
            Destroy(_enemyBlood, 5.5f);
            rb.velocity = _hitForce * recoilFactor * _hitDirection;
            isRecoiling = true;
        }
    }
    protected void OnTriggerStay2D(Collider2D _other)
    {
        
        if (_other.CompareTag("Player") && !isRecoiling)
        {
            if (attacktimer > attackSpeed)
            {
                Attack();
            }
            
        }              
    }


    protected virtual void Attack()
    {
        //isparried();
        if (PlayerController.Instance.pState.parry) //pede naman pala ilagay dito to mas mabilis ma check kung naka parry ba
        {
            attacktimer = 0;
            parrypercent -= 15;
            parryBar.fillAmount = parrypercent / parrymax;

            PlayerController.Instance.TakeDamage(damage);
            if (parrypercent <= 0)
            {
                parried = true;
                canAttack = false;
            }
        }
        else if (canAttack && !PlayerController.Instance.pState.blocking && !PlayerController.Instance.pState.invincible && !PlayerController.Instance.pState.parry)
        {
            PlayerController.Instance.TakeDamage(damage);
            PlayerController.Instance.HitStopTime(0, 5, 0.2f);
            attacktimer = 0;
        }
        else if (canAttack && PlayerController.Instance.pState.blocking && !PlayerController.Instance.pState.invincible && !PlayerController.Instance.pState.parry)
        {           
            PlayerController.Instance.TakeDamage(damage);
            attacktimer = 0;
        }

    }
}
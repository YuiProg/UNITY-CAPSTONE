using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class WARRIOR : Enemy
{
    // Start is called before the first frame update
    Vector2 spawnPoint;
    public bool isAlive = true;
    Animator anim;
    [SerializeField] float jumpHeight;
    [SerializeField] float chasedistance;
    [SerializeField] float jumptimer;
    [SerializeField] float ultiTimer;

    [SerializeField] float verticalBounce = 5f;

    [SerializeField] public GameObject bullet;
    [SerializeField] public Transform bulletpos;
    [SerializeField] public GameObject teleportFX;
    [SerializeField] public GameObject swordhitFX;


    //BORDER
    [SerializeField] GameObject Border_L;
    [SerializeField] GameObject Border_R;
    float aliveTimer;
    bool spottedPlayer = false;
    bool isulti = false;
    bool canjump = false;
    bool canUlti = false;
    [SerializeField] GameObject healthBarUI;

    protected override void Start()
    {
        base.Start();
        anim = GetComponent<Animator>();
        spawnPoint = transform.position;
        ChangeStates(EnemyStates.warrior_idle);
        canAttack = true;
        rb.gravityScale = 12f;
        Border_L.SetActive(false);
        Border_R.SetActive(false);
        isAlive = true;
    }

    private void Awake()
    {
        loadCheck();
    }
    void loadCheck()
    {
        if (PlayerPrefs.GetInt("WARRIOR") == 1)
        {
            print("WARRIOR DISABLED");
            gameObject.SetActive(false);
            healthBarUI.SetActive(false);
            Border_L.SetActive(false);
            Border_R.SetActive(false);
           
        }
        else if (PlayerPrefs.GetInt("WARRIOR") == 0)
        {
            gameObject.SetActive(true);
            print("WARRIOR IS ALIVE");
        }
    }
    protected override void UpdateEnemyStates()
    {
        float _dist = Vector2.Distance(transform.position, PlayerController.Instance.transform.position);
        jumptimer += Time.deltaTime;
        ultiTimer += Time.deltaTime;
        if (spottedPlayer)
        {
            healthBarUI.SetActive(true);
            Border_L.SetActive(true);
            Border_R.SetActive(true);
        }        
        else
        {
            healthBarUI.SetActive(false);
        }
        if (isAlive)
        {
            PlayerPrefs.SetInt("WARRIOR", 0);
        }
        else
        {
            PlayerPrefs.SetInt("WARRIOR", 1);
            print("WARRIOR IS DEAD");
            
        }
        if (_dist < chasedistance)
        {
            spottedPlayer = true;
        }
        if (parried)
        {
            canMove = false;
        }
        if (!PlayerController.Instance.pState.isAlive)
        {
            transform.position = spawnPoint;
            health = maxHealth;
            parrypercent = parrymax;
            parryBar.fillAmount = parrymax;
            spottedPlayer = false;
            Border_L.SetActive(false);
            Border_R.SetActive(false);
            ChangeStates(EnemyStates.warrior_jump);
        }
        if (isulti)
        {
            anim.SetBool("Ultimate", true);
        }
        else
        {
            anim.SetBool("Ultimate", false);
        }
        if (jumptimer > 3)
        {
            jumptimer = 0;
            canjump = true;
        }
        if (ultiTimer > 8)
        {
            ultiTimer = 0;
            canUlti = true;
        }
        if (isAlive == false)
        {
            PlayerController.Instance.maxHealth += 0.03f;
            PlayerController.Instance.maxstamina += 0.03f;
            PlayerController.Instance.health = PlayerController.Instance.maxHealth;
            PlayerController.Instance.HealthBar.fillAmount = PlayerController.Instance.health;
        }
        if (health <= 0)
        {
            aliveTimer += Time.deltaTime;
            isAlive = false;
            healthBarUI.SetActive(false);            
            canMove = false;
            canAttack = false;
            Border_L.SetActive(false);
            Border_R.SetActive(false);
            anim.SetBool("Chase", false);
            anim.SetTrigger("Death");
            if (aliveTimer > 5)
            {
                Save.instance.saveData();
                gameObject.SetActive(false);
            }
        }
        else
        {
            if (canMove && spottedPlayer && PlayerController.Instance.pState.isAlive)
            {
                switch (currentEnemyStates)
                {
                    case EnemyStates.warrior_idle:
                            ChangeStates(EnemyStates.warrior_chase);
                        break;
                    case EnemyStates.warrior_chase:
                        if (canjump)
                        {
                            ChangeStates(EnemyStates.warrior_jump);
                        }
                        else if (canUlti)
                        {
                            ChangeStates(EnemyStates.warrior_ultimate);
                        }
                        else if (PlayerController.Instance.takingDamage)
                        {
                            ChangeStates(EnemyStates.warrior_attack);
                        }
                        else
                        {
                            isulti = false;
                            
                            anim.SetBool("Ultimate", false);
                            anim.SetBool("Chase", true);

                            transform.position = Vector2.MoveTowards
                            (transform.position, new Vector2(PlayerController.Instance.transform.position.x, transform.position.y),
                            speed * Time.deltaTime);
                        }
                        Flip();
                        break;
                    case EnemyStates.warrior_jump:
                        StartCoroutine(jumpAttack(1f));
                        ChangeStates(EnemyStates.warrior_chase);
                        break;
                    case EnemyStates.warrior_ultimate:
                        StartCoroutine(Ultimate(1f));
                        ChangeStates(EnemyStates.warrior_chase);
                        break;
                    case EnemyStates.warrior_attack:
                        isRecoiling = true;
                        attackanim();
                        ChangeStates(EnemyStates.warrior_chase);
                        break;
                    default:
                        break;
                }
            }
        }
        
        
    }
    void Flip()
    {
        sr.flipX = PlayerController.Instance.transform.position.x < transform.position.x;
    }
    IEnumerator jumpAttack(float time)
    {
        canMove = false;
        float distancefromplayer = PlayerController.Instance.transform.position.x - transform.position.x;
        anim.SetTrigger("Jump");
        GameObject _enemyBlood = Instantiate(teleportFX, transform.position, Quaternion.identity);
        Destroy(_enemyBlood, 5.5f);
        CameraShake.Instance.ShakeCamera();
        rb.AddForce(new Vector2(distancefromplayer, jumpHeight), ForceMode2D.Impulse);
        Flip();
        yield return new WaitForSeconds(time);
        canjump = false;
        canMove = true;
        
    }

    

    IEnumerator Ultimate(float time)
    {
        isulti = true;
        canMove = false;
        float distancefromplayer = PlayerController.Instance.transform.position.x - transform.position.x;
        rb.AddForce(new Vector2(0, verticalBounce), ForceMode2D.Impulse);
        Flip();
        GameObject _enemyBlood = Instantiate(teleportFX, transform.position, Quaternion.identity);
        Destroy(_enemyBlood, 5.5f);
        yield return new WaitForSeconds(time / 2);
        rb.velocity = Vector2.zero;
        rb.gravityScale = 0;
        shoot();
        yield return new WaitForSeconds(time / 2 + 0.3f);
        rb.gravityScale = 12;
        GameObject tp = Instantiate(teleportFX, transform.position, Quaternion.identity);
        Destroy(tp, 5.5f);
        rb.MovePosition(new Vector2(transform.position.x, PlayerController.Instance.transform.position.x));
        canMove = true;
        canUlti = false;
        GameObject _enemyBloo = Instantiate(teleportFX, transform.position, Quaternion.identity);
        Destroy(_enemyBloo, 5.5f);
        if (canMove)
        {
            ChangeStates(EnemyStates.warrior_chase);
        }
    }

    void attackanim()
    {
        if (!isulti && Vector2.Distance(transform.position, PlayerController.Instance.transform.position) <= 5f)
        {
            GameObject swordFX = Instantiate(swordhitFX, transform.position, Quaternion.identity);
            Destroy(swordFX, 5.5f);
            anim.SetTrigger("Attack");
        }
    }
    void shoot()
    {
        Instantiate(bullet, bulletpos.position, Quaternion.identity);
    }

}
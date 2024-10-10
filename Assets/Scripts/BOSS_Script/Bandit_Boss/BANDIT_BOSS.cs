using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BANDIT_BOSS : Enemy
{
    [SerializeField] public GameObject bullet;
    [SerializeField] public GameObject bullet2;
    [SerializeField] public Transform bulletpos;
    [SerializeField] GameObject HealthBar;
    [SerializeField] AudioSource music;
    
    public float chaseDistance;
    public float jumpHeight;
    bool attacking = false;
    public bool spottedPlayer = false;
    float throwTimer;
    Animator anim;



    public static BANDIT_BOSS instance;
    protected override void Start()
    {
        base.Start();
        rb.gravityScale = 12f;
        HealthBar.SetActive(false);
        canAttack = true;
        canMove = true;
        anim = GetComponent<Animator>();
        ChangeStates(EnemyStates.B_Idle);
    }

    private void Awake()
    {
        loadChecker();
        if (instance != this && instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
        }
    }

    void loadChecker()
    {
        int ActiveCheck = PlayerPrefs.GetInt("Bandit");
        if (ActiveCheck == 1)
        {
            HealthBar.SetActive(false);
            gameObject.SetActive(false);
        }
    }
    protected override void UpdateEnemyStates()
    {
        throwTimer += Time.deltaTime;
        float _dist = Vector2.Distance(transform.position, PlayerController.Instance.transform.position);
        checker();
        if (canMove && PlayerController.Instance.pState.isAlive)
        {
            switch (currentEnemyStates)
            {                
                case EnemyStates.B_Idle:
                    if (_dist < chaseDistance)
                    {
                        spottedPlayer = true;
                        ChangeStates(EnemyStates.B_Chase);
                    }
                    break;
                case EnemyStates.B_Chase:
                    
                    if (throwTimer > 8)
                    {
                        throwTimer = 0;
                        StartCoroutine(Throw());
                        return;
                    }
                    else
                    {
                        DistanceCheck();
                    }
                    break;
                case EnemyStates.B_AttackBehavior:
                    AttackBehavior();            
                    break;
                default:
                    break;
            }
        }
        else if (!PlayerController.Instance.pState.isAlive)
        {
            anim.SetBool("Run", false);
        }
    }
    bool MusicPlaying = false;
    void checker()
    {
        canMove = !parried;
        if (!attacking && spottedPlayer && health > 0)
        {
            flip();
        }
        if (spottedPlayer)
        {
            HealthBar.SetActive(true);
            if (!MusicPlaying)
            {
                MusicPlaying = true;
                music.Play();
                return;
            }
            
        }
        if (health <= 0)
        {
            HealthBar.SetActive(false);
            anim.SetTrigger("Dead");
            spottedPlayer = false;
            canMove = false;
            canAttack = false;
            rb.drag = 1000;
            music.volume -= Time.deltaTime;
            PlayerPrefs.SetInt("Bandit", 1);
            Destroy(gameObject, 5f);
        }
        if (parried)
        {

            anim.SetBool("Parried", true);
        }
        else
        {
            anim.SetBool("Parried", false);
        }
        
    }
    
    bool inRange()
    {
        float _dist = Vector2.Distance(transform.position, PlayerController.Instance.transform.position);
        return _dist <= 3f;
    }
    void resetAllAttacks()
    {
        StopCoroutine(Attack1());
        StopCoroutine(Attack2());
        StopCoroutine(Attack3());
    }
    void DistanceCheck()
    {
        if (!attacking && canMove)
        {
            if (canMove && inRange())
            {
                Debug.Log("ATTACKING");
                ChangeStates(EnemyStates.B_AttackBehavior);
            }
            else if(canMove && !inRange())
            {
                resetAllAttacks();
                anim.SetBool("Run", true);
                transform.position = Vector2.MoveTowards(transform.position,
                new Vector2(PlayerController.Instance.transform.position.x, transform.position.y), speed * Time.deltaTime);
            }
        }
        else
        {
            return;
        }
    }
    

    void AttackBehavior()
    {
        int random = Random.Range(0,5);
        if (!attacking && !parried)
        {
            switch (random)
            {
                case 0:
                    StartCoroutine(JumpProjectile());
                    break;
                case 1:
                    StartCoroutine(Attack1());
                    break;
                case 2:
                    StartCoroutine(Attack2());
                    break;
                case 3:
                    StartCoroutine(Attack3());
                    break;
                case 4:
                    StartCoroutine(GroundSlam());
                    break;
                default:
                    break;
            }
        }
        
    }

    //ATTACKS
    IEnumerator JumpProjectile()
    {
        if (lookingRight)
        {
            Debug.Log("JUMPING");
            float distancefromplayer = PlayerController.Instance.transform.position.x - transform.position.x;
            anim.SetBool("Run", false);
            anim.SetTrigger("Jump_Projectile");
            attacking = true;
            canMove = false;
            yield return new WaitForSeconds(0.1f);
            rb.AddForce(new Vector2(-distancefromplayer - 10, jumpHeight), ForceMode2D.Impulse);
            yield return new WaitForSeconds(0.4f);
            Projectile2();
            yield return new WaitForSeconds(1f);
            canMove = true;
            attacking = false;
            ChangeStates(EnemyStates.B_Idle);
        }
        else if (lookingLeft)
        {
            Debug.Log("JUMPING");
            float distancefromplayer = PlayerController.Instance.transform.position.x - transform.position.x;
            anim.SetBool("Run", false);
            anim.SetTrigger("Jump_Projectile");
            attacking = true;
            canMove = false;
            yield return new WaitForSeconds(0.1f);
            rb.AddForce(new Vector2(-distancefromplayer + 10, jumpHeight), ForceMode2D.Impulse);
            yield return new WaitForSeconds(0.4f);
            Projectile2();
            yield return new WaitForSeconds(1f);
            canMove = true;
            attacking = false;
            ChangeStates(EnemyStates.B_Idle);
        }   
    }

    IEnumerator Throw()
    {
        attacking = true;
        canMove = false;
        anim.SetBool("Run", false);
        anim.SetTrigger("Throw");
        yield return new WaitForSeconds(0.5f);
        Projectile();
        attacking = false;
        canMove = true;
        ChangeStates(EnemyStates.B_Idle);
    }
    IEnumerator Attack1()
    {
        attacking = true;
        canMove = false;
        anim.SetBool("Run", false);
        anim.SetTrigger("Attack1");
        yield return new WaitForSeconds(0.7f);
        attacking = false;
        canMove = true;
        ChangeStates(EnemyStates.B_Idle);
    }

    IEnumerator Attack2()
    {
        attacking = true;
        canMove = false;
        anim.SetBool("Run", false);
        anim.SetTrigger("Attack2");
        yield return new WaitForSeconds(0.9f);
        attacking = false;
        canMove = true;
        ChangeStates(EnemyStates.B_Idle);
    }

    IEnumerator Attack3()
    {
        attacking = true;
        canMove = false;
        anim.SetBool("Run", false);
        anim.SetTrigger("Attack3");
        yield return new WaitForSeconds(1.8f);
        attacking = false;
        canMove = true;
        ChangeStates(EnemyStates.B_Idle);
    }
    IEnumerator GroundSlam()
    {
        attacking = true;
        canMove = false;
        anim.SetBool("Run", false);
        anim.SetTrigger("GroundAttack");
        yield return new WaitForSeconds(2f);
        attacking = false;
        canMove = true;
        ChangeStates(EnemyStates.B_Idle);
    }

    public bool lookingLeft = false;
    public bool lookingRight = false;
    void flip()
    {
        if (PlayerController.Instance.transform.position.x > transform.position.x)
        {
            transform.eulerAngles = new Vector3(0, 0, 0);
            lookingLeft = false;
            lookingRight = true;
        }
        else
        {
            transform.eulerAngles = new Vector3(0, 180, 0);
            lookingLeft = true;
            lookingRight = false;
        }
    }
    void Projectile()
    {
        Instantiate(bullet, bulletpos.position, Quaternion.identity);
    }
    void Projectile2()
    {
        Instantiate(bullet2, bulletpos.position, Quaternion.identity);
    }
}

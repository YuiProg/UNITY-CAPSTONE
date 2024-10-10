using System.Collections;
using System.Collections.Generic;
using System.Net;
using Unity.VisualScripting;
using UnityEngine;

public class Water_Boss : Enemy
{
    [SerializeField] private float chaseDistance;
    [SerializeField] private bool detectedplayer = false;
    Animator anim;

    [SerializeField] public GameObject bullet;
    [SerializeField] public Transform bulletpos;
    [SerializeField] public float AttackDistance;
    
    public float timer;
    public float secondtimer;
    Vector2 spawnpoint;
    
    public bool isAlive = true;
    bool secondphase = false;
    float timerlundge;
    float stuntimer;
    [SerializeField] protected GameObject secondphaseFX;
    [SerializeField] protected GameObject parryFX;
    protected override void Start()
    {
        base.Start();
        canAttack = true;
        anim = GetComponent<Animator>();
        rb.gravityScale = 12f;
        ChangeStates(EnemyStates.water_idle);
        spawnpoint = transform.position;
    }

    // Update is called once per frame
    protected override void UpdateEnemyStates()
    {
        float _dist = Vector2.Distance(transform.position, PlayerController.Instance.transform.position);
        if (parried)
        {
            canMove = false;
            GameObject _enemyBlood = Instantiate(parryFX, transform.position, Quaternion.identity);
            Destroy(_enemyBlood, 5.5f);
        }
        if (!PlayerController.Instance.pState.isAlive)
        {
            transform.position = spawnpoint;
            health = maxHealth;
            parrypercent = parrymax;
            parryBar.fillAmount = parrymax;
            ChangeStates(EnemyStates.water_idle);

        }
        if (health <= 0)
        {
            anim.SetTrigger("Death");
            Destroy(gameObject, 2f);
            canAttack = false;
            isAlive = false;
            canMove = false;
            GameObject destroy = GameObject.Find("AudioTrigger_Water_Boss");
            Destroy(destroy);
        }
        if (health < maxHealth / 2)
        {
            secondphase = true;
            secondPhase();

        }
        else
        {
            firstPhase();             
        }
                               
    }
    void Flip()
    {
        sr.flipX = PlayerController.Instance.transform.position.x < transform.position.x;
    }
    void secondPhase()
    {
        
        if (canMove && secondphase)
        {
            rb.drag = 1000;
            secondtimer += Time.deltaTime;
            if (secondtimer > 3)
            {
                rb.drag = 0;                               
                GameObject _enemyBlood = Instantiate(secondphaseFX, transform.position, Quaternion.identity);
                Destroy(_enemyBlood, 5.5f);
                float _dist = Vector2.Distance(transform.position, PlayerController.Instance.transform.position);
                switch (currentEnemyStates)
                {
                    case EnemyStates.water_idle:
                        if (_dist < chaseDistance)
                        {
                            ChangeStates(EnemyStates.water_chase);
                        }

                        break;
                    case EnemyStates.water_normal_attack:
                        anim.SetTrigger("Normal_Attack");
                        anim.SetBool("Chase", false);
                        ChangeStates(EnemyStates.water_idle);
                        break;
                    case EnemyStates.water_hard_attack:
                        anim.SetTrigger("Hard_Attack");
                        anim.SetBool("Chase", false);
                        ChangeStates(EnemyStates.water_idle);
                        break;
                    case EnemyStates.water_chase:
                        anim.SetBool("Chase", true);

                        transform.position = Vector2.MoveTowards
                        (transform.position, new Vector2(PlayerController.Instance.transform.position.x, transform.position.y),
                        speed * Time.deltaTime);
                        if (PlayerController.Instance.takingDamage)
                        {
                            randomattack();
                        }
                        Flip();
                        break;
                    default:
                        break;
                }
            }
        }              
        
    }
    void randomattack()
    {
        int randomattack = Random.Range(0, 2);

        switch (randomattack)
        {
            case 0:
                ChangeStates(EnemyStates.water_hard_attack);
                break;
            case 1:
                ChangeStates(EnemyStates.water_normal_attack);
                break;
            default:
                break;
        }
    }

    void shoot()
    {
        Instantiate(bullet, bulletpos.position, Quaternion.identity);
    }
    void firstPhase()
    {
        float _dist = Vector2.Distance(transform.position, PlayerController.Instance.transform.position);
        Flip();
        timer += Time.deltaTime;

        if (_dist < chaseDistance && timer > 1)
        {
            rb.drag = 1000;
            anim.SetTrigger("Projectile");
            timer = 0;
            shoot();
        }
    }
    
}

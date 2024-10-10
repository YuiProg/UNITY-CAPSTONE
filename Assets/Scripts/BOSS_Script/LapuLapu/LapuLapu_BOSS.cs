using System.Collections;
using System.Collections.Generic;
using System.Net.Http.Headers;
using UnityEngine;
using UnityEngine.UI;

public class LapuLapu_BOSS : Enemy
{

    [SerializeField] GameObject HealthBar;
    public float chaseDistance;
    bool spottedPlayer = false;
    Vector2 spawnPoint;
    Animator anim;
    [SerializeField] GameObject Music;
    protected override void Start()
    {
        base.Start();
        anim = GetComponent<Animator>();
        canMove = true;
        canAttack = true;
        rb.gravityScale = 12f;
        spawnPoint = transform.position;
        ChangeStates(EnemyStates.LP_Idle);
        HealthBar.SetActive(false);
    }

    private void Awake()
    {
        loadcheck();
    }

    void loadcheck()
    {
        if (PlayerPrefs.GetInt("LapuLapu") == 1)
        {
            gameObject.SetActive(false);
        }
        else if (PlayerPrefs.GetInt("LapuLapu") == 0)
        {
            gameObject.SetActive(true);
        }
    }
    protected override void UpdateEnemyStates()
    {
        float _dist = Vector2.Distance(PlayerController.Instance.transform.position, transform.position);
        if (!attacking)
        {
            flip();
        }
        if (spottedPlayer)
        {
            HealthBar.SetActive(true);
            Music.SetActive(true);
        }
        if (parried)
        {
            canMove = false;
            canAttack = false;
            anim.SetBool("Stagger", true);
        }
        else
        {
            canMove = true;
            canAttack = true;
            anim.SetBool("Stagger", false);
        }
        if (!PlayerController.Instance.pState.isAlive)
        {
            transform.position = spawnPoint;
            health = maxHealth;
            parrypercent = parrymax;
            parryBar.fillAmount = parrymax;
            spottedPlayer = false;
            Music.SetActive(false);
            HealthBar.SetActive(false);
            ChangeStates(EnemyStates.LP_Idle);
            return;
        }
        if (health <= 0)
        {
            PlayerPrefs.SetInt("LapuLapu", 1);
            anim.SetTrigger("Dead");
            HealthBar.SetActive(false);
            Destroy(gameObject, 2f);
            return;
        }
        if (canMove)
        {
            switch (currentEnemyStates)
            {
                case EnemyStates.LP_Idle:
                    if (_dist < chaseDistance)
                    {
                        spottedPlayer = true;
                        ChangeStates(EnemyStates.LP_Chase);
                    }
                    break;
                case EnemyStates.LP_Chase:
                    if (!parried && Vector2.Distance(PlayerController.Instance.transform.position, transform.position) >= 3f)
                    {
                        anim.SetBool("Walk", true);
                        transform.position = Vector2.MoveTowards
                        (transform.position, new Vector2(PlayerController.Instance.transform.position.x, transform.position.y),
                        speed * Time.deltaTime);
                    }
                    else if (!parried && Vector2.Distance(PlayerController.Instance.transform.position, transform.position) <= 3f)
                    {
                        anim.SetBool("Walk", false);
                        ChangeStates(EnemyStates.LP_Attack_Behavior);
                    }
                    break;
                case EnemyStates.LP_Attack_Behavior:
                    AttackBehavior();
                    break;
                default:
                    break;
            }
        }
        
    }

    void AttackBehavior()
    {
        int RAttacks = Random.Range(0,4);
        if (!attacking)
        {
            switch (RAttacks)
            {
                case 0:
                    StartCoroutine(Attack1());
                    break;
                case 1:
                    StartCoroutine(Attack2());
                    break;
                case 2:
                    StartCoroutine(Attack3());
                    break;
                case 3:
                    StartCoroutine(ComboAttack());
                    break;
                default:
                    break;
            }
        }       
    }
    bool attacking = false;
    IEnumerator Attack1()
    {
        attacking = true;
        Debug.Log("ATTACK 1");
        canMove = false;
        anim.SetTrigger("Attack1");
        yield return new WaitForSeconds(1f);
        canMove = true;
        attacking = false;
        ChangeStates(EnemyStates.LP_Idle);
    }

    IEnumerator Attack2()
    {
        attacking = true;
        Debug.Log("ATTACK 2");
        canMove = false;
        anim.SetTrigger("Attack2");
        yield return new WaitForSeconds(1f);
        canMove = true;
        attacking = false;
        ChangeStates(EnemyStates.LP_Idle);
    }

    IEnumerator Attack3()
    {
        attacking = true;
        Debug.Log("ATTACK 3");
        canMove = false;
        anim.SetTrigger("Attack3");
        yield return new WaitForSeconds(2f);
        canMove = true;
        attacking = false;
        ChangeStates(EnemyStates.LP_Idle);
    }
    IEnumerator ComboAttack()
    {
        attacking = true;
        Debug.Log("COMBO ATTACK");
        canMove = false;
        anim.SetTrigger("Combo");
        yield return new WaitForSeconds(2f);
        canMove = true;
        attacking = false;
        ChangeStates(EnemyStates.LP_Idle);
    }



    void flip()
    {
        if (PlayerController.Instance.transform.position.x > transform.position.x)
        {
            transform.eulerAngles = new Vector3(0, 0, 0);
            healthBar.transform.eulerAngles = new Vector3(0, 0, 0);
        }
        else
        {
            healthBar.transform.eulerAngles = new Vector3(0, 0, 0);
            transform.eulerAngles = new Vector3(0, 180, 0);
        }
    }
}

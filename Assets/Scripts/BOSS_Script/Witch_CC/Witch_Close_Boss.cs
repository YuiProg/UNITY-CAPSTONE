using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Witch_Close_Boss : Enemy
{
    // Start is called before the first frame update
    Vector2 spawnPoint;
    Animator anim;
    bool spottedPlayer = false;
    bool attacking = false;
    bool runningAway = false;
    [SerializeField] float chaseDistance;
    [SerializeField] Transform moveHere;
    [SerializeField] GameObject healthBAR;
    [SerializeField] GameObject wallLeft;
    [SerializeField] GameObject wallRight;
    [SerializeField] GameObject MUSIC;
    protected override void Start()
    {
        base.Start();
        healthBAR.SetActive(false);
        wallLeft.SetActive(false);
        wallRight.SetActive(false);
        MUSIC.SetActive(false);
        anim = GetComponent<Animator>();
        canAttack = true;
        spawnPoint = transform.position;
        canMove = true;
        rb.gravityScale = 12f;
        ChangeStates(EnemyStates.MCC_Idle);
    }

    protected override void UpdateEnemyStates()
    {
        float _dist = Vector2.Distance(PlayerController.Instance.transform.position, transform.position);
        if (parried)
        {
            canMove = false;
        }
        else
        {
            canMove = true;
        }
        if (spottedPlayer)
        {
            healthBAR.SetActive(true);
            wallLeft.SetActive(true);
            wallRight.SetActive(true);
            MUSIC.SetActive(true);
        }
        if (!attacking && spottedPlayer)
        {
            flip();
        }
        if (!PlayerController.Instance.pState.isAlive)
        {
            spottedPlayer = false;
            healthBAR.SetActive(false);
            health = maxHealth;
            parrymax = parrypercent;
            parryBar.fillAmount = parrypercent;
            wallLeft.SetActive(false);
            MUSIC.SetActive(false);
            wallRight.SetActive(false);
            ChangeStates(EnemyStates.MCC_Idle);
            return;
        }
        if (health <= 0)
        {
            canMove = false;
            canAttack = false;
            healthBAR.SetActive(false);
            anim.SetTrigger("Die");
            wallLeft.SetActive(false);
            wallRight.SetActive(false);
            MUSIC.SetActive(false);
            Destroy(gameObject, 2f);
        }
        if (canMove)
        {
            switch (currentEnemyStates)
            {
                case EnemyStates.MCC_Idle:
                    StopCoroutine(runAway());
                    if (_dist < chaseDistance)
                    {
                        spottedPlayer = true;
                        ChangeStates(EnemyStates.MCC_Chase);
                    }
                    break;
                case EnemyStates.MCC_Chase:
                    if (!parried && _dist <= 3f)
                    {
                        anim.SetBool("Run", false);
                        ChangeStates(EnemyStates.MCC_Attack_Behavior);
                    }
                    else if (!runningAway && !parried && _dist >= 3f)
                    {
                        anim.SetBool("Run", true);
                        transform.position = Vector2.MoveTowards
                        (transform.position, new Vector2(PlayerController.Instance.transform.position.x, transform.position.y),
                        speed * Time.deltaTime);
                    }
                    break;
                case EnemyStates.MCC_MoveAway:
                    runningAway = true;
                    StartCoroutine(runAway());
                    break;
                case EnemyStates.MCC_Attack_Behavior:
                    attackBehavior();
                    break;
                default:
                    break;
            }
        }
        
    }

    void attackBehavior()
    {
        int attacks = Random.Range(0,3);
        if (!attacking && !runningAway)
        {
            switch (attacks)
            {
                case 0:
                    StartCoroutine(Attack1());
                    break;
                case 1:
                    StartCoroutine(Attack2());
                    break;
                case 2:                    
                    ChangeStates(EnemyStates.MCC_MoveAway);
                    break;
                default:
                    break;
            }
        }
        
    }

    IEnumerator runAway()
    {
        if (runningAway)
        {
            if (lookingLeft && runningAway)
            {
                Debug.Log("RUNNING AWAY");
                sr.flipX = PlayerController.Instance.transform.position.x < transform.position.x;
                anim.SetBool("Run", true);
                transform.position = Vector2.MoveTowards(transform.position, moveHere.transform.position, speed * Time.deltaTime);
                yield return new WaitForSeconds(1f);
                anim.SetBool("Run", false);
                sr.flipX = PlayerController.Instance.transform.position.x > transform.position.x;
                runningAway = false;
                
            }
            else if (lookingRight && runningAway)
            {
                Debug.Log("RUNNING AWAY");
                sr.flipX = PlayerController.Instance.transform.position.x > transform.position.x;
                anim.SetBool("Run", true);
                transform.position = Vector2.MoveTowards(transform.position, moveHere.transform.position, speed * Time.deltaTime);
                yield return new WaitForSeconds(1f);
                anim.SetBool("Run", false);
                sr.flipX = PlayerController.Instance.transform.position.x < transform.position.x;
                runningAway = false;
            }
            flip();
            ChangeStates(EnemyStates.MCC_Idle);
        }      
    }
    IEnumerator Attack1()
    {
        attacking = true;
        canMove = false;
        anim.SetTrigger("Attack1");
        yield return new WaitForSeconds(1f);
        canMove = true;
        attacking = false;
        ChangeStates(EnemyStates.MCC_Idle);
    }

    IEnumerator Attack2()
    {
        attacking = true;
        canMove = false;
        anim.SetTrigger("Attack2");
        yield return new WaitForSeconds(1f);
        canMove = true;
        attacking = false;
        ChangeStates(EnemyStates.MCC_Idle);
    }

    public bool lookingRight = false;
    public bool lookingLeft = false;
    void flip()
    {
        if (PlayerController.Instance.transform.position.x > transform.position.x)
        {
            transform.eulerAngles = new Vector3(0, 0, 0);
            parryBar.transform.eulerAngles = new Vector3(0, 0, 0);
            lookingLeft = false;
            lookingRight = true;
        }
        else
        {
            transform.eulerAngles = new Vector3(0, 180, 0);
            parryBar.transform.eulerAngles = new Vector3(0, 0, 0);
            lookingRight = false;
            lookingLeft = true;
        }
    }
}


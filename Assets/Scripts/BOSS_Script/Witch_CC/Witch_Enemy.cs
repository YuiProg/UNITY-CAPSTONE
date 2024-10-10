using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Witch_Enemy : Enemy
{
    Vector2 spawnPoint;
    bool spottedPlayer = false;
    bool runningAway = false;
    [SerializeField]float chaseDistance;
    Animator anim;

    [SerializeField] Transform moveHere;

    protected override void Start()
    {
        base.Start();
        canAttack = true;
        spawnPoint = transform.position;
        rb.gravityScale = 12f;
        anim = GetComponent<Animator>();
        ChangeStates(EnemyStates.W1_Idle);
    }
    float time;
    protected override void UpdateEnemyStates()
    {
        float _dist = Vector2.Distance(transform.position, PlayerController.Instance.transform.position);
        if (!runningAway && spottedPlayer)
        {
            flip();
        }
        if (health <= 0)
        {
            time += Time.deltaTime;
            anim.SetTrigger("Dead");
            canMove = false;
            canAttack = false;
            if (time >= 3)
            {
                time = 0;
                gameObject.SetActive(false);
            }
        }
        if (!PlayerController.Instance.pState.isAlive)
        {
            health = maxHealth;
            parrypercent = parrymax;
            spottedPlayer = false;
            ChangeStates(EnemyStates.W1_Idle);
        }
        switch (currentEnemyStates)
        {            
            case EnemyStates.W1_Idle:
                if (_dist < chaseDistance)
                {
                    spottedPlayer = true;
                    ChangeStates(EnemyStates.W1_Attack_Behavior);
                }
                break;
            case EnemyStates.W1_Attack_Behavior:
                attackHandler();
                break;
            case EnemyStates.W1_MoveAway:
                runningAway = true;                
                StartCoroutine(MoveAway());
                break;
            default:
                break;
        }
    }
    public bool rangeProcess = false;
    
    void attackHandler()
    {
        if (spottedPlayer)
        {
            if (canMove && Vector2.Distance(transform.position, PlayerController.Instance.transform.position) <= 5f)
            {
                int attackrandom = Random.Range(0,3);
                anim.SetBool("Walk", false);
                switch (attackrandom)
                {
                    case 0:
                        ChangeStates(EnemyStates.W1_MoveAway);
                        break;
                    case 1:
                        StartCoroutine(Attack2());
                        break;
                    case 2:                        
                        StartCoroutine(Attack1());
                        break;
                    default:
                        break;
                }
            }
            else if (canMove && Vector2.Distance(transform.position, PlayerController.Instance.transform.position) >= 5f)
            {
                int randomSpell = Random.Range(0,2);
                anim.SetBool("Walk", false);
                switch (randomSpell)
                {
                    case 0:
                        StartCoroutine(FirstSpell());
                        break;
                    case 1:
                        StartCoroutine(SecondSpell());
                        break;
                    default:
                        break;
                }
            }          
            
        }
    }
    
    IEnumerator FirstSpell()
    {
        Debug.Log("Spell 1");
        anim.SetTrigger("Spell1");
        canMove = false;
        yield return new WaitForSeconds(2f);
        canMove = true;
        ChangeStates(EnemyStates.W1_Idle);
    }

    IEnumerator SecondSpell()
    {
        Debug.Log("Spell 2");
        anim.SetTrigger("Spell2");      
        canMove = false;
        yield return new WaitForSeconds(2f);
        canMove = true;
        ChangeStates(EnemyStates.W1_Idle);
    }

    IEnumerator MoveAway()
    {
        if (runningAway)
        {
            if (lookingLeft)
            {
                anim.SetBool("Run", true);
                Debug.Log("RUNNING AWAY LEFT");
                sr.flipX = PlayerController.Instance.transform.position.x < transform.position.x;
                canMove = true;
                transform.position = Vector2.MoveTowards
                (transform.position, moveHere.transform.position,
                speed * Time.deltaTime);
                yield return new WaitForSeconds(1f);
                sr.flipX = PlayerController.Instance.transform.position.x > transform.position.x;
                runningAway = false;
                anim.SetBool("Run", false);
                ChangeStates(EnemyStates.W1_Attack_Behavior);
            }
            else if (lookingRight)
            {
                anim.SetBool("Run", true);
                Debug.Log("RUNNING AWAY RIGHT");
                sr.flipX = PlayerController.Instance.transform.position.x > transform.position.x;
                canMove = true;
                transform.position = Vector2.MoveTowards
                (transform.position, moveHere.transform.position,
                speed * Time.deltaTime);
                yield return new WaitForSeconds(1f);
                sr.flipX = PlayerController.Instance.transform.position.x < transform.position.x;
                runningAway = false;
                anim.SetBool("Run", false);
                ChangeStates(EnemyStates.W1_Attack_Behavior);
            }
            
        }
       
    }

    IEnumerator Attack1()
    {
        if (!runningAway)
        {
            Debug.Log("Attack1");
            canMove = false;
            anim.SetTrigger("Attack1");
            yield return new WaitForSeconds(2f);
            canMove = true;
            ChangeStates(EnemyStates.W1_Attack_Behavior);
        }
    }

    IEnumerator Attack2()
    {
        Debug.Log("Attack2");
        canMove = false;
        anim.SetTrigger("Attack2");
        yield return new WaitForSeconds(2f);
        canMove = true;
        ChangeStates(EnemyStates.W1_Attack_Behavior);
    }

    public bool lookingRight = false;
    public bool lookingLeft = false;
    void flip()
    {
        if (PlayerController.Instance.transform.position.x > transform.position.x)
        {
            transform.eulerAngles = new Vector3(0, 0, 0);
            healthBar.transform.eulerAngles = new Vector3(0,0,0);
            lookingRight = true;
            lookingLeft = false;
        }
        else
        {
            transform.eulerAngles = new Vector3(0, 180, 0);
            healthBar.transform.eulerAngles = new Vector3(0, 0, 0);
            lookingRight = false;
            lookingLeft = true;
        }
    }
}

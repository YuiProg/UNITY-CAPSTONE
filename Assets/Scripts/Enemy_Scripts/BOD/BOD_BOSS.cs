using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class BOD_BOSS : Enemy
{
    [SerializeField] float chaseDistance;    
    [SerializeField] float attackTimer;
    [SerializeField] Transform loc;

    [SerializeField] GameObject Projectile;
    [SerializeField] GameObject ProjLOC;
    [SerializeField] Text status;
    Animator anim;
    Vector2 spawnPoint;
    bool spottedPlayer;
    bool casting = false;
    [SerializeField]float castTimer;
    [SerializeField] bool lookingLeft;
    [SerializeField] bool lookingRight;

    protected override void Start()
    {
        base.Start();
        rb.gravityScale = 12f;
        canAttack = true;
        canMove = true;
        spawnPoint = transform.position;
        anim = GetComponent<Animator>();
        ChangeStates(EnemyStates.BOD_Idle);
    }

    protected override void UpdateEnemyStates()
    {
        flip();
        attackTimer += Time.deltaTime;
        castTimer += Time.deltaTime;
        float _dist = Vector3.Distance(loc.transform.position, PlayerController.Instance.transform.position);
        if (parried)
        {
            status.text = "! PARRIED !";
            canMove = false;
        }
        else
        {
            status.text = " ";
            canMove = true;
        }
        if (health <= 0)
        {
            anim.SetBool("Walk", false);
            anim.SetTrigger("Death");
            Destroy(gameObject, 1.3f);
            return;
        }
        if (!PlayerController.Instance.pState.isAlive)
        {
            transform.position = spawnPoint;
            health = maxHealth;
            healthBar.fillAmount = health / maxHealth;            
        }
        switch (currentEnemyStates)
        {            
            case EnemyStates.BOD_Idle:
                if (_dist < chaseDistance)
                {
                    spottedPlayer = true;
                    ChangeStates(EnemyStates.BOD_Chase);
                }
                break;
            case EnemyStates.BOD_Chase:
                
                if (castTimer > 10)
                {
                    castTimer = 0;
                    casting = true;
                    StartCoroutine(CastSpell());
                }
                else
                {
                    Walk();
                }
                
                break;
            default:
                break;
        }
    }
       
    void Walk()
    {
        if (spottedPlayer && canMove)
        {
            if (!casting && canMove && Vector2.Distance(loc.transform.position, PlayerController.Instance.transform.position) <= 7.3f)
            {
                StartCoroutine(AttackBOD());
                attackTimer = 0;

            }
            else if (!casting && canMove && Vector2.Distance(loc.transform.position, PlayerController.Instance.transform.position) >= 7.3f)
            {
                StopCoroutine(AttackBOD());
                if (attackTimer > 1f)
                {
                    canMove = false;
                    anim.SetBool("Walk", true);
                    transform.position = Vector2.MoveTowards
                    (transform.position, new Vector2(PlayerController.Instance.transform.position.x, loc.transform.position.y),
                    speed * Time.deltaTime);
                }
            }
        }          
    }
    IEnumerator AttackBOD()
    {
        Debug.Log("ATTACKING");
        canMove = false;
        anim.SetBool("Walk", false);
        anim.SetTrigger("Attack");
        yield return new WaitForSeconds(2f);
        canMove = true;
    }
    IEnumerator CastSpell()
    {
        casting = true;
        canMove = false;
        anim.SetBool("Walk", false);
        anim.SetTrigger("Cast");
        yield return new WaitForSeconds(0.8f);
        shoot();
        canMove = true;
        casting = false;
        ChangeStates(EnemyStates.BOD_Idle);

    }
    void shoot()
    {
        Instantiate(Projectile, ProjLOC.transform.position, Quaternion.identity);
    }
    void flip()
    {
        if (PlayerController.Instance.transform.position.x > loc.transform.position.x)
        {
            transform.eulerAngles = new Vector3(0, 180, 0);
            status.transform.eulerAngles = new Vector3(0, 0, 0);
            healthBar.transform.eulerAngles = new Vector3(0, 0, 0);
            lookingRight = true;
            lookingLeft = false;
        }
        else
        {
            transform.eulerAngles = new Vector3(0, 0, 0);
            status.transform.eulerAngles = new Vector3(0, 0, 0);
            healthBar.transform.eulerAngles = new Vector3(0,0,0);
            lookingLeft = true;
            lookingRight = false;
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HUGEKNIGHT : Enemy
{
    public float chaseDistance;
    bool isAttacking = false;
    bool spottedPlayer = false;
    int count;
    Animator anim;

    //border
    [SerializeField] GameObject BORDERR;
    [SerializeField] GameObject BORDERL;

    //drops
    [SerializeField] GameObject ambers;


    //ui
    [SerializeField] GameObject HEALTHBAR;

    protected override void Start()
    {
        base.Start();
        anim = GetComponent<Animator>();
        canMove = true;
        canAttack = true;
        ChangeStates(EnemyStates.HK_Idle);
    }

    private void Awake()
    {
        //loadCheck();
    }

    void loadCheck()
    {
        if (PlayerPrefs.GetInt("HK") == 1)
        {
            Destroy(gameObject);
        }
    }

    protected override void UpdateEnemyStates()
    {
        float dist = Vector2.Distance(transform.position, PlayerController.Instance.transform.position);
        flip(!isAttacking);
        stateCheck();
        if (canMove && PlayerController.Instance.pState.isAlive)
        {
            switch (currentEnemyStates)
            {
                case EnemyStates.HK_Idle:
                    if (dist < chaseDistance)
                    {
                        spottedPlayer = true;
                        ChangeStates(EnemyStates.HK_Chase);
                    }
                    break;
                case EnemyStates.HK_Chase:
                    if (distanceCheck())
                    {
                        ChangeStates(EnemyStates.HK_AttackBehavior);
                    }
                    anim.SetBool("Walk", true);
                    transform.position = Vector3.MoveTowards(transform.position, 
                        new Vector3(PlayerController.Instance.transform.position.x, transform.position.y), speed * Time.deltaTime);

                    break;
                case EnemyStates.HK_AttackBehavior:
                    if (!isAttacking)
                    {
                        StartCoroutine(Attack1());
                    }
                    break;
                default:
                    break;
            }
        }
    }

    void drop()
    {
        if (count <= 10)
        {
            Instantiate(ambers, transform.position, Quaternion.identity);
            count++;
        }
    }
    bool isdead = false;
    bool hasdroppeditems = false;
    void stateCheck()
    {
        BORDERL.SetActive(spottedPlayer);
        BORDERR.SetActive(spottedPlayer);
        HEALTHBAR.SetActive(spottedPlayer);
        canMove = !parried;
        canAttack = PlayerController.Instance.pState.isAlive;
        if (health <= 0)
        {
            HEALTHBAR.SetActive(false);
            BORDERL.SetActive(false);
            BORDERR.SetActive(false);
            isdead = true;
            canMove = false;
            canAttack = false;
            anim.SetTrigger("Death");
            PlayerPrefs.SetInt("HK", 1);
            Destroy(gameObject, 5f);
        }
        if (isdead && !hasdroppeditems)
        {
            drop();
            if (count == 10)
            {
                hasdroppeditems = true;
            }
        }
    }
    bool distanceCheck()
    {
        float dist = Vector2.Distance(transform.position, PlayerController.Instance.transform.position);
        return dist < 4f;
    }

    IEnumerator Attack1()
    {
        anim.SetBool("Walk", false);
        isAttacking = true;
        canMove = false;
        anim.SetTrigger("Attack");
        yield return new WaitForSeconds(1.8f);
        isAttacking = false;
        canMove = true;
        ChangeStates(EnemyStates.HK_Idle);
    }

    void flip(bool canflip)
    {
        if (canflip)
        {
            if (PlayerController.Instance.transform.position.x < transform.position.x)
            {
                transform.eulerAngles = new Vector3(0, 180, 0);
                HEALTHBAR.transform.eulerAngles = new Vector3(0, 0, 0);
            }
            else
            {
                transform.eulerAngles = new Vector3(0, 0, 0);
                HEALTHBAR.transform.eulerAngles = new Vector3(0, 0, 0);
            }
        }
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HUGEKNIGHT : Enemy
{
    public float chaseDistance;
    bool isAttacking = false;
    public bool spottedPlayer = false;
    int count;
    Animator anim;

    //border
    [SerializeField] GameObject BORDERR;
    [SerializeField] GameObject BORDERL;

    //drops
    [SerializeField] GameObject ambers;
    [SerializeField] Transform amberLOC;

    //ui
    [SerializeField] GameObject HEALTHBAR;

    public static HUGEKNIGHT instance;
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
        loadCheck();
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
        }
    }

    void loadCheck()
    {
        if (PlayerPrefs.GetInt("HK") == 1)
        {
            Destroy(gameObject);
            BORDERL.SetActive(false);
            BORDERR.SetActive(false);
        }
    }

    protected override void UpdateEnemyStates()
    {
        float dist = Vector2.Distance(transform.position, PlayerController.Instance.transform.position);
        flip(!isAttacking && health > 0);
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
            Instantiate(ambers, amberLOC.position, Quaternion.identity);
            count++;
        }
    }
    bool isdead = false;
    bool hasdroppeditems = false;
    bool banner = false;
    void stateCheck()
    {
        if (parried) anim.SetTrigger("Parried") ;
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
            spottedPlayer = false;
            anim.SetTrigger("Death");
            PlayerPrefs.SetInt("HK", 1);
            PlayerPrefs.SetString("Quest","Talk to the Tausug Scout");
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
        if (health <= 0 && !banner)
        {
            banner = true;
            PlayerController.Instance.pState.killedABoss = true;
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

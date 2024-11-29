using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GOLEM_BOSS : Enemy
{
    public bool spottedPlayer = false;
    public float chaseDistance;
    public float rollTimer;
    public bool lookingRight;
    public bool lookingLeft;
    public bool canFlip;
    bool isAttacking = false;
    Animator anim;
    //roll
    [SerializeField] Transform rollLeft;
    [SerializeField] Transform rollRight;
    //hp
    [SerializeField] GameObject HEALTHBAR;
    [SerializeField] GameObject BORDER_R;
    [SerializeField] GameObject BORDER_L;
    //essencedrop
    int count;
    [SerializeField] GameObject Essence;
    [SerializeField] Transform EssenceDROP;

    public static GOLEM_BOSS Instance;

    protected override void Start()
    {
        base.Start();
        rb.gravityScale = 12f;
        anim = GetComponent<Animator>();
        canAttack = true;
        canMove = true;
        BORDER_L.SetActive(false);
        BORDER_R.SetActive(false);
        HEALTHBAR.SetActive(false);
        ChangeStates(EnemyStates.G_Idle);
    }

    private void Awake()
    {
        if (PlayerPrefs.GetInt("HERALD GOLEM") == 1)
        {
            HEALTHBAR.SetActive(false);
            BORDER_L.SetActive(false);
            BORDER_R.SetActive(false);
            Destroy(gameObject);     
        }
        if (Instance != this && Instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }
   
    protected override void UpdateEnemyStates()
    {
        rollTimer += Time.deltaTime;
        float distance = Vector2.Distance(transform.position, PlayerController.Instance.transform.position);
        flip();
        stateCheck();

        if (canMove && !isAttacking)
        {
            if (rollTimer > 10f && spottedPlayer)
            {
                rollTimer = 0f;
                anim.SetBool("Run", false);
                ChangeStates(EnemyStates.G_Roll);
            }
            switch (currentEnemyStates)
            {
                case EnemyStates.G_Idle:
                    if (distance < chaseDistance)
                    {
                        spottedPlayer = true;
                        ChangeStates(EnemyStates.G_Chase);
                    }
                    break;
                case EnemyStates.G_Chase:
                    


                    if (distanceCheck())
                    {
                        anim.SetBool("Run", false);
                        ChangeStates(EnemyStates.G_AttackBehavior);
                    }
                    else
                    {
                        anim.SetBool("Run", true);
                        transform.position = Vector2.MoveTowards(transform.position,
                        new Vector2(PlayerController.Instance.transform.position.x, transform.position.y), speed * Time.deltaTime);
                    }               
                    break;
                case EnemyStates.G_Roll:
                    StartCoroutine(Rolling());
                    break;
                case EnemyStates.G_AttackBehavior:
                    if (!isAttacking)
                    {
                        attackBehavior();
                    }
                    break;
                default:
                    break;
            }
        }
    }
    public bool distanceCheck()
    {
        float distance = Vector2.Distance(transform.position, PlayerController.Instance.transform.position);
        return distance < 3f;
    }
    bool isalive = true;
    void stateCheck()
    {
        canMove = !parried;
        canFlip = !isAttacking;
        anim.SetBool("Parried", parried);
        if (spottedPlayer)
        {
            BORDER_L.SetActive(true);
            BORDER_R.SetActive(true);
            HEALTHBAR.SetActive(true);
        }
        if (health <= 0)
        {
            canMove = false;
            canAttack = false;
            anim.SetTrigger("Death");
            PlayerPrefs.SetInt("HERALD GOLEM", 1);
            HEALTHBAR.SetActive(false);
            BORDER_L.SetActive(false);
            BORDER_R.SetActive(false);
            dropE();
            spottedPlayer = false;
            Save.instance.saveData();
            QuestTracker.instance.hasQuest = true;
            PlayerPrefs.SetString("Quest", "Return to the igorot leader");
            PlayerPrefs.SetInt("Mactan", 1);
            dead();
            PlayerController.Instance.pState.killedABoss = true;
            Destroy(gameObject, 2f);
        }
        if (!PlayerController.Instance.pState.isAlive)
        {
            canMove = false;
            canAttack = false;
            HEALTHBAR.SetActive(false);
        }
    }
    void dead()
    {
        if (isalive)
        {
            PlayerPrefs.SetInt("Mactan", 1);
            isalive = false;
            QuestTracker.instance.hasQuest = true;
            PlayerPrefs.SetString("Quest", "Return to the igorot leader");
            PlayerController.Instance.pState.killedABoss = true;
        }
    }
    void dropE()
    {
        if (count != 3)
        {
            count++;
            Instantiate(Essence, EssenceDROP.position, Quaternion.identity);
        }
    }
    void attackBehavior()
    {
        int i = Random.Range(0, 3);
        switch (i)
        {
            case 0:
                StartCoroutine(Attack1());
                break;
            case 1:
                StartCoroutine(Slam());
                break;
            case 2:
                StartCoroutine(Slam3());
                break;
            default:
                break;
        }
    }


    //ATTACKS

    //ETO ROLL NI ZECH HAHAAHA
    IEnumerator Rolling()
    {
        isAttacking = true;
        anim.SetTrigger("Roll");
        damage = 2000f;
        float rollDuration = 3f; 
        float rollSpeed = 15f; 
        float elapsedTime = 0f;
        yield return new WaitForSeconds(.8f);
        Vector2 targetPosition = new Vector2(PlayerController.Instance.transform.position.x, transform.position.y);

        while (elapsedTime < rollDuration)
        {
            CameraShake.Instance.ShakeCamera();
            if (lookingLeft)
            {
                transform.position = Vector2.MoveTowards(transform.position, rollLeft.position, rollSpeed * Time.deltaTime);
            }
            else if (lookingRight)
            {
                transform.position = Vector2.MoveTowards(transform.position, rollRight.position, rollSpeed * Time.deltaTime);
            }
           
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        damage = 6f;
        yield return new WaitForSeconds(2f);
        isAttacking = false;
        ChangeStates(EnemyStates.G_Idle);
    }
    IEnumerator Attack1()
    {
        isAttacking = true;
        anim.SetTrigger("Punch");
        canMove = false;
        yield return new WaitForSeconds(1f);
        isAttacking = false;
        canMove = true;
        ChangeStates(EnemyStates.G_Idle);
    }

    IEnumerator Slam()
    {
        isAttacking = true;
        canMove = false;
        anim.SetTrigger("Slam");
        yield return new WaitForSeconds(1.8f);
        isAttacking = false;
        canMove = true;
        ChangeStates(EnemyStates.G_Idle);
    }
    IEnumerator Slam3()
    {
        isAttacking = true;
        canMove = false;
        anim.SetTrigger("Slam3");
        yield return new WaitForSeconds(2.5f);
        isAttacking = false;
        canMove = true;
        ChangeStates(EnemyStates.G_Idle);
    }
    void flip()
    {
        if (canFlip)
        {
            if (PlayerController.Instance.transform.position.x > transform.position.x)
            {
                transform.eulerAngles = new Vector3(0, 180, 0);
                lookingRight = true;
                lookingLeft = false;
            }
            else
            {
                lookingLeft = true;
                lookingRight = false;
                transform.eulerAngles = new Vector3(0, 0, 0);
            }
        }
        
    }
}

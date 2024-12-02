using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Servant_Enemy : Enemy
{
    bool spottedPlayer = false;
    bool isAttacking = false;
    bool hasTalked = false;
    bool isTalking = false;
    public bool servantscanATK = false;
    public float chaseDistance = 2f;
    Animator anim;
    [SerializeField] Text status;
    [SerializeField] GameObject DLG;
    [SerializeField] Text dialogue;
    [SerializeField] Text npcName;
    public static Servant_Enemy instance;
    protected override void Start()
    {
        base.Start();
        anim = GetComponent<Animator>();
        rb.gravityScale = 12f;
        ChangeStates(EnemyStates.S_Idle);
    }

    private void Awake()
    {
        if (instance == this && instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
        }
    }

    protected override void UpdateEnemyStates()
    {
        float _dist = Vector2.Distance(transform.position, PlayerController.Instance.transform.position);
        flip();
        stateCheck();
        if (canMove && !isTalking)
        {
            switch (currentEnemyStates)
            {
                case EnemyStates.S_Idle:
                    if (_dist < chaseDistance)
                    {
                        spottedPlayer = true;
                        ChangeStates(EnemyStates.S_Dialogue);
                    }
                    break;
                case EnemyStates.S_Dialogue:
                    if (!hasTalked)
                    {
                        hasTalked = true;
                        StartCoroutine(Dialogue(4.5f));
                    }
                    else
                    {
                        ChangeStates(EnemyStates.S_Chase);
                    }
                    
                    break;
                case EnemyStates.S_Chase:
                    anim.SetBool("Running", true);
                    transform.position = Vector2.MoveTowards(transform.position,
                        new Vector2(PlayerController.Instance.transform.position.x, transform.position.y), speed * Time.deltaTime);

                    if (distanceCheck())
                    {
                        ChangeStates(EnemyStates.S_AttackBehavior);
                    }
                    break;
                case EnemyStates.S_AttackBehavior:
                    if (!isAttacking)
                    {
                        AttackBehavior();
                    }

                    break;
                default:
                    break;
            }
        }
        
    }

    IEnumerator Dialogue(float time)
    {
        PlayerController.Instance.pState.isNPC = true;
        isTalking = true;
        canMove = false;
        DLG.SetActive(true);
        dialogue.text = "The book—it’s in his hands! Take it!";
        npcName.text = "Servant";
        QuestTracker.instance.hasQuest = true;
        yield return new WaitForSeconds(time);
        PlayerController.Instance.pState.isNPC = false;
        chaseDistance = 30f;
        canMove = true;
        DLG.SetActive(false);
        dialogue.text = "";
        npcName.text = "";
        isTalking = false;
        servantscanATK = true;
        PlayerPrefs.SetString("Quest", "Defeat the servants");
        ChangeStates(EnemyStates.S_Idle);
    }
    void stopattacks()
    {
        StopCoroutine(Attack1());
        StopCoroutine(Attack2());
    }
    void stateCheck()
    {
        canMove = !spottedPlayer;
        canAttack = !parried;
        canMove = !parried;
        if (parried)
        {
            stopattacks();
            status.text = "!!";
        }
        else
        {
            status.text = "";
        }
        if(health <= 0)
        {
            canAttack = false;
            canMove = false;
            anim.SetTrigger("Death");
            Destroy(gameObject, 1f);
        }
    }
    public bool distanceCheck()
    {
        float distance = Vector2.Distance(transform.position, PlayerController.Instance.transform.position);
        return distance < 2f;
    }

    void AttackBehavior()
    {
        anim.SetBool("Running", false);
        int i = Random.Range(0,2);
        switch (i)
        {
            case 0:
                StartCoroutine(Attack1());
                break;
            case 1:
                StartCoroutine(Attack2());
                break;
            default:
                break;
        }

    }

    //attacks
    IEnumerator Attack1()
    {
        isAttacking = true;
        canMove = false;
        anim.SetTrigger("Attack1");
        yield return new WaitForSeconds(1f);
        isAttacking = false;
        canMove = true;
        ChangeStates(EnemyStates.S_Chase);
    }

    IEnumerator Attack2()
    {
        isAttacking = true;
        canMove = false;
        anim.SetTrigger("Attack2");
        yield return new WaitForSeconds(2f);
        isAttacking = false;
        canMove = true;
        ChangeStates(EnemyStates.S_Chase);
    }
    
    void flip()
    {
        if (PlayerController.Instance.transform.position.x < transform.position.x)
        {
            transform.eulerAngles = new Vector3(0, 180 ,0);
            healthBar.transform.eulerAngles = new Vector3(0, 0, 0);
            status.transform.eulerAngles = new Vector3(0,0,0);
        }
        else
        {
            transform.eulerAngles = new Vector3(0, 0, 0);
            healthBar.transform.eulerAngles = new Vector3(0, 0, 0);
            status.transform.eulerAngles = new Vector3(0, 0, 0);
        }
    }
}

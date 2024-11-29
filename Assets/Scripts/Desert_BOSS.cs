using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class Desert_BOSS : Enemy
{
    bool attacking = false;
    public bool spottedPlayer = false;
    public float chaseDistance;
    bool MusicPlaying = false;
    Animator anim;
    [SerializeField] GameObject HEALTHBAR;
    [SerializeField] GameObject BORDER_R;
    [SerializeField] GameObject BORDER_L;
    [SerializeField] AudioSource music;

    //essence
    [SerializeField] GameObject essence;
    [SerializeField] Transform essenceloc;

    //world map

    int count;
    bool dead = false;
    public static Desert_BOSS instance;
    protected override void Start()
    {
        base.Start();
        canAttack = true;
        canMove = true;
        anim = GetComponent<Animator>();
        rb.gravityScale = 12f;
        ChangeStates(EnemyStates.DB_Idle);
        BORDER_L.SetActive(false);
        BORDER_R.SetActive(false);
        HEALTHBAR.SetActive(false);
    }
    private void Awake()
    {
        HEALTHBAR.SetActive(PlayerPrefs.GetInt("Desert") != 1);
        gameObject.SetActive(PlayerPrefs.GetInt("Desert") != 1);
        if (instance != null && instance != this)
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
        float _dist = Vector2.Distance(PlayerController.Instance.transform.position, transform.position);
        statusCheck();
        if (health <= 0 && !dead)
        {
            anim.SetTrigger("Dead");
            PlayerController.Instance.pState.killedABoss = true;
            PlayerPrefs.SetString("Quest", "Return to statue");
            Save.instance.saveData();
            dead = true;
        }
        if (canMove && !attacking)
        {
            switch (currentEnemyStates)
            {               
                case EnemyStates.DB_Idle:
                    if (_dist < chaseDistance)
                    {
                        
                        spottedPlayer = true;
                        ChangeStates(EnemyStates.DB_Chase);
                    }
                    break;
                case EnemyStates.DB_Chase:
                    distanceCheck();
                    break;
                case EnemyStates.DB_AttackBehavior:
                    attackBehavior();
                    break;
                default:
                    break;
            }
        }
    }

    void dropEssence()
    {
        if (count != 5)
        {
            count++;
            Instantiate(essence, essenceloc.position, Quaternion.identity);
        }
    }
    void statusCheck()
    {
        canMove = !parried;       
        canAttack = PlayerController.Instance.pState.isAlive;
        if (!attacking) flip();
        if (parried) stopAttacks();
        HEALTHBAR.SetActive(spottedPlayer && PlayerPrefs.GetInt("Desert") != 1);
        BORDER_L.SetActive(spottedPlayer && PlayerPrefs.GetInt("Desert") != 1);
        BORDER_R.SetActive(spottedPlayer && PlayerPrefs.GetInt("Desert") != 1);
        
        if (health <= 0)
        {
            dropEssence();
            canMove = false;
            canAttack = false;
            music.volume -= Time.deltaTime;
            spottedPlayer = false;
            HEALTHBAR.SetActive(false);
            BORDER_L.SetActive(false);
            BORDER_R.SetActive(false);        
            PlayerPrefs.SetInt("Desert", 1);
            Destroy(gameObject, 2f);
        }

        if (!MusicPlaying && spottedPlayer)
        {
            MusicPlaying = true;
            music.Play();
        }
        
    }
    bool inRange()
    {
        float _dist = Vector2.Distance(PlayerController.Instance.transform.position, transform.position);
        return _dist <= 3f;
    }

    void distanceCheck()
    {
        if (inRange())
        {
            anim.SetBool("Run", false);
            ChangeStates(EnemyStates.DB_AttackBehavior);
        }
        else
        {
            anim.SetBool("Run", true);
            transform.position = Vector2.MoveTowards(transform.position, 
                new Vector2(PlayerController.Instance.transform.position.x, transform.position.y), speed * Time.deltaTime);
        }
    }
    void attackBehavior()
    {
        int attacks = Random.Range(0,5);
        if (!attacking)
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
                    StartCoroutine(Attack3());
                    break;
                case 3:
                    StartCoroutine(Attack4());
                    break;
                case 4:
                    StartCoroutine(Attack5());
                    break;
                default:
                    break;
            }
        }
        
    }

    void flip()
    {
        if (PlayerController.Instance.transform.position.x > transform.position.x)
        {
            transform.eulerAngles = new Vector3(0, 0, 0);
        }
        else
        {
            transform.eulerAngles = new Vector3(0, 180, 0);
        }
    }

    //ATTACKS WALA PHASE 2

    void stopAttacks()
    {
        StopCoroutine(Attack1());
        StopCoroutine(Attack2());
        StopCoroutine(Attack3());
        StopCoroutine(Attack4());
        StopCoroutine(Attack5());
    }

    IEnumerator Attack5()
    {
        canMove = false;
        attacking = true;
        anim.SetTrigger("AirAttack");
        yield return new WaitForSeconds(1f);
        canMove = true;
        attacking = false;
        ChangeStates(EnemyStates.DB_Idle);
    }
    IEnumerator Attack1()
    {
        canMove = false;
        attacking = true;
        anim.SetTrigger("Attack1");
        yield return new WaitForSeconds(1f);
        canMove = true;
        attacking = false;
        ChangeStates(EnemyStates.DB_Idle);
    }

    IEnumerator Attack2()
    {
        canMove = false;
        attacking = true;
        anim.SetTrigger("Attack2");
        yield return new WaitForSeconds(1.8f);
        canMove = true;
        attacking = false;
        ChangeStates(EnemyStates.DB_Idle);
    }
    IEnumerator Attack3()
    {
        canMove = false;
        attacking = true;
        anim.SetTrigger("Attack3");
        yield return new WaitForSeconds(2f);
        canMove = true;
        attacking = false;
        ChangeStates(EnemyStates.DB_Idle);
    }
    IEnumerator Attack4()
    {
        canMove = false;
        attacking = true;
        anim.SetTrigger("Ultimate");
        yield return new WaitForSeconds(2f);
        canMove = true;
        attacking = false;
        ChangeStates(EnemyStates.DB_Idle);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FINALBOSS : Enemy
{
    [SerializeField] GameObject HEALTHBAR;

    Animator anim;
    public bool spottedPlayer;
    public float chaseDistance;
    bool isSecondPhase = false;
    bool isAttacking = false;
    bool hasTransformed = false;
    bool isTalking = false;
    AudioManager audioManager;

    //dialogue
    [SerializeField] GameObject DIALOGUE;
    [SerializeField] Text dlg;
    [SerializeField] Text npcName;
    [SerializeField] GameObject UI;


    protected override void Start()
    {
        base.Start();
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
        canMove = true;
        canAttack = true;
        anim = GetComponent<Animator>();
        rb.gravityScale = 12f;
        if (PlayerPrefs.GetInt("HASTALKEDFB") != 1)
        {
            ChangeStates(EnemyStates.FB_Dialogue);
        }
        else
        {
            chaseDistance = 25f;
            ChangeStates(EnemyStates.FB_IDLE);
        }
        
    }

    protected override void UpdateEnemyStates()
    {
        isSecondPhase = health <= maxHealth / 2;
        flip(!isAttacking && !isTalking);
        stateCheck();
        if (isSecondPhase && !hasTransformed)
        {
            hasTransformed = true;
            StartCoroutine(Transform(5f));
        }
        float dist = Vector2.Distance(transform.position, PlayerController.Instance.transform.position);
        if (canMove && PlayerController.Instance.pState.isAlive)
        {
            switch (currentEnemyStates)
            {
                case EnemyStates.FB_Dialogue:
                    if (dist < chaseDistance)
                    {
                        if (!isTalking)
                        {
                            StartCoroutine(Dialogue(4.5f));
                        }
                    }
                    break;
                case EnemyStates.FB_D_Dialogue:
                    if (!isTalking)
                    {
                        anim.SetTrigger("Death");
                        canMove = false;
                        canAttack = false;
                        StartCoroutine(DeathDialogue(4.5f));
                    }
                    break;
                case EnemyStates.FB_IDLE:
                    if (dist < chaseDistance)
                    {
                        spottedPlayer = true;
                        if (!isSecondPhase)
                        {
                            ChangeStates(EnemyStates.FB_CHASE);
                        }                   
                    }
                    break;
                case EnemyStates.FB_CHASE:
                    if (distanceCheck())
                    {
                        ChangeStates(EnemyStates.FB_ATTACKBEHAVIOR);
                    }
                    anim.SetBool("FB_CHASE", true);
                    transform.position = Vector2.MoveTowards(transform.position, 
                        new Vector2(PlayerController.Instance.transform.position.x, transform.position.y), speed * Time.deltaTime);
                    break;
                case EnemyStates.FB_ATTACKBEHAVIOR:
                    if (!isAttacking)
                    {
                        Phase1AttackPattern();
                    }
                    break;
                case EnemyStates.FB_TRANSFORM:
                    if (!hasTransformed)
                    {
                        hasTransformed = true;
                        StartCoroutine(Transform(3f));
                    }
                    else
                    {
                        ChangeStates(EnemyStates.FB_E_IDLE);
                    }
                    break;
                case EnemyStates.FB_E_IDLE:
                    anim.Play("FB_A_IDLE");
                    ChangeStates(EnemyStates.FB_E_CHASE);
                    break;
                case EnemyStates.FB_E_CHASE:
                    if (distanceCheck())
                    {
                        ChangeStates(EnemyStates.FB_E_ATTACKBEHAVIOR);
                    }
                    
                    anim.SetBool("FB_A_CHASE", true);
                    transform.position = Vector2.MoveTowards(transform.position,
                        new Vector2(PlayerController.Instance.transform.position.x, transform.position.y), speed * Time.deltaTime);
                    break;
                case EnemyStates.FB_E_ATTACKBEHAVIOR:
                    if (!isAttacking)
                    {
                        Phase2AttackPattern();
                    }
                    break;
                default:
                    break;
            }
        }
    }

    public void cameraShake()
    {
        CameraShake.Instance.ShakeCamera();
    }
    IEnumerator DeathDialogue(float time)
    {
        PlayerController.Instance.pState.canPause = false;
        PlayerController.Instance.pState.canOpenJournal = false;
        PlayerController.Instance.pState.isNPC = true;
        isTalking = true;
        UI.SetActive(false);
        HEALTHBAR.SetActive(false);
        yield return new WaitForSeconds(2.5f);
        DIALOGUE.SetActive(true);

        string[] dialogue = new[]
        {
            "You’re me. But why? Why destroy everything for this?",
            "I was so close. I could’ve changed everything. Made us better.",
            "You’ve destroyed timelines and universes, lives. All for power?",
            "Power? No. Perfection be able to rewrite permanently, to erase every mistake. Don’t you want the same.",
            "No. Our scars make us who we are. Perfection isn’t worth the cost of everything else.",
            "Better isn’t worth it if it means erasing who we are. Our struggles, our memories—they matter. You’ve lost sight of that.",
            "You’re naive.",
            "I’ll fix this timeline. And I’ll fix you.",
        };


        string[] names = new[]
        {
            "",
            "The Alternate Zieck",
            "",
            "The Alternate Zieck",
            "",
            "",
            "The Alternate Zieck",
            ""
        };

        for (int i = 0; i < dialogue.Length; i++)
        {
            dlg.text = dialogue[i];
            npcName.text = names[i];
            float elapsedtime = 0f;
            while (elapsedtime < time)
            {
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    elapsedtime = time;
                    break;
                }
                else if (Input.GetKeyDown(KeyCode.F))
                {
                    elapsedtime = time;
                }
                elapsedtime += Time.deltaTime;
                yield return null;
            }
        }
        dlg.text = "";
        npcName.text = "";
        DIALOGUE.SetActive(false);
        UI.SetActive(true);
        PlayerController.Instance.pState.canPause = true;
        PlayerController.Instance.pState.canOpenJournal = true;
        PlayerController.Instance.pState.isNPC = false;
        PlayerController.Instance.pState.killedABoss = true;
        Destroy(gameObject, 2f);
        
    }
    IEnumerator Dialogue(float time)
    {
        PlayerController.Instance.pState.canPause = false;
        PlayerController.Instance.pState.canOpenJournal = false;
        PlayerController.Instance.pState.isNPC = true;
        isTalking = true;
        UI.SetActive(false);
        DIALOGUE.SetActive(true);
        string[] dialogue = new[]
        {
            "Because of what you’ve done, my timeline, my world, is on the brink of destruction.",
            "I don’t care about the consequences. I can do whatever it takes to achieve my purpose.",
            "Because I’ve seen the truth. The Grimoire of Ages doesn’t just show history; it shapes it. I’ve collected every one across universes.",
            "To traverse in the future not just past and now, with yours, I’ll have them all.",
        };

        string[] names = new[]
        {
            "",
            "The Alternate Zieck",
            "The Alternate Zieck",
            "The Alternate Zieck",
        };

        for (int i = 0; i < dialogue.Length; i++)
        {
            dlg.text = dialogue[i];
            npcName.text = names[i];
            float elapsetime = 0f;
            while (elapsetime < time)
            {
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    elapsetime = time;
                    break;
                }
                else if(Input.GetKeyDown(KeyCode.F))
                {
                    elapsetime = time;
                }
                elapsetime += Time.deltaTime;
                yield return null;
            }
        }
        dlg.text = "";
        npcName.text = "";
        DIALOGUE.SetActive(false);
        UI.SetActive(true);
        PlayerController.Instance.pState.canPause = true;
        PlayerController.Instance.pState.canOpenJournal = true;
        PlayerController.Instance.pState.isNPC = false;
        chaseDistance = 30f;
        isTalking = false;
        PlayerPrefs.SetInt("HASTALKEDFB", 1);

        QuestTracker.instance.hasQuest = true;
        PlayerPrefs.SetString("Quest", "Absorb the power of Altered Zieck");
        ChangeStates(EnemyStates.FB_IDLE);
    }
    IEnumerator Transform(float time)
    {
        canMove = false;
        isAttacking = true;
        anim.SetTrigger("Transform");
        audioManager.PlaySFX(audioManager.Transform);
        yield return new WaitForSeconds(time);
        canMove = true;
        isAttacking = false;
        ChangeStates(EnemyStates.FB_E_CHASE);
    }
    void stateCheck()
    {
        canMove = !parried;
        canAttack = !canAttack;
        HEALTHBAR.SetActive(spottedPlayer && health >= 0 && PlayerController.Instance.pState.isAlive);
        if (health <= 0) ChangeStates(EnemyStates.FB_D_Dialogue);
    }
    void Phase1AttackPattern()
    {
        int i = Random.Range(0, 4);
        switch (i)
        {
            case 0:
                StartCoroutine(FB_Attack1());
                break;
            case 1:
                StartCoroutine(FB_Attack2());
                break;
            case 2:
                StartCoroutine(FB_Skill1());
                break;
            case 3:
                StartCoroutine(FB_Skill2());
                break;
            default:
                break;
        }
    }

    void Phase2AttackPattern()
    {
        int i = Random.Range(0,4);
        switch (i)
        {
            case 0:
                StartCoroutine(FB_E_Attack1());
                break;
            case 1:
                StartCoroutine(FB_E_Attack2());
                break;
            case 2:
                StartCoroutine(FB_E_Attack3());
                break;
            case 3:
                StartCoroutine(FB_E_Attack4());
                break;
            default:
                break;
        }
    }
    //sounds
    void attackSound()
    {
        audioManager.PlaySFX(audioManager.FB_Attack);
    }

    void thundersound()
    {
        audioManager.PlaySFX(audioManager.FB_Thunder);
    }
    //phase 1 attacks


    IEnumerator FB_Attack1()
    {
        anim.SetBool("FB_CHASE", false);
        isAttacking = true;
        canMove = false;
        anim.SetTrigger("FB_Attack1");
        yield return new WaitForSeconds(1f);
        isAttacking = false;
        canMove = true;
        ChangeStates(EnemyStates.FB_IDLE);
    }
    IEnumerator FB_Attack2()
    {
        anim.SetBool("FB_CHASE", false);
        isAttacking = true;
        canMove = false;
        anim.SetTrigger("FB_Attack2");
        yield return new WaitForSeconds(1f);
        isAttacking = false;
        canMove = true;
        ChangeStates(EnemyStates.FB_IDLE);
    }
    IEnumerator FB_Skill1()
    {
        anim.SetBool("FB_CHASE", false);
        isAttacking = true;
        canMove = false;
        anim.SetTrigger("FB_Skill1");
        yield return new WaitForSeconds(2f);
        isAttacking = false;
        canMove = true;
        ChangeStates(EnemyStates.FB_IDLE);
    }
    IEnumerator FB_Skill2()
    {
        anim.SetBool("FB_CHASE", false);
        isAttacking = true;
        canMove = false;
        anim.SetTrigger("FB_Skill2");
        yield return new WaitForSeconds(4f);
        isAttacking = false;
        canMove = true;
        ChangeStates(EnemyStates.FB_IDLE);
    }

    //phase 2 attacks pare


    IEnumerator FB_E_Attack1()
    {
        anim.SetBool("FB_A_CHASE", false);
        isAttacking = true;
        canMove = false;
        anim.SetTrigger("FB_A_Attack1");
        yield return new WaitForSeconds(1f);
        isAttacking = false;
        canMove = true;
        ChangeStates(EnemyStates.FB_E_CHASE);
    }
    IEnumerator FB_E_Attack2()
    {
        anim.SetBool("FB_A_CHASE", false);
        isAttacking = true;
        canMove = false;
        anim.SetTrigger("FB_A_Attack2");
        yield return new WaitForSeconds(1f);
        isAttacking = false;
        canMove = true;
        ChangeStates(EnemyStates.FB_E_CHASE);
    }
    IEnumerator FB_E_Attack3()
    {
        anim.SetBool("FB_A_CHASE", false);
        isAttacking = true;
        canMove = false;
        anim.SetTrigger("FB_A_Attack3");
        yield return new WaitForSeconds(2f);
        isAttacking = false;
        canMove = true;
        ChangeStates(EnemyStates.FB_E_CHASE);
    }
    IEnumerator FB_E_Attack4()
    {
        anim.SetBool("FB_A_CHASE", false);
        isAttacking = true;
        canMove = false;
        anim.SetTrigger("FB_A_Attack4");
        yield return new WaitForSeconds(3f);
        isAttacking = false;
        canMove = true;
        ChangeStates(EnemyStates.FB_E_CHASE);
    }
    bool distanceCheck()
    {
        float dist = Vector2.Distance(transform.position, PlayerController.Instance.transform.position);
        return dist < 5f;
    }

    void flip(bool canFlip)
    {
        if (canFlip)
        {
            if (PlayerController.Instance.transform.position.x < transform.position.x)
            {
                transform.eulerAngles = new Vector3(0, 180, 0);
            }
            else
            {
                transform.eulerAngles = new Vector3(0, 0, 0);
            }
        }
    }
}

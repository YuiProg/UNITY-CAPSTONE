using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Servant2 : Enemy
{
    //dlg
    [SerializeField] GameObject DIALOGUE;
    [SerializeField] Text dlg;
    [SerializeField] Text names;
    bool istalking = false;
    //world map
    [SerializeField] GameObject worldmap;

    //servant
    Animator anim;
    bool spottedPlayer = false;
    bool isAttacking = false;
    public float chaseDistance;

    protected override void Start()
    {
        base.Start();
        anim = GetComponent<Animator>();
        canMove = true;
        canAttack = true;
        rb.gravityScale = 12f;
        ChangeStates(EnemyStates.S2_Idle);
    }


    protected override void UpdateEnemyStates()
    {
        stateCheck();
        flip();

        float distance = Vector2.Distance(PlayerController.Instance.transform.position, transform.position);
        if (canMove)
        {
            switch (currentEnemyStates)
            {
                case EnemyStates.S2_Idle:
                    if (distance < chaseDistance)
                    {
                        spottedPlayer = true;
                        ChangeStates(EnemyStates.S2_Chase);
                    }
                    break;
                case EnemyStates.S2_Chase:
                    if (spottedPlayer)
                    {
                        anim.SetBool("Running", true);
                        transform.position = Vector2.MoveTowards(transform.position,
                            new Vector2(PlayerController.Instance.transform.position.x, transform.position.y), speed * Time.deltaTime);

                        if (distanceCheck())
                        {
                            ChangeStates(EnemyStates.S2_AttackBehavior);
                        }
                    }
                    else
                    {
                        ChangeStates(EnemyStates.S2_Idle);
                    }

                    break;
                case EnemyStates.S2_AttackBehavior:
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
    void stopattacks()
    {
        StopCoroutine(Attack1());
        StopCoroutine(Attack2());
    }
    bool isnotdead = true;
    bool dialogueStarted = false;
    void stateCheck()
    {
        canMove = !spottedPlayer;
        canAttack = !parried;
        canMove = !parried;

        if (parried)
        {
            stopattacks();
        }

        if (health <= 0)
        {
            isnotdead = false;
            canAttack = false;
            canMove = false;
            QuestTracker.instance.hasQuest = false;
            PlayerPrefs.DeleteKey("Quest");
            anim.SetTrigger("Death");
        }

        if (!isnotdead && !dialogueStarted)
        {
            dialogueStarted = true;
            StartCoroutine(DIALOGUE1(4.5f));
        }
    }

    IEnumerator DIALOGUE1(float time)
    {
        if (!istalking)
        {
            istalking = true;
            PlayerController.Instance.pState.canPause = false;
            PlayerController.Instance.pState.isNPC = true;
            DIALOGUE.SetActive(true);
            PlayerPrefs.SetInt("MAP", 1);
            dlg.text = "There is a current attack in Ifugao we must act now.";
            names.text = "Book of Ages";
            yield return new WaitForSeconds(time);
            dlg.text = "";
            names.text = "";
            DIALOGUE.SetActive(false);
            PlayerController.Instance.pState.Transitioning = true;
            yield return new WaitForSeconds(time - 1);
            PlayerController.Instance.pState.Transitioning = false;
            names.text = "";
            worldmap.SetActive(true);
            yield return new WaitForSeconds(time);
            DIALOGUE.SetActive(true);
            dlg.text = "Here you can select an area to go to.";
            yield return new WaitForSeconds(time);
            dlg.text = "You can see the artifact that we need";
            yield return new WaitForSeconds(time);
            dlg.text = "Lets go in Ifugao";
            yield return new WaitForSeconds(time);
            DIALOGUE.SetActive(false);
            istalking = false;
            Cursor.visible = true;
            Destroy(gameObject, 1f);
        }
        else
        {
            yield return null;
        }
    }
    bool distanceCheck()
    {
        float distance = Vector2.Distance(transform.position, PlayerController.Instance.transform.position);
        return distance < 2f;
    }

    void AttackBehavior()
    {
        anim.SetBool("Running", false);
        int i = Random.Range(0, 2);
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
        ChangeStates(EnemyStates.S2_Chase);
    }

    IEnumerator Attack2()
    {
        isAttacking = true;
        canMove = false;
        anim.SetTrigger("Attack2");
        yield return new WaitForSeconds(2f);
        isAttacking = false;
        canMove = true;
        ChangeStates(EnemyStates.S2_Chase);
    }
    void flip()
    {
        if (PlayerController.Instance.transform.position.x < transform.position.x)
        {
            transform.eulerAngles = new Vector3(0, 180, 0);
            healthBar.transform.eulerAngles = new Vector3(0, 0, 0);
        }
        else
        {
            transform.eulerAngles = new Vector3(0, 0, 0);
            healthBar.transform.eulerAngles = new Vector3(0, 0, 0);
        }
    }
}

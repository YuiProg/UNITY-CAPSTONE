using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MactanArcher : Enemy
{
    bool isAttacking = false;
    bool spottedPlayer = false;
    public float chaseDistance;
    Animator anim;
    //bullet
    [SerializeField] public GameObject bullet;
    [SerializeField] public Transform bulletpos;
    protected override void Start()
    {
        base.Start();
        anim = GetComponent<Animator>();
        canAttack = true;
        canMove = true;
        rb.gravityScale = 12f;
        ChangeStates(EnemyStates.MA_Idle);
    }

    // Update is called once per frame
    protected override void UpdateEnemyStates()
    {
        float _dist = Vector2.Distance(transform.position, PlayerController.Instance.transform.position);
        stateCheck();
        flip(!isAttacking);
        if (canMove && !isAttacking)
        {
            switch (currentEnemyStates)
            {
                case EnemyStates.MA_Idle:
                    if (_dist < chaseDistance)
                    {
                        spottedPlayer = true;
                        ChangeStates(EnemyStates.MA_Attack);
                    }
                    break;
                case EnemyStates.MA_Attack:
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

    void stateCheck()
    {
        canAttack = PlayerController.Instance.pState.isAlive;
        if (health <= 0)
        {
            canMove = false;
            canAttack = false;
            anim.SetTrigger("Death");
            Destroy(gameObject, 1f);
        }
    }

    //attack
    IEnumerator Attack1()
    {
        Debug.Log("ATTACKING");
        isAttacking = true;
        canMove = false;
        anim.SetTrigger("Shoot");
        yield return new WaitForSeconds(.3f);
        shoot();
        yield return new WaitForSeconds(2.5f);
        isAttacking = false;
        canMove = true;
        ChangeStates(EnemyStates.MA_Idle);
    }

    void shoot()
    {
        Instantiate(bullet, bulletpos.transform.position, Quaternion.identity);
    }

    void flip(bool canFlip)
    {
        if (canFlip)
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
}

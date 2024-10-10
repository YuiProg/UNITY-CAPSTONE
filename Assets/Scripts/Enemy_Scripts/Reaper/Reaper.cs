using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Reaper : Enemy
{
    // Start is called before the first frame update
    [SerializeField] private float chaseDistance;
    Animator anim;
    public Text status;
    bool attacking = false;
    protected override void Start()
    {
        base.Start();
        canAttack = true;
        ChangeStates(EnemyStates.Reaper_Idle);
        anim = GetComponent<Animator>();
        
    }

    protected override void UpdateEnemyStates()
    {
        float _dist = Vector2.Distance(transform.position, PlayerController.Instance.transform.position);
        FlipReaper();
        canMove = !attacking;
        if (!PlayerController.Instance.pState.isAlive)
        {
            canAttack = false;
            canMove = false;
        }
        if (parried)
        {
            canMove = false;
            status.text = "! PARRIED !";
        }
        else 
        {
            status.text = " ";
        }
        if (canMove && !attacking)
        {
            if (health <= 0)
            {
                canAttack = false;
                canMove = false;
                anim.SetBool("Reaper_Attack", false);
                anim.SetTrigger("Reaper_Death");
                Destroy(gameObject, 0.5f);
            }
            switch (currentEnemyStates)
            {
                case EnemyStates.Reaper_Idle:
                    if (_dist < chaseDistance)
                    {
                        ChangeStates(EnemyStates.Reaper_Chase);
                    }
                    break;
                case EnemyStates.Reaper_Chase:
                    
                    rb.MovePosition(Vector2.MoveTowards(transform.position, PlayerController.Instance.transform.position, Time.deltaTime * speed));
                    if (inRange())
                    {
                        ChangeStates(EnemyStates.Reaper_Attacking);
                    }
                    
                    break;
                case EnemyStates.Reaper_Attacking:
                    StartCoroutine(Attack1());
                    break;
                default:
                    break;
            }
        }
    }
    bool inRange()
    {
        float _dist = Vector2.Distance(transform.position, PlayerController.Instance.transform.position);
        return _dist <= 1.8f;
    }
    IEnumerator Attack1()
    {
        rb.drag = 1000;
        anim.SetTrigger("Reaper_Attack");
        attacking = true;
        yield return new WaitForSeconds(1f);
        attacking = false;
        rb.drag = 0;
        ChangeStates(EnemyStates.Reaper_Idle);
    }
    void FlipReaper()
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

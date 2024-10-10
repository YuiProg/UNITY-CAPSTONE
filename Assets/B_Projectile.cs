using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class B_Projectile : MonoBehaviour
{
    private GameObject player;
    private Rigidbody2D rb;
    public float force;
    [SerializeField] public float damage;
    
    public float timer;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        player = GameObject.FindGameObjectWithTag("Player");
        

        if (BANDIT_BOSS.instance.lookingLeft)
        {
            Vector3 dir = player.transform.position + transform.position;
            rb.velocity = new Vector2(-dir.x, -dir.y).normalized * force;
        }
        else if (BANDIT_BOSS.instance.lookingRight)
        {
            Vector3 dir = player.transform.position + transform.position;
            rb.velocity = new Vector2(dir.x, dir.y).normalized * force;
        }
    }

    private void Update()
    {
        timer += Time.deltaTime;
        flip();
        if (timer > 4)
        {
            Destroy(gameObject);
        }
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && !PlayerController.Instance.pState.invincible)
        {
            Attack();
            Destroy(gameObject);
        }
    }

    private void Attack()
    {
        if (!PlayerController.Instance.pState.blocking)
        {
            PlayerController.Instance.TakeDamage(damage);
            PlayerController.Instance.HitStopTime(0, 5, 0.5f);
        }
        else
        {
            PlayerController.Instance.TakeDamage(damage);
        }
    }
    bool lookingLeft = false;
    bool lookingRight = false;
    void flip()
    {
        if (PlayerController.Instance.transform.position.x < transform.position.x)
        {
            transform.eulerAngles = new Vector3(0, 180, 0);
            lookingLeft = false;
            lookingRight = true;
        }
        else
        {
            transform.eulerAngles = new Vector3(0, 0, 0);
            lookingLeft = true;
            lookingRight = false;
        }
    }
}

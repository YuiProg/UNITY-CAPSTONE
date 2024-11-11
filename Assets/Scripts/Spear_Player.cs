using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spear_Player : MonoBehaviour
{
    [SerializeField] GameObject LOC;
    private Rigidbody2D rb;
    public float force;
    [SerializeField] public float damage;
    public float timer;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        Vector3 dir = LOC.transform.position - transform.position;
        rb.velocity = new Vector2(dir.x, dir.y).normalized * force;
    }

    private void Update()
    {
        timer += Time.deltaTime;

        if (timer > 4)
        {
            timer = 0;
            Destroy(gameObject);
        }

        if (PlayerController.Instance.transform.position.x < transform.position.x)
        {
            transform.eulerAngles = new Vector3(0, 180, 0);
        }
        else
        {
            transform.eulerAngles = new Vector3(0, 0, 0);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy") && !PlayerController.Instance.pState.invincible)
        {
            Enemy e = GetComponent<Enemy>();
            
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
}


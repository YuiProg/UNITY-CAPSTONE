using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class Surkin_Bullet : MonoBehaviour
{
    private GameObject player;
    private Rigidbody2D rb;
    public float force;
    [SerializeField]public float damage;
    public float timer;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        player = GameObject.FindGameObjectWithTag("Player");

        Vector3 dir = player.transform.position - transform.position;
        rb.velocity = new Vector2(dir.x, dir.y).normalized * force;

    }

    private void Update()
    {
        timer += Time.deltaTime;

        if (timer > 8)
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
}

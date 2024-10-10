using System.Collections;
using UnityEngine;

public class Demon_Projectile : MonoBehaviour
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

        
    }

    IEnumerator Fire(float time)
    {
        yield return new WaitForSeconds(time);
        Vector3 dir = player.transform.position - transform.position;
        rb.velocity = new Vector2(dir.x, dir.y).normalized * force;

        float rot = Mathf.Atan2(-dir.y, -dir.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, rot + 180);
    }
    private void Update()
    {
        StartCoroutine(Fire(3f));
        timer += Time.deltaTime;

        if (timer > 5)
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

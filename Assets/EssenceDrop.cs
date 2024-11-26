using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EssenceDrop : MonoBehaviour
{
    Rigidbody2D rb;
    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        rb.AddForce(new Vector2(0, 10), ForceMode2D.Impulse);
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            PlayerController.Instance.levels = PlayerController.Instance.levels + 1;
            Save.instance.saveStats();
            Destroy(gameObject);
        }
    }
}

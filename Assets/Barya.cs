using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Barya : MonoBehaviour
{
    Rigidbody2D rb;
    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        rb.AddForce(new Vector2(0, 8), ForceMode2D.Impulse);
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            PlayerController.Instance.barya = PlayerController.Instance.barya + 1;
            Save.instance.saveStats();
            Destroy(gameObject);
        }
    }
}

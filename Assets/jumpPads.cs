using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class jumpPads : MonoBehaviour
{
    public float JumpForce;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.gameObject.GetComponent<Rigidbody2D>().AddForce(Vector2.up * JumpForce, ForceMode2D.Impulse);
        }
    }
}

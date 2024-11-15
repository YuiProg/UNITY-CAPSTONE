using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Barya : MonoBehaviour
{
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            PlayerController.Instance.barya = PlayerController.Instance.barya + 1;
            Destroy(gameObject);
        }
    }
}

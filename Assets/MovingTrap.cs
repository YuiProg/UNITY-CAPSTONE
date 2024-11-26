using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingTrap : MonoBehaviour
{
    [SerializeField] Transform UP;
    [SerializeField] Transform DOWN;
    public float movespeed = 4f;

    private Vector3 nextposition;
    private void Start()
    {
        nextposition = DOWN.position;
    }

    private void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, nextposition, movespeed * Time.deltaTime);

        if (transform.position == nextposition)
        {
            nextposition = (nextposition == UP.position) ? DOWN.position : UP.position;
        }
    }



    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            PlayerController.Instance.TakeDamage(50);
        }
    }
}

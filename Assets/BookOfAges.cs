using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BookOfAges : MonoBehaviour
{
    [SerializeField] Transform playerpos;
    float speed;


    void Update()
    {
        gameObject.SetActive(PlayerPrefs.GetInt("HASBOOK") == 1);

        transform.position = playerpos.position;
    }


}


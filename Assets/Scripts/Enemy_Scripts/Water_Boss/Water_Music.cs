using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Water_Music : MonoBehaviour
{

    AudioSource music;
    void Start()
    {
        music = GetComponent<AudioSource>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            music.Play();
        }
    }
}

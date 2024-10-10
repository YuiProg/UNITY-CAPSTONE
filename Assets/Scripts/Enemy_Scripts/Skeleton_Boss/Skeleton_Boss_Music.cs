using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skeleton_Boss_Music : MonoBehaviour
{
    AudioSource music;
    void Start()
    {
        music = GetComponent<AudioSource>();
    }
    private void Update()
    {
        if (!PlayerController.Instance.pState.isAlive)
        {
            music.Stop();
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (PlayerController.Instance.pState.isAlive)
        {
            if (collision.CompareTag("Player"))
            {
                music.Play();
            }
        }
        
    }

}

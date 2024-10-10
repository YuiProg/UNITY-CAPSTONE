using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Power_Up : MonoBehaviour
{
    // Start is called before the first frame update
    float damage = 5;
    float hdamage = 10;

    private void Awake()
    {
        if (PlayerPrefs.GetInt("POWER UP") == 1)
        {
            gameObject.SetActive(false);
        }
        else if (PlayerPrefs.GetInt("POWER UP") == 0)
        {
            gameObject.SetActive(true);
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {

        PlayerPrefs.SetInt("POWER UP", 1);
        if (collision.CompareTag("Player"))
        {
            PlayerController.Instance.normal_damage = PlayerController.Instance.normal_damage + damage;
            PlayerController.Instance.normal_hdamage = PlayerController.Instance.normal_damage + hdamage;
            PlayerController.Instance.damage = PlayerController.Instance.normal_damage;
            PlayerController.Instance.hdamage = PlayerController.Instance.normal_hdamage;
            gameObject.SetActive(false);
        }
        
    }
}

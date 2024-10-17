using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEditor;
using UnityEngine;

public class Save : MonoBehaviour
{
    public static Save instance;
    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
        }

        if (PlayerPrefs.GetInt("LOAD") == 1)
        {
            loadData();
        }
        else if(PlayerPrefs.GetInt("LOAD") == 0)
        {
            print("HAS NO SAVE");
            return;
        }
        
    }
    public void saveData()
    {
        PlayerPrefs.SetFloat("X", transform.position.x);
        PlayerPrefs.SetFloat("Y", transform.position.y);
        PlayerPrefs.SetFloat("Health", PlayerController.Instance.health);
        PlayerPrefs.SetFloat("Max Health", PlayerController.Instance.maxHealth);
        PlayerPrefs.SetFloat("Shield", PlayerController.Instance.shieldCount);
        PlayerPrefs.SetFloat("N_Damage", PlayerController.Instance.normal_damage);
        PlayerPrefs.SetFloat("H_Damage", PlayerController.Instance.normal_hdamage);
        PlayerPrefs.SetFloat("C_Damage", PlayerController.Instance.Cdamage);
        PlayerPrefs.SetInt("Levels", PlayerController.Instance.levels);
        print("SAVED DATA");

    }

    public void loadData()
    {
        try
        {
            float x = PlayerPrefs.GetFloat("X");
            float y = PlayerPrefs.GetFloat("Y");
            float health = PlayerPrefs.GetFloat("Health");
            float maxHealth = PlayerPrefs.GetFloat("Max Health");
            float shield = PlayerPrefs.GetFloat("Shield");
            float Ndamage = PlayerPrefs.GetFloat("N_Damage");
            float Hdamage = PlayerPrefs.GetFloat("H_Damage");
            float combo = PlayerPrefs.GetFloat("C_Damage");
            int levels = PlayerPrefs.GetInt("Levels");
            //health
            PlayerController.Instance.health = health;
            PlayerController.Instance.maxHealth = maxHealth;
            PlayerController.Instance.shieldCount = shield;
            PlayerController.Instance.transform.position = new Vector2(x, y);
            PlayerController.Instance.HealthBar.fillAmount = health / maxHealth;
            //damage
            PlayerController.Instance.normal_damage = Ndamage;
            PlayerController.Instance.normal_hdamage = Hdamage;
            PlayerController.Instance.Cdamage = combo;
            //levels
            PlayerController.Instance.levels = (int)levels;
        }
        catch (System.NullReferenceException test)
        {
            print(test);
        }        
    }

    public void deleteData()
    {

        PlayerPrefs.DeleteAll();
    }
}

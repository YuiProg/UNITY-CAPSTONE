using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelSystem : MonoBehaviour
{
    public Text levels;
    public Text ovDamage;
    public Text nDamage;
    public Text hDamage;
    public Text sDamage;
    public Text health;
    public Text stamina;
    float AllDamage;
    int mainLevel;
    void Update()
    {
        if (mainLevel != 0)
        {
            mainLevel = PlayerPrefs.GetInt("Levels");
            levels.text = $"LEVELS: {PlayerPrefs.GetInt("Levels")}";
            AllDamage = PlayerController.Instance.normal_damage + PlayerController.Instance.normal_hdamage;

            ovDamage.text = $"OVERALL DAMAGE: {AllDamage}";
            nDamage.text = $"NORMAL DAMAGE: {PlayerController.Instance.normal_damage} + 2";
            hDamage.text = $"HARD DAMAGE: {PlayerController.Instance.normal_hdamage} + 2";
            sDamage.text = $"SKILL DAMAGE: {PlayerController.Instance.normal_slash_Damage} + 2";
            health.text = $"HP: {PlayerController.Instance.maxHealth} + 2";
            stamina.text = $"STAMINA: {PlayerController.Instance.maxstamina} + 2";
        }
        else
        {
            mainLevel = PlayerPrefs.GetInt("Levels");
            levels.text = $"LEVELS: {PlayerPrefs.GetInt("Levels")}";
            AllDamage = PlayerController.Instance.normal_damage + PlayerController.Instance.normal_hdamage;

            ovDamage.text = $"OVERALL DAMAGE: {AllDamage}";
            nDamage.text = $"NORMAL DAMAGE: {PlayerController.Instance.normal_damage}";
            hDamage.text = $"HARD DAMAGE: {PlayerController.Instance.normal_hdamage}";
            sDamage.text = $"SKILL DAMAGE: {PlayerController.Instance.normal_slash_Damage}";
            health.text = $"HP: {PlayerController.Instance.maxHealth}";
            stamina.text = $"STAMINA: {PlayerController.Instance.maxstamina}";
        }
        

    }

    public void levelUp()
    {
        if (mainLevel > 0)
        {
            PlayerController.Instance.levels--;
            PlayerController.Instance.stamina = PlayerController.Instance.maxstamina;
            PlayerController.Instance.maxHealth = PlayerController.Instance.maxHealth + 10;
            PlayerController.Instance.maxstamina = PlayerController.Instance.maxstamina + 10;
            PlayerController.Instance.normal_damage = PlayerController.Instance.normal_damage + 2;
            PlayerController.Instance.normal_hdamage = PlayerController.Instance.normal_hdamage  + 2;
            PlayerController.Instance.normal_slash_Damage = PlayerController.Instance.normal_slash_Damage + 2;
            PlayerController.Instance.health = PlayerController.Instance.maxHealth;
            PlayerController.Instance.HealthBar.fillAmount = PlayerController.Instance.health / PlayerController.Instance.maxHealth;
            PlayerController.Instance.damage = PlayerController.Instance.normal_damage;
            PlayerController.Instance.hdamage = PlayerController.Instance.normal_hdamage;
            PlayerController.Instance.Cdamage = PlayerController.Instance.normal_slash_Damage;
            

            Save.instance.saveData();
        }
    }
}

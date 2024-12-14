using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
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
    public Text Defense;
    public Text AllLevel;
    //obtained skill
    public Text spearDamage;
    [SerializeField] GameObject spearDMG;
    [SerializeField] GameObject slashDMG;
    AudioManager audiomanager;
    float AllDamage;
    int mainLevel;

    private void Start()
    {
        audiomanager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
    }
    void Update()
    {
        display();
        if (PlayerPrefs.GetInt("SPEAR") == 1)
        {
            spearDMG.SetActive(true);
        }
        else
        {
            spearDMG.SetActive(false);
        }
        if (PlayerPrefs.GetInt("SLASH") == 1)
        {
            slashDMG.SetActive(true);
        }
        else
        {
            slashDMG.SetActive(false);
        }
    }

    void display()
    {
        if (mainLevel != 0)
        {
            mainLevel = PlayerPrefs.GetInt("Levels");
            levels.text = $"ESSENCE: {PlayerPrefs.GetInt("Levels")}";
            AllLevel.text = $"LEVEL {PlayerController.Instance.mainLevel}";
            AllDamage = PlayerController.Instance.normal_damage + PlayerController.Instance.normal_hdamage;
            spearDamage.text = $"SPEAR DAMAGE: {PlayerController.Instance.normal_spear_damage} + 2";
            ovDamage.text = $"OVERALL DAMAGE: {AllDamage}";
            nDamage.text = $"NORMAL DAMAGE: {PlayerController.Instance.normal_damage} + 2";
            hDamage.text = $"HARD DAMAGE: {PlayerController.Instance.normal_hdamage} + 2";
            sDamage.text = $"SKILL DAMAGE: {PlayerController.Instance.normal_slash_Damage} + 2";
            health.text = $"HP: {PlayerController.Instance.maxHealth} + 10";
            stamina.text = $"STAMINA: {PlayerController.Instance.maxstamina} + 10";
            Defense.text = $"DEFENSE: {PlayerController.Instance.maxShield} + 15";
        }
        else
        {
            mainLevel = PlayerPrefs.GetInt("Levels");
            levels.text = $"ESSENCE: {PlayerPrefs.GetInt("Levels")}";
            AllLevel.text = $"LEVEL {PlayerController.Instance.mainLevel}";
            AllDamage = PlayerController.Instance.normal_damage + PlayerController.Instance.normal_hdamage;
            spearDamage.text = $"SPEAR DAMAGE: {PlayerController.Instance.normal_spear_damage}";
            ovDamage.text = $"OVERALL DAMAGE: {AllDamage}";
            nDamage.text = $"NORMAL DAMAGE: {PlayerController.Instance.normal_damage}";
            hDamage.text = $"HARD DAMAGE: {PlayerController.Instance.normal_hdamage}";
            sDamage.text = $"SKILL DAMAGE: {PlayerController.Instance.normal_slash_Damage}";
            health.text = $"HP: {PlayerController.Instance.maxHealth}";
            stamina.text = $"STAMINA: {PlayerController.Instance.maxstamina}";
            Defense.text = $"DEFENSE: {PlayerController.Instance.maxShield}";
        }
    }
    public void levelUp()
    {
        if (mainLevel > 0)
        {
            audiomanager.PlaySFX(audiomanager.BUTTONCLICK);
            PlayerController.Instance.levels--;
            PlayerController.Instance.normal_damage = PlayerController.Instance.normal_damage + 2;
            PlayerController.Instance.normal_hdamage = PlayerController.Instance.normal_hdamage  + 2;
            PlayerController.Instance.normal_slash_Damage = PlayerController.Instance.normal_slash_Damage + 2;
            PlayerController.Instance.normal_spear_damage = PlayerController.Instance.normal_spear_damage + 2;
            PlayerController.Instance.damage = PlayerController.Instance.normal_damage;
            PlayerController.Instance.hdamage = PlayerController.Instance.normal_hdamage;
            PlayerController.Instance.Cdamage = PlayerController.Instance.normal_slash_Damage;
            PlayerController.Instance.spearDamage = PlayerController.Instance.normal_spear_damage;
            PlayerController.Instance.mainLevel++;

            Save.instance.saveData();
        }
    }

    public void upgradeHP()
    {
        if (mainLevel > 0)
        {
            audiomanager.PlaySFX(audiomanager.BUTTONCLICK);
            PlayerController.Instance.levels--;
            PlayerController.Instance.maxHealth = PlayerController.Instance.maxHealth + 10;
            PlayerController.Instance.health = PlayerController.Instance.maxHealth;
            PlayerController.Instance.stamina = PlayerController.Instance.maxstamina;
            PlayerController.Instance.maxstamina = PlayerController.Instance.maxstamina + 10;
            PlayerController.Instance.HealthBar.fillAmount = PlayerController.Instance.health / PlayerController.Instance.maxHealth;
            PlayerController.Instance.mainLevel++;
            Save.instance.saveData();
        }
    }

    public void upgradeDEF()
    {
        if (mainLevel > 0)
        {
            audiomanager.PlaySFX(audiomanager.BUTTONCLICK);
            PlayerController.Instance.levels--;
            PlayerController.Instance.maxShield = PlayerController.Instance.maxShield + 15;
            PlayerController.Instance.shieldCount = PlayerController.Instance.maxShield;
            PlayerController.Instance.ShieldBar.fillAmount = PlayerController.Instance.shieldCount / PlayerController.Instance.maxShield;
            PlayerController.Instance.mainLevel++;
            Save.instance.saveData();
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class STATS : MonoBehaviour
{
    [SerializeField] Text maxHealth;
    [SerializeField] Text Defence;
    [SerializeField] Text LEVEL;
    [SerializeField] Text nDamage;
    [SerializeField] Text hDamage;
    [SerializeField] Text sDamage;
    [SerializeField] Text spearDamage;
    [SerializeField] Text potionHeal;
    [SerializeField] Text Stamina;
    [SerializeField] Text Amber;
    [Space(5)]
    //skills
    [Header("SKILLS")]
    [SerializeField] GameObject SPEARDAMAGE;
    [SerializeField] GameObject SLASHDAMAGE;
    private void Start()
    {
        getStats();
        checker();
    }

    private void Update()
    {
        getStats();
        checker();
    }

    void checker()
    {
        if (!PlayerController.Instance.pState.obtainedSLASH)
        {
            SLASHDAMAGE.SetActive(false);
        }
        else
        {
            SLASHDAMAGE.SetActive(true);
        }
        if (!PlayerController.Instance.pState.obtainedSpear)
        {
            SPEARDAMAGE.SetActive(false);
        }
        else
        {
            SPEARDAMAGE.SetActive(true);
        }
    }
    void getStats()
    {
        LEVEL.text = $"LEVEL {PlayerController.Instance.mainLevel}";
        Amber.text = $"AMBER - {PlayerController.Instance.barya}";
        maxHealth.text = $"MAX HEALTH: {PlayerController.Instance.maxHealth}";
        Defence.text = $"DEFENCE: {PlayerController.Instance.shieldCount}";
        nDamage.text = $"NORMAL ATTACK DAMAGE: {PlayerController.Instance.normal_damage}";
        hDamage.text = $"HARD ATTACK DAMAGE: {PlayerController.Instance.normal_hdamage}";
        sDamage.text = $"SLASH DAMAGE: {PlayerController.Instance.normal_slash_Damage}";
        spearDamage.text = $"SPEAR DAMAGE: {PlayerController.Instance.normal_spear_damage}";
        Stamina.text = $"STAMINA: {PlayerController.Instance.maxstamina}";
        potionHeal.text = $"POTION HEAL: {PlayerController.Instance.potionHealBar}";
    }
}

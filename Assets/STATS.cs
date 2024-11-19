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
    [SerializeField] Text potionHeal;
    [SerializeField] Text Stamina;
    [SerializeField] Text Amber;
    private void Start()
    {
        getStats();
    }

    void getStats()
    {
        LEVEL.text = $"LEVEL {PlayerController.Instance.mainLevel}";
        Amber.text = $"AMBER - {PlayerController.Instance.barya}";
        maxHealth.text = $"MAX HEALTH: {PlayerController.Instance.maxHealth}";
        Defence.text = $"DEFENCE: {PlayerController.Instance.shieldCount}";
        nDamage.text = $"NORMAL ATTACK DAMAGE: {PlayerController.Instance.normal_damage}";
        hDamage.text = $"HARD ATTACK DAMAGE: {PlayerController.Instance.normal_hdamage}";
        sDamage.text = $"SKILL DAMAGE: {PlayerController.Instance.normal_slash_Damage}";
        Stamina.text = $"STAMINA: {PlayerController.Instance.maxstamina}";
        potionHeal.text = $"POTION HEAL: {PlayerController.Instance.potionHealBar}";
    }
}

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
    float AllDamage;
    int mainLevel;
    void Update()
    {
        mainLevel = PlayerController.Instance.levels;
        levels.text = $"LEVELS: {PlayerController.Instance.levels}";
        AllDamage = PlayerController.Instance.normal_damage + PlayerController.Instance.normal_hdamage;

        ovDamage.text = $"OVERALL DAMAGE: {AllDamage}";
        nDamage.text = $"NORMAL DAMAGE: {PlayerController.Instance.normal_damage}";
        hDamage.text = $"HARD DAMAGE: {PlayerController.Instance.normal_hdamage}";
        sDamage.text = $"SKILL DAMAGE: {PlayerController.Instance.Cdamage}";

    }

    public void levelUp()
    {
        if (mainLevel != 0)
        {
            PlayerController.Instance.levels--;
            PlayerController.Instance.normal_damage = PlayerController.Instance.normal_damage + 2;
            PlayerController.Instance.normal_hdamage = PlayerController.Instance.normal_hdamage + 2;
            PlayerController.Instance.Cdamage = PlayerController.Instance.Cdamage + 2;
        }
    }
}

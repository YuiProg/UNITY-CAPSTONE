using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialWindow : MonoBehaviour
{
    [SerializeField] GameObject TutorialMenu;
    [SerializeField] GameObject PauseMenu;
    [SerializeField] GameObject ESSENCE;
    [SerializeField] GameObject AMBER;
    [SerializeField] GameObject DODGE;
    [SerializeField] GameObject PARRY;
    [SerializeField] GameObject LEVEL;


    public void levelwin()
    {
        ESSENCE.SetActive(false);
        AMBER.SetActive(false);
        DODGE.SetActive(false);
        LEVEL.SetActive(true);
        PARRY.SetActive(false);
    }

    public void essencewin()
    {
        ESSENCE.SetActive(true);
        AMBER.SetActive(false);
        DODGE.SetActive(false);
        LEVEL.SetActive(false);
        PARRY.SetActive(false);
    }

    public void amberwin()
    {
        ESSENCE.SetActive(false);
        AMBER.SetActive(true);
        DODGE.SetActive(false);
        LEVEL.SetActive(false);
        PARRY.SetActive(false);
    }

    public void dodge()
    {
        ESSENCE.SetActive(false);
        AMBER.SetActive(false);
        DODGE.SetActive(true);
        LEVEL.SetActive(false);
        PARRY.SetActive(false);
    }

    public void parry()
    {
        ESSENCE.SetActive(false);
        AMBER.SetActive(false);
        DODGE.SetActive(false);
        LEVEL.SetActive(false);
        PARRY.SetActive(true);
    }

    public void returntomenu()
    {
        PauseMenu.SetActive(true);
        TutorialMenu.SetActive(false);
    }
}

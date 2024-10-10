using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NEWGAMESAVE : MonoBehaviour
{
    [SerializeField] GameObject newgameBTN;
    [SerializeField] GameObject confirmWIN;
    [SerializeField] GameObject btns;

    public void confirmWINUP()
    {
        newgameBTN.SetActive(false);
        confirmWIN.SetActive(true);
        btns.SetActive(false);
    }

    public void confirmWINDOWN()
    {
        newgameBTN.SetActive(true);
        confirmWIN.SetActive(false);
        btns.SetActive(true);
    }
}

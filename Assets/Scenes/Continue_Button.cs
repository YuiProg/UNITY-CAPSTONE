using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Continue_Button : MonoBehaviour
{
    [SerializeField] GameObject btnc;
    [SerializeField] GameObject newgameOLD;
    [SerializeField] GameObject newgameNEW;
    [SerializeField] GameObject popupWIN;
    void Start()
    {
        if (PlayerPrefs.HasKey("X"))
        {
            popupWIN.SetActive(false);
            btnc.SetActive(true);
            newgameOLD.SetActive(false);
            newgameNEW.SetActive(true);
        }
        else
        {
            popupWIN.SetActive(false);
            btnc.SetActive(false);
            newgameOLD.SetActive(true);
            newgameNEW.SetActive(false);
        }
    }

    
}

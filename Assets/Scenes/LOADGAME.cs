using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class LOADGAME : MonoBehaviour
{

    public void ADDLISTENER()
    {
        PlayerPrefs.SetInt("LOAD", 1);
    }
}

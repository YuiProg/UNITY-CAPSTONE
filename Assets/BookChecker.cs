using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BookChecker : MonoBehaviour
{
    [SerializeField] GameObject BOOK1;
    [SerializeField] GameObject BOOK2;
    [SerializeField] GameObject BOOK3;
    [SerializeField] GameObject BOOK4;

    void Update()
    {
        if (PlayerController.Instance.pState.hasBOOK)
        {
            BOOK1.SetActive(true);
        }
        else
        {
            BOOK1.SetActive(false);
        }
        if (PlayerPrefs.GetInt("HERALD GOLEM") == 1)
        {
            BOOK2.SetActive(true);
        }
        else
        {
            BOOK2.SetActive(false);
        }
        if (PlayerPrefs.GetInt("MAGELLAN") == 1)
        {
            BOOK3.SetActive(true);
        }
        else
        {
            BOOK3.SetActive(false);
        }
        if (PlayerPrefs.GetInt("TONDOMBOSS") == 1)
        {
            BOOK4.SetActive(true);
        }
        else
        {
            BOOK4.SetActive(false);
        }
    }
}

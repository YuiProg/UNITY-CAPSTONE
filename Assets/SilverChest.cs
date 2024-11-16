using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SilverChest : Enemy
{
    Animator anim;
    public Transform baryahere;
    [SerializeField] GameObject Barya;
    bool isopen = false;
    public string chestID;
    protected override void Start()
    {
        base.Start();
        anim = GetComponent<Animator>();

        chestID = chestID + transform.position;

        isopen = PlayerPrefs.GetInt(chestID, 0) == 1;

        if (isopen)
        {
            health = 0; 
            anim.SetBool("Open", true);
        }

    }

    protected override void UpdateEnemyStates()
    {
        if (health <= 0 && !isopen)
        {
            isopen = true;
            drop(5);
            anim.SetBool("Open", true);

            PlayerPrefs.SetInt(chestID, 1);
            PlayerPrefs.Save();
        }
    }

    void drop(int dropcount)
    {
        for (int i = 0; i < dropcount; i++)
        {
            if (dropcount >= 0)
            {
                Instantiate(Barya, baryahere.transform.position, Quaternion.identity);
            }
            
        }
    }
}

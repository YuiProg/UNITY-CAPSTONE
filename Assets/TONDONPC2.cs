using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TONDONPC2 : MonoBehaviour
{
    [SerializeField] GameObject UI;
    [SerializeField] GameObject DIALOGUE;
    [SerializeField] Text dialogue;
    [SerializeField] Text nameNPC;
    bool inTrigger;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            inTrigger = true;
        }    
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            inTrigger = false;
        }
    }


}

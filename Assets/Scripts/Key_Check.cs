using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Key_Check : MonoBehaviour
{
    [SerializeField] Text dialogue;
    [SerializeField] GameObject dlg;
    [SerializeField] GameObject colliderD;
    [SerializeField] Text item;
    Animator anim;
    void Start()
    {
        anim = GetComponent<Animator>();
        dlg.SetActive(false);
        colliderD.SetActive(true);
    }
    private void Awake()
    {
        loadCheck();
    }

    void loadCheck()
    {
        if (PlayerPrefs.GetInt("DOOR_1") == 1)
        {
            gameObject.SetActive(false);
        }
        else if (PlayerPrefs.GetInt("DOOR_1") == 0)
        {
            gameObject.SetActive(true);
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (PlayerController.Instance.pState.hasKey)
            {
                PlayerPrefs.SetInt("DOOR_1", 1);
                anim.Play("OPEN DOOR");
                colliderD.SetActive(false);
                PlayerController.Instance.pState.hasKey = false;
                Destroy(gameObject, 3f);
            }
            else
            {
                dlg.SetActive(true);
                dialogue.text = "MISSING KEY";
            }            
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        dialogue.text = " ";
        item.text = " ";
        dlg.SetActive(false);
    }
}

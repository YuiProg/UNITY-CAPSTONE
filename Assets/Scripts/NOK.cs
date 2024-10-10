using UnityEngine;
using UnityEngine.UI;

public class NOK : MonoBehaviour
{
    public string textprompt;
    [SerializeField] Text text;
    [SerializeField] GameObject tutorial;
    // Start is called before the first frame update
    private void Start()
    {
        tutorial.SetActive(false);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            text.text = textprompt;
            tutorial.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            text.text = "!";
            tutorial.SetActive(false);
        }
    }
}

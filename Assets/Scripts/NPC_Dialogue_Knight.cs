using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class NPC_Dialogue_Knight : MonoBehaviour
{
    [SerializeField] Text dialogue;
    [SerializeField] GameObject dlg;
    // Start is called before the first frame update
    void Start()
    {
        dialogue.text = " ";
        dlg.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            dlg.SetActive(true);
            StartCoroutine(K_dialogue(3));
        }
       
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        dlg.SetActive(false);

    }


    IEnumerator K_dialogue(float time)
    {
        dialogue.text = "Hi my name is Trix";
        yield return new WaitForSeconds(time);
        dialogue.text = "if you happen to step on a smoke apparition";
        yield return new WaitForSeconds(time);
        dialogue.text = "go ahead and interact with that big grave";
        yield return new WaitForSeconds(time);
        dialogue.text = "and thy shall find thyself standin on fog";
        yield return new WaitForSeconds(time);
        dialogue.text = "Oh and by the way";
        yield return new WaitForSeconds(time);
        dialogue.text = "If you meet a knight named Yuri";
        yield return new WaitForSeconds(time);
        dialogue.text = "Please tell me";
        yield return new WaitForSeconds(time);
        dialogue.text = "Please";
    }
}

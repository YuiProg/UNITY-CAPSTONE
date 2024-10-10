using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Checkpoint : MonoBehaviour
{
    public Transform respawnpoint;
    [SerializeField] Text saveTXT;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Save.instance.saveData();
            StartCoroutine(save());
            PlayerController.Instance.updatecheckpoint(respawnpoint.position);
        }
    }

    private IEnumerator save()
    {
        
        saveTXT.text = "Saving...";
        yield return new WaitForSeconds(1.5f);
        saveTXT.text = " ";
    }
}

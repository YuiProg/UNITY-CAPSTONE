using UnityEngine;


public class PORTAL : MonoBehaviour
{
    [SerializeField] GameObject spawnTP;

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (Input.GetKeyDown(KeyCode.Q))
            {
                PlayerController.Instance.transform.position = spawnTP.transform.position;
            }
        }
    }

}

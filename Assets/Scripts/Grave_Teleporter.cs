using UnityEngine;

public class Grave_Teleporter : MonoBehaviour
{
    [SerializeField] GameObject TP_LOC;
    // Start is called before the first frame update
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (Input.GetKeyDown(KeyCode.Q))
            {
                PlayerController.Instance.transform.position = TP_LOC.transform.position;
            }
        }
    }
}

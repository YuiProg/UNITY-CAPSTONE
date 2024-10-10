using UnityEngine;

public class Trigger_Plat : MonoBehaviour
{
    [SerializeField] GameObject inv_plat;
    // Start is called before the first frame update
    private void Awake()
    {
        if (PlayerPrefs.GetInt("INV_PLAT") == 1)
        {
            inv_plat.SetActive(true);
        }
        else if (PlayerPrefs.GetInt("INV_PLAT") == 0)
        {
            inv_plat.SetActive(false);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            PlayerPrefs.SetInt("INV_PLAT", 1);
            inv_plat.SetActive(true);
        }
    }
}

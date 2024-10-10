using UnityEngine;

public class Fall_Damage : MonoBehaviour
{ 

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (PlayerController.Instance.pState.blocking || !PlayerController.Instance.pState.blocking || PlayerController.Instance.pState.invincible || !PlayerController.Instance.pState.invincible ||
                        PlayerController.Instance.pState.parry || !PlayerController.Instance.pState.parry)
            {
                PlayerController.Instance.health = PlayerController.Instance.health - PlayerController.Instance.maxHealth;
                PlayerController.Instance.HealthBar.fillAmount = PlayerController.Instance.health;
            }
        }
        
    }

}

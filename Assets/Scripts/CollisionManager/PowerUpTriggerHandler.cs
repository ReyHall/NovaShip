using UnityEngine;

public class PowerUpTriggerHandler : MonoBehaviour
{
    private PowerUp powerUp;

    private void Awake()
    {
        powerUp = GetComponent<PowerUp>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player")) Destroy(gameObject);
        else if (collision.CompareTag("Shot"))
        {
            Destroy(collision.gameObject);
            if (powerUp != null)
            {
                StartCoroutine(powerUp.MoverParaCimaSuavemente(0.5f, 0.2f));
                powerUp.TrocarPowerUp();
            }
        }
    }
}

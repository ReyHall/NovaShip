using UnityEngine;

public class PowerUpTriggerHandler : MonoBehaviour
{
    public PowerUp powerUp;

    private void Awake()
    {
        if (powerUp == null)
        {
            powerUp = GetComponent<PowerUp>();
            if (powerUp == null)
            {
                Debug.LogError("PowerUp component not found on the GameObject.");
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Destroy(gameObject);
        }
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

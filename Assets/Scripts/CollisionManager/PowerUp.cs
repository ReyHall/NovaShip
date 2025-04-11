using UnityEngine;
using System.Collections.Generic;

public class PowerUp : MonoBehaviour 
{
    [Tooltip("Lista de possíveis power-ups para troca aleatória")]
    public GameObject[] powerUpOptions;

    private Dictionary<string, Color> powerUpColors = new Dictionary<string, Color>()
    {
        { "Fireexplosion", Color.red },
        { "Firefast", Color.magenta },
        { "Invisible", Color.yellow }
    };

    private SpriteRenderer spriteRenderer;
    private PowerUpType powerUpType;

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        powerUpType = GetComponent<PowerUpType>();
        AtualizarCor();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            SpriteRenderer naveRenderer = collision.GetComponent<SpriteRenderer>();

            if (naveRenderer != null && powerUpType != null && powerUpColors.ContainsKey(powerUpType.powerUpID))
            {
                naveRenderer.color = powerUpColors[powerUpType.powerUpID];
            }

            Destroy(gameObject);
        }
        else if (collision.CompareTag("Shot"))
        {
            transform.position += Vector3.up * 0.5f;
            TrocarPowerUp();

            Destroy(collision.gameObject);
        }
    }

    private void TrocarPowerUp()
    {
        if (powerUpOptions.Length <= 1 || powerUpType == null) return;

        string currentID = powerUpType.powerUpID;
        GameObject newPowerUp;
        string newID;

        do
        {
            int index = Random.Range(0, powerUpOptions.Length);
            newPowerUp = powerUpOptions[index];
            newID = newPowerUp.GetComponent<PowerUpType>()?.powerUpID;
        } 
        while (newID == currentID || string.IsNullOrEmpty(newID));
        powerUpType.powerUpID = newID;

        Sprite newSprite = newPowerUp.GetComponent<SpriteRenderer>()?.sprite;
        if (newSprite != null) spriteRenderer.sprite = newSprite;
        AtualizarCor();
    }

    private void AtualizarCor()
    {
        if (spriteRenderer != null && powerUpType != null && powerUpColors.TryGetValue(powerUpType.powerUpID, out Color newColor))
        {
            spriteRenderer.color = newColor;
        }
    }

    private void OnBecameInvisible()
    {
        Destroy(gameObject);
    }
}

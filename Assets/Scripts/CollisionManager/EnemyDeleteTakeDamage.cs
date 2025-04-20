using UnityEngine;

public class EnemyDeleteTakeDamage : MonoBehaviour
{
    [SerializeField] private TrembleEffect trembleEffect;
    [SerializeField] private FlashCollision flashCollision;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            // Aplica tremor e flash no jogador
            TrembleEffect trembleEffect = collision.GetComponent<TrembleEffect>();
            if (trembleEffect != null) trembleEffect.TriggerShake();

            FlashCollision flashCollision = collision.GetComponent<FlashCollision>();
            if (flashCollision != null) flashCollision.TriggerFlash();

            // Aplica dano ao jogador
            Player player = collision.GetComponent<Player>();
            if (player != null)
            {
                player.TakeDamage(10);  // Por exemplo, causa 10 de dano
            }

            // Destrói o inimigo
            Destroy(gameObject);
        }
    }
}


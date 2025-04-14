using UnityEngine;

public class DeleteTakeDamage : MonoBehaviour
{
    private bool isDead = false;
    public AudioClip audioClip;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Shot"))
        {
            // Reproduz o som de colisão na posição atual
            if (audioClip != null)
            {
                AudioSource.PlayClipAtPoint(audioClip, transform.position, 1.0f);
            }

            EnemyHealth health = GetComponent<EnemyHealth>();
            if (health != null)
            {
                health.TakeDamage(1);
            }

            Destroy(collision.gameObject);

            TrembleEffect trembleEnemy = GetComponent<TrembleEffect>();
            if (trembleEnemy != null)
            {
                trembleEnemy.TriggerShake();
            }

            if (!isDead && health != null && health.CurrentHP <= 0)
            {
                isDead = true;

                SpawnerPowerUp spawner = GetComponent<SpawnerPowerUp>();
                if (spawner != null)
                {
                    spawner.TrySpawnPowerUp(transform.position);
                }

                Destroy(gameObject);
            }
        }
    }
}

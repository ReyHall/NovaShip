using UnityEngine;

public class DeleteTakeDamage : MonoBehaviour
{
    private bool isDead = false;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Shot"))
        {
            EnemyHealth health = GetComponent<EnemyHealth>();
            if (health != null) health.TakeDamage(1);
            Destroy(collision.gameObject);
            
            TrembleEffect trembleEnemy = GetComponent<TrembleEffect>();
            if(trembleEnemy != null) trembleEnemy.TriggerShake();

            if (!isDead && health != null && health.CurrentHP <= 0)
            {
                isDead = true;

                SpawnerPowerUp spawner = GetComponent<SpawnerPowerUp>();
                if (spawner != null) spawner.TrySpawnPowerUp(transform.position);
                
                Destroy(gameObject);
            }
        }
    }
}

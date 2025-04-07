using UnityEngine;

public class DeleteTakeDamage : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Shot"))
        {
            EnemyHealth health = GetComponent<EnemyHealth>();
            if (health != null) health.TakeDamage(1);
            Destroy(collision.gameObject);
            
            TrembleEffect trembleEnemy = GetComponent<TrembleEffect>();
            if(trembleEnemy != null) trembleEnemy.TriggerShake();
        }
    }
}

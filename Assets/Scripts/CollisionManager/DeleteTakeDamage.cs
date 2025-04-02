using UnityEngine;

public class DeleteTakeDamage : MonoBehaviour
{
    [SerializeField] public int hp = 5;
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] public GameObject explosionEffect;
    private int maxHp;

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        maxHp = hp;
    }
    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Shot"))
        {
            TakeDamage(1);
            Destroy(collision.gameObject);
        }
    }

    private void TakeDamage(int damage)
    {
        hp -= damage;
        float percentage = Mathf.Clamp((float)hp / maxHp, 0f, 1f);
        spriteRenderer.color = new Color(1f, percentage, percentage);

        if (hp <= 0) Die();
    }

    private void Die()
    {
        if (explosionEffect != null)
        {
            Vector3 explosionPosition = new Vector3(transform.position.x, transform.position.y, -11);
            GameObject explosion = Instantiate(explosionEffect, explosionPosition, Quaternion.identity);
            Destroy(explosion, 1f);
        }

        Destroy(gameObject);
    }
}

using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private int health = 100;

    public void TakeDamage(int damage)
    {
        health -= damage;
        if (health <= 0)  Die();
    }

    private void Die()
    {
        Destroy(gameObject);
    }
}

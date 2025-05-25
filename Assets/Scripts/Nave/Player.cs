using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private int health = 100;

    public bool shieldActive = false;

    public void ActivateShield(float duration)
    {
        shieldActive = true;
        Debug.Log("Shield ativado!");
        Invoke(nameof(DeactivateShield), duration);
    }

    private void DeactivateShield()
    {
        shieldActive = false;
        Debug.Log("Shield acabou!");
    }

    public void TakeDamage(int damage)
    {
        if (shieldActive)
        {
            Debug.Log("Dano bloqueado pelo Shield!");
            return;
        }

        health -= damage;
        Debug.Log("Tomou dano. Vida atual: " + health);

        if (health <= 0) Die();
    }

    private void Die()
    {
        Debug.Log("Jogador morreu!");
        Destroy(gameObject);
    }
}

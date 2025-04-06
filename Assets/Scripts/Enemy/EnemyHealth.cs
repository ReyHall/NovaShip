using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class EnemyHealth : MonoBehaviour
{
    [SerializeField] public int hp = 5;
    [SerializeField] public ParticleSystem particleEffect;
    private SpriteRenderer spriteRenderer;
    private FlashColorEffect flashEffect;
    private int maxHp;

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        flashEffect = GetComponent<FlashColorEffect>();
        maxHp = hp;
    }

    public void TakeDamage(int damage)
    {
        hp -= damage;
        if (flashEffect != null) flashEffect.Flash();
        if (hp <= 0) Die();
    }

    private void Die()
    {
        if (particleEffect != null)
        {
            ParticleSystem effect = Instantiate(particleEffect, transform.position, Quaternion.identity);
            effect.Play();
            Destroy(effect.gameObject, effect.main.duration);
        }

        Destroy(gameObject);
    }
}

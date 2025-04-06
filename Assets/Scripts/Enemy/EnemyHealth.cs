using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class EnemyHealth : MonoBehaviour
{
    [SerializeField] private int hp = 5;
    [SerializeField] private ParticleSystem particleEffect;
    private SpriteRenderer spriteRenderer;
    private FlashColorEffect flashEffect;
    private int maxHp;
    public int CurrentHP => hp;
    public int MaxHP => maxHp;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        flashEffect = GetComponent<FlashColorEffect>();
        maxHp = hp;
    }


    public void TakeDamage(int damage)
    {
        hp -= damage;
        hp = Mathf.Max(0, hp);

        if (flashEffect != null) flashEffect.Flash();
        if (hp <= 0)Die();
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

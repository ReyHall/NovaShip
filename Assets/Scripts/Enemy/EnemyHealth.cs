using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class EnemyHealth : MonoBehaviour
{
    [SerializeField] private int hp = 5;
    [SerializeField] private ParticleSystem particleEffect;

    [Header("Particle Colors")]
    [SerializeField] private Color particleColorA = Color.red;
    [SerializeField] private Color particleColorB = new Color(1f, 0.5f, 0.5f);

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
        if (hp <= 0) Die();
    }

    private void Die()
    {
        if (particleEffect != null)
        {
            ParticleSystem effect = Instantiate(particleEffect, transform.position, Quaternion.identity);

            var mainModule = effect.main;
            mainModule.startColor = new ParticleSystem.MinMaxGradient(particleColorA, particleColorB);

            effect.Play();
            Destroy(effect.gameObject, mainModule.duration);
        }

        Destroy(gameObject);
    }
}

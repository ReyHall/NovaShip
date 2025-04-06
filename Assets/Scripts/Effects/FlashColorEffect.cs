using UnityEngine;
using System.Collections;

public class FlashColorEffect : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    private Coroutine flashCoroutine;
    private Color originalColor;
    [SerializeField] private float flashDuration = 0.1f;
    [SerializeField] private Color highHpColor = Color.cyan;
    [SerializeField] private Color lowHpColor = Color.red;
    [SerializeField] private EnemyHealth enemyHealth;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        originalColor = spriteRenderer.color;

        if (enemyHealth == null) enemyHealth = GetComponent<EnemyHealth>();
    }

    public void Flash()
    {
        if (spriteRenderer == null || enemyHealth == null) return;

        Color flashColor = GetColorBasedOnHealth();

        if (flashCoroutine != null)
        {
            StopCoroutine(flashCoroutine);
        }

        flashCoroutine = StartCoroutine(FlashCoroutine(flashColor, flashDuration));
    }

    private Color GetColorBasedOnHealth()
    {
        float healthPercent = (enemyHealth.MaxHP > 0) ? (float)enemyHealth.CurrentHP / enemyHealth.MaxHP : 0f;

        if (healthPercent >= 0.50f) return highHpColor;
        return lowHpColor;
    }

    private IEnumerator FlashCoroutine(Color colorToFlash, float duration)
    {
        spriteRenderer.color = colorToFlash;
        yield return new WaitForSeconds(duration);
        spriteRenderer.color = originalColor;
    }
}

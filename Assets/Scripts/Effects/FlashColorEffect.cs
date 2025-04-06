using UnityEngine;
using System.Collections;

public class FlashColorEffect : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    private Coroutine flashCoroutine;
    private Color originalColor;
    [SerializeField] private Color flashColor = Color.cyan;
    [SerializeField] private float flashDuration = 0.1f;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        originalColor = spriteRenderer.color;
    }

    public void Flash()
    {
        if (spriteRenderer == null) return;

        if (flashCoroutine != null)
        {
            StopCoroutine(flashCoroutine);
        }

        flashCoroutine = StartCoroutine(FlashCoroutine(flashDuration));
    }

    private IEnumerator FlashCoroutine(float duration)
    {
        spriteRenderer.color = flashColor;
        yield return new WaitForSeconds(duration);
        spriteRenderer.color = originalColor;
    }

}

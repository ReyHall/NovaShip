using UnityEngine;
using UnityEngine.UI;

public class TitleBlink : MonoBehaviour
{
    public Image titleImage;      // sua imagem do título
    public float speed = 2f;      // velocidade da transição
    public float minAlpha = 0.5f; // quão "apagado" ele fica
    public float maxAlpha = 1f;   // quão "aceso" ele fica

    private float t = 0f;

    void Update()
    {
        if (titleImage == null) return;

        // Calcular alpha suave
        t += Time.deltaTime * speed;
        float alpha = Mathf.Lerp(minAlpha, maxAlpha, (Mathf.Sin(t) + 1f) / 2f); // fade suave com seno

        Color color = titleImage.color;
        color.a = alpha;
        titleImage.color = color;
    }
}

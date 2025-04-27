using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class SceneTransition : MonoBehaviour
{
    public Image fadeImage;
    public float fadeDuration = 1f;
    public float loadSceneAlphaThreshold = 0.9f;

    void Start()
    {
        StartCoroutine(FadeIn());
    }

    public void StartFade(string sceneName)
    {
        StartCoroutine(FadeOutToScene(sceneName));
    }

    public IEnumerator StartFadeOnly() // Nova função pública para fade sem trocar de cena
    {
        yield return StartCoroutine(FadeOutOnly());
    }

    IEnumerator FadeIn()
    {
        float elapsedTime = 0f;
        Color color = fadeImage.color;
        color.a = 1f;
        fadeImage.color = color;

        while (elapsedTime < fadeDuration)
        {
            float alpha = 1f - Mathf.Clamp01(elapsedTime / fadeDuration);
            color.a = alpha;
            fadeImage.color = color;
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        color.a = 0f;
        fadeImage.color = color;
    }

    IEnumerator FadeOutToScene(string sceneName)
    {
        float elapsedTime = 0f;
        Color color = fadeImage.color;
        color.a = 0f;
        fadeImage.color = color;

        while (elapsedTime < fadeDuration)
        {
            float alpha = Mathf.Clamp01(elapsedTime / fadeDuration);
            color.a = alpha;
            fadeImage.color = color;

            if (alpha >= loadSceneAlphaThreshold)
            {
                SceneManager.LoadScene(sceneName);
                yield break;
            }

            elapsedTime += Time.deltaTime;
            yield return null;
        }
    }

    IEnumerator FadeOutOnly()
    {
        float elapsedTime = 0f;
        Color color = fadeImage.color;
        color.a = 0f;
        fadeImage.color = color;

        while (elapsedTime < fadeDuration)
        {
            float alpha = Mathf.Clamp01(elapsedTime / fadeDuration);
            color.a = alpha;
            fadeImage.color = color;
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Garante tela 100% preta no final
        color.a = 1f;
        fadeImage.color = color;
    }
}

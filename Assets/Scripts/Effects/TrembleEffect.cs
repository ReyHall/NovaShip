using UnityEngine;
using System.Collections;

public class TrembleEffect : MonoBehaviour
{
    [SerializeField] private float duration = 0.3f;
    [SerializeField] private float magnitude = 0.1f;

    private Coroutine shakeCoroutine;
    private Transform target;
    private Vector3 offset = Vector3.zero;
    
    private void Awake()
    {
        target = transform;
    }

    public void TriggerShake()
    {
        if (shakeCoroutine != null) StopCoroutine(shakeCoroutine);
        shakeCoroutine = StartCoroutine(Shake());
    }

    private IEnumerator Shake()
    {
        float elapsed = 0f;

        while (elapsed < duration)
        {
            offset.x = Random.Range(-1f, 1f) * magnitude;
            offset.y = Random.Range(-1f, 1f) * magnitude;

            target.localPosition += offset;

            yield return null;

            target.localPosition -= offset;
            elapsed += Time.deltaTime;
        }

        offset = Vector3.zero;
    }
}

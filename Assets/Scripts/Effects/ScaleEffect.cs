using UnityEngine;

public class ScaleEffect : MonoBehaviour
{
    public float scaleSpeed = 0.2f;    // Velocidade da oscilação
    public float minScale = 0.5f;      // Escala mínima
    public float maxScale = 1f;        // Escala máxima

    private Vector3 initialScale;
    private float time;

    void Start()
    {
        initialScale = transform.localScale; // Armazena a escala inicial do objeto
    }

    void Update()
    {
        time += Time.deltaTime * scaleSpeed;
        float scale = Mathf.PingPong(time, maxScale - minScale) + minScale;
        transform.localScale = initialScale * scale;
    }

    // Método para definir o valor de 'time'
    public void SetTime(float newTime)
    {
        time = newTime;
    }

    // Método para obter o valor atual de 'time'
    public float GetTime()
    {
        return time;
    }
}

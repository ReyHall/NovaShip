using UnityEngine;

public class FloatingUI : MonoBehaviour
{
    public float floatStrength = 5f;   // altura do movimento
    public float floatSpeed = 2f;      // velocidade da oscilação

    private Vector3 startPos;

    void Start()
    {
        startPos = transform.localPosition;
    }

    void Update()
    {
        float offsetY = Mathf.Sin(Time.time * floatSpeed) * floatStrength;
        transform.localPosition = startPos + new Vector3(0f, offsetY, 0f);
    }
}

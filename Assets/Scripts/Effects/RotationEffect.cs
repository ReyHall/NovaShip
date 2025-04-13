using UnityEngine;

public class RotationEffect : MonoBehaviour 
{
    public float rotationSpeed = 50f;
    public float rotation = 0f;

    private void Update()
    {
        rotation += rotationSpeed * Time.deltaTime;
        transform.rotation = Quaternion.Euler(0, 0, -rotation);    
    }
}


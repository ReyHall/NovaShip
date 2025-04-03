using UnityEngine;

public class MovimentBackground : MonoBehaviour
{
    public float velocity = 0.1f;
    public bool moveHorizontal = false;
    public bool invertY = false;
    public ColorChanger colorChanger;
    private Renderer render;
    private Vector2 offset;

    private void Start()
    {
        render = GetComponent<Renderer>();
        if (colorChanger != null) colorChanger.Initialize(render);
    }

    private void Update()
    {
        if (render != null)
        {
            offset.x += moveHorizontal ? velocity * Time.deltaTime : 0;
            offset.y += velocity * Time.deltaTime * (invertY ? -1 : 1);
            render.material.mainTextureOffset = offset;
            
            if (colorChanger != null) colorChanger.UpdateColor();
        }
    }
}

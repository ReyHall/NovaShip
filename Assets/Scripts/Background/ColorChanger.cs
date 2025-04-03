using UnityEngine;

public class ColorChanger : MonoBehaviour
{
    public float colorChangeSpeed = 1f;
    public float transparency = 0.5f;
    public bool randomColor = true;
    [SerializeField] private Color fixedColor = Color.white;
    private Renderer render;
    private Color currentColor;
    private Color targetColor;
    private float colorLerpTime = 0f;

    public void Initialize(Renderer renderer)
    {
        render = renderer;
        if (render != null)
        {
            SetMaterialTransparent();
            currentColor = randomColor ? GetRandomColor() : fixedColor;
            targetColor = randomColor ? GetRandomColor() : fixedColor;
        }
    }

    public void UpdateColor()
    {
        if (render != null)
        {
            colorLerpTime += Time.deltaTime * colorChangeSpeed;
            render.material.color = Color.Lerp(currentColor, targetColor, colorLerpTime);

            if (colorLerpTime >= 1f)
            {
                currentColor = targetColor;
                targetColor = randomColor ? GetRandomColor() : fixedColor;
                colorLerpTime = 0f;
            }
        }
    }

    private Color GetRandomColor()
    {
        return new Color(Random.value, Random.value, Random.value, transparency);
    }

    private void SetMaterialTransparent()
    {
        if (render != null)
        {
            Material mat = render.material;
            mat.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
            mat.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
            mat.SetInt("_ZWrite", 0);
            mat.DisableKeyword("_ALPHATEST_ON");
            mat.EnableKeyword("_ALPHABLEND_ON");
            mat.DisableKeyword("_ALPHAPREMULTIPLY_ON");
            mat.renderQueue = 3000;
        }
    }
}

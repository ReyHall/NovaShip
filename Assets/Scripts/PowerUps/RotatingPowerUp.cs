using UnityEngine;

public class RotatingPowerUp : MonoBehaviour
{
    [Header("Troca de Prefab")]
    [Tooltip("Prefabs alternativos para trocar após a rotação")]
    public GameObject[] alternativePrefabs;

    [Tooltip("Quantas voltas completas (360°) ele deve fazer antes de trocar")]
    public int rotationsToSwitch = 1;
    private RotationEffect rotationEffect;
    private static int globalLastPrefabIndex = -1;

    void Start()
    {
        rotationEffect = GetComponentInChildren<RotationEffect>();
        if (rotationEffect == null)
        {
            Debug.LogError("Nenhum script RotationEffect encontrado no objeto filho!");
        }
    }

    void Update()
    {
        if (rotationEffect == null) return;

        float currentRotation = rotationEffect.rotation;

        if (currentRotation >= 360f * rotationsToSwitch)
        {
            SwitchToAnotherPowerUp();
            rotationEffect.rotation = 0f;
        }
    }

    void SwitchToAnotherPowerUp()
    {
        if (alternativePrefabs.Length == 0) return;

        int newIndex;
        do
        {
            newIndex = Random.Range(0, alternativePrefabs.Length);
        } while (newIndex == globalLastPrefabIndex && alternativePrefabs.Length > 1);

        globalLastPrefabIndex = newIndex;

        float currentTime = 0f;
        ScaleEffect currentScaleEffect = GetComponentInChildren<ScaleEffect>();
        if (currentScaleEffect != null)
        {
            currentTime = currentScaleEffect.GetTime();
        }

        GameObject newPrefab = alternativePrefabs[newIndex];
        GameObject newGO = Instantiate(newPrefab, transform.position, transform.rotation, transform.parent);

        ScaleEffect newScaleEffect = newGO.GetComponentInChildren<ScaleEffect>();
        if (newScaleEffect != null)
        {
            newScaleEffect.SetTime(currentTime);
        }

        Destroy(gameObject);
    }
}

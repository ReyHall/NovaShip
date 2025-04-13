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
        // Obtém o componente RotationEffect do primeiro filho
        rotationEffect = GetComponentInChildren<RotationEffect>();
        if (rotationEffect == null)
        {
            Debug.LogError("Nenhum script RotationEffect encontrado no objeto filho!");
        }
    }

    void Update()
    {
        if (rotationEffect == null) return;

        // Obtém a rotação acumulada do RotationEffect
        float currentRotation = rotationEffect.rotation;

        // Verifica se a rotação acumulada excedeu o limiar para trocar de prefab
        if (currentRotation >= 360f * rotationsToSwitch)
        {
            SwitchToAnotherPowerUp();
            rotationEffect.rotation = 0f; // Reseta a rotação após a troca
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

        // Captura o valor atual de 'time' do ScaleEffect, se existir
        float currentTime = 0f;
        ScaleEffect currentScaleEffect = GetComponentInChildren<ScaleEffect>();
        if (currentScaleEffect != null)
        {
            currentTime = currentScaleEffect.GetTime();
        }

        GameObject newPrefab = alternativePrefabs[newIndex];
        GameObject newGO = Instantiate(newPrefab, transform.position, transform.rotation, transform.parent);

        // Aplica o valor de 'time' ao novo ScaleEffect, se existir
        ScaleEffect newScaleEffect = newGO.GetComponentInChildren<ScaleEffect>();
        if (newScaleEffect != null)
        {
            newScaleEffect.SetTime(currentTime);
        }

        Destroy(gameObject);

        Debug.Log("Power-up trocado para: " + newPrefab.name);
    }
}

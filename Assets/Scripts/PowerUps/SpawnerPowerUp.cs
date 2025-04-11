using UnityEngine;

public class SpawnerPowerUp : MonoBehaviour 
{
    [Tooltip("Prefabs dos power-ups que podem ser instanciados")]
    public GameObject[] powerUpPrefabs;

    [Tooltip("Chance inicial de spawn")]
    public float dropChance = 0f;

    [Tooltip("Chance máxima que pode ser alcançada")]
    public float maxChance = 50f;

    [Tooltip("Quanto a chance aumenta a cada inimigo derrotado sem drop")]
    public float chanceIncrement = 2.5f;

    public void TrySpawnPowerUp(Vector3 position)
    {
        float roll = Random.Range(0f, 100f);

        if (roll <= dropChance && powerUpPrefabs.Length > 0)
        {
            int index = Random.Range(0, powerUpPrefabs.Length);
            GameObject selectedPowerUp = powerUpPrefabs[index];

            if (selectedPowerUp != null)
            {
                Instantiate(selectedPowerUp, position, Quaternion.identity);
                dropChance = 0f;
                return;
            }
        }

        float chanceTotal = Mathf.Min(dropChance + chanceIncrement, maxChance);
        dropChance = chanceTotal;
    }
}

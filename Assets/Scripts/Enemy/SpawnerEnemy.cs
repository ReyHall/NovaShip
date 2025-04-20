using System.Collections;
using UnityEngine;

public class SpawnerEnemy : MonoBehaviour
{
    [SerializeField] private GameObject[] enemys;
    [SerializeField] private float tempo = 2f;
    [SerializeField] private int maxEnemies = 2;

    private Camera cam;
    private int currentEnemyCount = 0;

    private void Awake()
    {
        cam = Camera.main;
    }

    private void Start()
    {
        StartCoroutine(SpawnEnemies());
    }

    private IEnumerator SpawnEnemies()
    {
        while (true)
        {
            if (currentEnemyCount < maxEnemies)
            {
                CreateEnemyInstance();
            }
            yield return new WaitForSeconds(tempo);
        }
    }

    private void CreateEnemyInstance()
    {
        if (enemys.Length == 0) return;

        GameObject enemyPrefab = enemys[Random.Range(0, enemys.Length)];

        float screenX = Random.Range(0.1f, 0.9f);
        float screenY = 1.1f; // Mais acima da tela

        Vector3 spawnPosition = cam.ViewportToWorldPoint(new Vector3(screenX, screenY, 10f)); // z fixo
        spawnPosition.z = 0; // Garante que fique no plano correto

        GameObject enemy = Instantiate(enemyPrefab, spawnPosition, Quaternion.identity);
        currentEnemyCount++;
        
        enemy.GetComponent<Enemy>().OnDestroyed += HandleEnemyDestroyed;
    }


    private void HandleEnemyDestroyed()
    {
        currentEnemyCount = Mathf.Max(0, currentEnemyCount - 1);
    }
}

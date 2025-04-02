using System.Collections;
using UnityEngine;

public class CreateEnemy : MonoBehaviour
{
    [SerializeField] private GameObject[] enemys;
    [SerializeField] private float tempo = 2f;

    private Camera cam;

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
            CreateEnemyInstance();
            yield return new WaitForSeconds(tempo);
        }
    }

    private void CreateEnemyInstance()
    {
        if (enemys.Length == 0) return;

        GameObject enemyPrefab = enemys[Random.Range(0, enemys.Length)];

        float screenX = Random.Range(0.1f, 0.9f);
        float screenY = 1.05f;
        
        Vector3 spawnPosition = cam.ViewportToWorldPoint(new Vector3(screenX, screenY, cam.nearClipPlane));
        spawnPosition.z = 0;

        Instantiate(enemyPrefab, spawnPosition, Quaternion.identity);
    }
}

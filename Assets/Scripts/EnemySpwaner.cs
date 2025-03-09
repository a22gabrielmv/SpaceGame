using UnityEngine;
using System.Collections;

public class EnemySpawner : MonoBehaviour
{
    public GameObject reaperPrefab;  
    public GameObject gethPrefab;    

    public int initialSpawnCount = 2;
    public float initialSpawnRate = 5f;
    public float spawnAcceleration = 0.9f;
    public int maxEnemiesPerWave = 10;
    public float difficultyIncreaseInterval = 30f;

    private Vector3 spawnCenter = new Vector3(0, 0, -100);
    private int currentSpawnCount;
    private float currentSpawnRate;

    void Start()
    {
        
        reaperPrefab.SetActive(false);
        gethPrefab.SetActive(false);

        currentSpawnCount = initialSpawnCount;
        currentSpawnRate = initialSpawnRate;

        StartCoroutine(SpawnEnemies());
        StartCoroutine(IncreaseDifficulty());
    }

    IEnumerator SpawnEnemies()
    {
        while (true)
        {
            for (int i = 0; i < currentSpawnCount; i++)
            {
                SpawnEnemy();
            }
            yield return new WaitForSeconds(currentSpawnRate);
        }
    }

    void SpawnEnemy()
    {
        Vector3 spawnPosition = GetRandomSpawnPosition();
        GameObject basePrefab = Random.Range(0, 10) < 6 ? gethPrefab : reaperPrefab;

        
        GameObject enemy = Instantiate(basePrefab, spawnPosition, Quaternion.identity);
        enemy.SetActive(true);
    }

    Vector3 GetRandomSpawnPosition()
    {
        float distance = Random.Range(110f, 115f);
        float angle = Random.Range(0f, 360f) * Mathf.Deg2Rad;

        float x = spawnCenter.x + distance * Mathf.Cos(angle);
        float z = spawnCenter.z + distance * Mathf.Sin(angle);

        return new Vector3(x, 0, z);
    }

    IEnumerator IncreaseDifficulty()
    {
        while (true)
        {
            yield return new WaitForSeconds(difficultyIncreaseInterval);

            if (currentSpawnRate > 1f)
                currentSpawnRate *= spawnAcceleration;

            if (currentSpawnCount < maxEnemiesPerWave)
                currentSpawnCount++;

            Debug.Log($"Dificultad aumentada: SpawnRate = {currentSpawnRate}, Enemigos por oleada = {currentSpawnCount}");
        }
    }
}

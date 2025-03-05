using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawnerLimited : MonoBehaviour
{
    public GameObject enemyPrefab; // Prefab del nemico da spawnare
    public Transform spawnPoint;   // Punto centrale dello spawn
    public float spawnRadius = 5f; // Raggio di spawn casuale
    public int maxEnemiesToSpawn = 10; // Numero massimo di nemici totali da spawnare
    public float spawnInterval = 60f; // Tempo tra ogni spawn

    private int totalSpawnedEnemies = 0; // Conta il numero totale di nemici spawnati
    private EnemyManager enemyManager;   // Reference to the EnemyManager

    private void Start()
    {
        enemyManager = FindObjectOfType<EnemyManager>(); // Find the EnemyManager in the scene
    }

    // This method should be called by the WaveManager to reset the spawn counter before starting each wave
    public void ResetSpawner()
    {
        totalSpawnedEnemies = 0;
    }

    public void StartSpawningEnemies(int enemiesToSpawn)
    {
        ResetSpawner(); // Reset the total spawned enemies counter before starting a new wave
        StartCoroutine(SpawnEnemies(enemiesToSpawn));
    }

    private IEnumerator SpawnEnemies(int enemiesToSpawn)
    {
        while (totalSpawnedEnemies < enemiesToSpawn)
        {
            yield return new WaitForSeconds(spawnInterval);

            Vector2 randomPosition = (Vector2)spawnPoint.position + Random.insideUnitCircle * spawnRadius;
            GameObject spawnedEnemy = Instantiate(enemyPrefab, randomPosition, Quaternion.identity);
            totalSpawnedEnemies++;

            // Add the spawned enemy to the EnemyManager's list
            if (enemyManager != null)
            {
                enemyManager.AddEnemy(spawnedEnemy);
            }

            Debug.Log($"Spawner {gameObject.name}: Spawned enemy {totalSpawnedEnemies}/{enemiesToSpawn} at {randomPosition}!");
        }

        Debug.Log($"Spawner {gameObject.name}: All enemies spawned for this wave.");
    }

    private void OnDrawGizmosSelected()
    {
        if (spawnPoint != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(spawnPoint.position, spawnRadius);
        }
    }
}

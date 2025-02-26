using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject enemyPrefab; // Prefab del nemico da spawnare
    public Transform spawnPoint;   // Punto centrale dello spawn
    public float spawnRadius = 5f; // Raggio di spawn casuale
    public int maxEnemies = 3;     // Numero massimo di nemici nella scena
    public float spawnInterval = 60f; // Tempo tra ogni verifica di spawn

    private void Start()
    {
        StartCoroutine(SpawnEnemies());
    }

    private IEnumerator SpawnEnemies()
    {
        while (true)
        {
            yield return new WaitForSeconds(spawnInterval);

            int enemyCount = GameObject.FindGameObjectsWithTag("Enemy").Length;

            if (enemyCount < maxEnemies)
            {
                Vector2 randomPosition = (Vector2)spawnPoint.position + Random.insideUnitCircle * spawnRadius;
                Instantiate(enemyPrefab, randomPosition, Quaternion.identity);
                Debug.Log($"Spawned a new enemy at {randomPosition}! Current count: {enemyCount + 1}");
            }
            else
            {
                Debug.Log("Max enemies reached, no spawn.");
            }
        }
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

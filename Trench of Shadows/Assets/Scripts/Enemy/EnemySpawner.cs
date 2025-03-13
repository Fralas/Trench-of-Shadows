using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject enemyPrefab; // Prefab del nemico da spawnare
    public Transform spawnPoint;   // Punto centrale dello spawn
    public float spawnRadius = 5f; // Raggio di spawn casuale
    public int maxEnemies = 3;     // Numero massimo di nemici per questo spawner
    public float spawnInterval = 60f; // Tempo tra ogni verifica di spawn
    public InventoryManager playerInventory; // Riferimento all'inventario del giocatore

    private List<GameObject> spawnedEnemies = new List<GameObject>(); // Lista dei nemici spawnati da questo spawner

    private void Start()
    {
        StartCoroutine(SpawnEnemies());
    }

    private IEnumerator SpawnEnemies()
    {
        while (true)
        {
            yield return new WaitForSeconds(spawnInterval);

            // Rimuove dalla lista i nemici distrutti
            spawnedEnemies.RemoveAll(enemy => enemy == null);

            if (spawnedEnemies.Count < maxEnemies)
            {
                Vector2 randomPosition = (Vector2)spawnPoint.position + Random.insideUnitCircle * spawnRadius;
                GameObject newEnemy = Instantiate(enemyPrefab, randomPosition, Quaternion.identity);
                spawnedEnemies.Add(newEnemy);

                // Assegna il riferimento all'inventario del giocatore
                EnemyDropItem enemyDrop = newEnemy.GetComponent<EnemyDropItem>();
                if (enemyDrop != null)
                {
                    enemyDrop.playerInventory = playerInventory;
                }
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

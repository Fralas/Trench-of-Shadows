using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WorldTime; // This imports the namespace, but we need to explicitly use WorldTime.WorldTime

public class EnemySpawnerAtNight : MonoBehaviour
{
    public GameObject enemyPrefab; 
    public Transform spawnPoint;   
    public float spawnRadius = 5f; 
    public int maxEnemies = 3;     
    public float spawnInterval = 60f; 

    private List<GameObject> spawnedEnemies = new List<GameObject>();
    private WorldTime.WorldTime worldTime; 

    private void Start()
    {
        worldTime = FindObjectOfType<WorldTime.WorldTime>();
        if (worldTime != null)
        {
            worldTime.WorldTimeChanged += OnWorldTimeChanged;
        }
        StartCoroutine(SpawnEnemies());
    }

    private void OnWorldTimeChanged(object sender, TimeSpan currentTime)
    {
        if (currentTime.Hours >= 6 && currentTime.Hours < 18)
        {
            foreach (var enemy in spawnedEnemies)
            {
                if (enemy != null)
                {
                    Destroy(enemy);
                }
            }
            spawnedEnemies.Clear();
        }
    }

    private IEnumerator SpawnEnemies()
    {
        while (true)
        {
            yield return new WaitForSeconds(spawnInterval);

            if (worldTime == null) continue;
            TimeSpan currentTime = worldTime.GetCurrentTime();

            if (currentTime.Hours >= 18 || currentTime.Hours < 6)
            {
                spawnedEnemies.RemoveAll(enemy => enemy == null);

                if (spawnedEnemies.Count < maxEnemies)
                {
                    Vector2 randomPosition = (Vector2)spawnPoint.position + UnityEngine.Random.insideUnitCircle * spawnRadius;
                    GameObject newEnemy = Instantiate(enemyPrefab, randomPosition, Quaternion.identity);
                    spawnedEnemies.Add(newEnemy);
                    Debug.Log($"Spawner {gameObject.name}: Spawned a new enemy at {randomPosition}! Current count: {spawnedEnemies.Count}");
                }
                else
                {
                    Debug.Log($"Spawner {gameObject.name}: Max enemies reached, no spawn.");
                }
            }
            else
            {
                Debug.Log($"Spawner {gameObject.name}: No spawn, it's daytime.");
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

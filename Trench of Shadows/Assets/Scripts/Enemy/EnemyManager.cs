using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    public List<GameObject> aliveEnemies = new List<GameObject>(); // List of all alive enemies

    private void Update()
    {
        // Remove any dead enemies from the list
        aliveEnemies.RemoveAll(enemy => enemy == null);
    }

    // Add an enemy to the list
    public void AddEnemy(GameObject enemy)
    {
        if (!aliveEnemies.Contains(enemy))
        {
            aliveEnemies.Add(enemy);
        }
    }

    // Remove an enemy from the list (called when enemy is destroyed)
    public void RemoveEnemy(GameObject enemy)
    {
        aliveEnemies.Remove(enemy);
    }

    // Get the count of all alive enemies
    public int GetAliveEnemyCount()
    {
        return aliveEnemies.Count;
    }

    private void OnDrawGizmos()
    {
        // You can visualize the count of alive enemies in the scene
        Gizmos.color = Color.green;
        Gizmos.DrawSphere(transform.position, 1f); // Example of how to mark this object
    }
}

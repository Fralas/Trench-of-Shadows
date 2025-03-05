using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro; // Import TextMeshPro namespace

public class WaveManager : MonoBehaviour
{
    public List<EnemySpawnerLimited> enemySpawners; // List of all EnemySpawners in the scene
    public int initialEnemiesPerWave = 5; // Initial number of enemies to spawn in the first wave
    public float timeBetweenWaves = 5f; // Time between waves
    public float spawnDelay = 2f; // Delay before checking enemies after wave starts

    private int currentWave = 0; // Current wave number
    private int enemiesToSpawnThisWave; // Number of enemies to spawn in the current wave
    private int totalEnemiesAlive = 0; // Track total enemies alive in this wave
    private int enemiesKilledThisWave = 0; // Track how many enemies have been killed in the current wave

    private EnemyManager enemyManager; // Reference to the EnemyManager
    private float waveStartTime = 0f; // Time when the wave starts
    private bool isWaveInProgress = false; // Flag to track if a wave is in progress

    public TextMeshProUGUI roundText; // Reference to the UI TextMeshProUGUI component for the round display

    private void Start()
    {
        // Find all the EnemySpawnerLimited components in the scene and add them to the list
        enemySpawners = new List<EnemySpawnerLimited>(FindObjectsOfType<EnemySpawnerLimited>());
        enemyManager = FindObjectOfType<EnemyManager>(); // Find the EnemyManager
        StartNextWave(); // Start the first wave
    }

    private void Update()
    {
        // Only start checking after the spawn delay has passed
        if (Time.time - waveStartTime > spawnDelay)
        {
            // Check if all enemies are killed and start the next wave, but only if the wave is not already in progress
            if (enemyManager.GetAliveEnemyCount() == 0 && totalEnemiesAlive > 0 && !isWaveInProgress)
            {
                // Start the next wave with delay
                if (enemiesKilledThisWave == totalEnemiesAlive)
                {
                    StartCoroutine(PrepareNextWave());
                }
            }
        }
    }

    private void StartNextWave()
    {
        // Ensure the wave is not already in progress
        if (isWaveInProgress) return;

        currentWave++;
        enemiesToSpawnThisWave = (int)(initialEnemiesPerWave * Mathf.Pow(2, currentWave - 1)); // Double the enemies each wave
        totalEnemiesAlive = enemiesToSpawnThisWave; // Set the total enemies alive for this wave
        enemiesKilledThisWave = 0; // Reset killed enemies count

        Debug.Log($"Wave {currentWave} started! Spawn {enemiesToSpawnThisWave} enemies.");
        SpawnEnemiesForWave(); // Spawn the enemies for the current wave

        waveStartTime = Time.time; // Record the start time of the wave
        isWaveInProgress = true; // Mark the wave as in progress

        UpdateRoundText(); // Update the round text
    }

    private void SpawnEnemiesForWave()
    {
        int enemiesPerSpawner = enemiesToSpawnThisWave / enemySpawners.Count; // Calculate how many enemies each spawner should handle

        foreach (var spawner in enemySpawners)
        {
            spawner.StartSpawningEnemies(enemiesPerSpawner); // Tell each spawner how many enemies to spawn
        }
    }

    private IEnumerator PrepareNextWave()
    {
        Debug.Log($"Wave {currentWave} complete! Preparing next wave...");

        // Wait for a short period before starting the next wave
        yield return new WaitForSeconds(timeBetweenWaves);

        // Once the next wave preparation time has passed, start the next wave
        isWaveInProgress = false; // Reset the wave progress flag
        StartNextWave(); // Start the next wave
    }

    // This method will be called by the EnemyHealth script when an enemy dies
    public void OnEnemyDied()
    {
        // Ensure the enemy has not already been disposed of
        if (enemyManager != null)
        {
            enemiesKilledThisWave++;
            Debug.Log($"Enemies killed this wave: {enemiesKilledThisWave}/{totalEnemiesAlive}");

            // Optionally, check if all enemies are dead before triggering next wave.
            if (enemiesKilledThisWave >= totalEnemiesAlive)
            {
                StartCoroutine(PrepareNextWave());
            }
        }
    }

    // Update the RoundText UI to show the current wave
    private void UpdateRoundText()
    {
        if (roundText != null)
        {
            roundText.text = $"{currentWave}"; // Update the text to show the current wave
        }
    }
}

using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelChanger : MonoBehaviour
{
    [SerializeField]
    private LevelConnections _connection;

    [SerializeField]
    private string _targetSceneName;

    [SerializeField]
    private Transform _spawnPoint;

    private static bool isFirstSpawn = true;  // ✅ Controlla se è la prima volta che il gioco parte

    private void Start()
{
    GameObject player = GameObject.FindWithTag("Player");

    if (player != null)
    {
        if (DeathHomeButton.hasRespawned)
        {
            Debug.Log("🛑 Il player è appena rinato, non cambio la posizione!");
            DeathHomeButton.hasRespawned = false; // ✅ Resetto la variabile per il futuro
        }
        else if (isFirstSpawn)
        {
            Transform startingSpawn = GameObject.Find("StartingPoint")?.transform;
            if (startingSpawn != null)
            {
                player.transform.position = startingSpawn.position;
                Debug.Log("Player posizionato sullo StartingPoint: " + startingSpawn.position);
            }
            isFirstSpawn = false;
        }
        else
        {
            player.transform.position = _spawnPoint.position + new Vector3(1, 0, 0);
        }
    }
}


    private void Update()
    {
        if (GameObject.FindWithTag("Player") == null)
        {
            Debug.LogWarning("Player ancora non trovato!");
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.collider.CompareTag("Player"))
        {
            LevelConnections.ActiveConnection = _connection;
            SceneManager.LoadScene(_targetSceneName);
        }
    }
}

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
            if (isFirstSpawn)
            {
                // ✅ Prima volta: usa StartingPoint
                Transform startingSpawn = GameObject.Find("StartingPoint")?.transform;
                if (startingSpawn != null)
                {
                    player.transform.position = startingSpawn.position;
                    Debug.Log("Player posizionato sullo StartingPoint: " + startingSpawn.position);
                }
                else
                {
                    Debug.LogWarning("StartingPoint non trovato nella scena!");
                }
                isFirstSpawn = false;  // ✅ Dopo il primo spawn, non lo usa più
            }
            else
            {
                // ✅ Cambi scena: usa SpawnPoint
                player.transform.position = _spawnPoint.position + new Vector3(1, 0, 0);
                Debug.Log("Player posizionato su SpawnPoint: " + _spawnPoint.position);
            }
        }
        else
        {
            Debug.LogWarning("Player non trovato nella scena!");
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

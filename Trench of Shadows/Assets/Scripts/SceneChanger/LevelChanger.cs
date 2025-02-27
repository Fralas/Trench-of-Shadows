



//VERSIONE 1----------------------------------------------------------------------------------------------------------------------------
/*using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelChanger : MonoBehaviour
{
    [SerializeField]
    private LevelConnections _connection;

    [SerializeField]
    private string _targetSceneName;

    [SerializeField]
    private Transform _spawnPoint;

    private static bool isFirstSpawn = true;  // ✅ Controlla se è la prima volta che il gioco parte

    private void Start()
{
    GameObject player = GameObject.FindWithTag("Player");

    if (player != null)
    {
        if (DeathHomeButton.hasRespawned)
        {
            Debug.Log("🛑 Il player è appena rinato, non cambio la posizione!");
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
            player.transform.position = _spawnPoint.position; // + new Vector3(1, 0, 0)
            Debug.Log("Player posizionato sullo spawnPoint: " + _spawnPoint.name);
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
}*/


//VERSIONE 2 ----------------------------------------------------------------------------------------------------------------------------

/*using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelChanger : MonoBehaviour
{
    [SerializeField] private LevelConnections _connection;
    [SerializeField] private string _targetSceneName;     // Nome della scena da caricare
    [SerializeField] private string _currentSceneSpawnName; // Nome dello spawn nella scena attuale
    [SerializeField] private string _targetSceneSpawnName;  // Nome dello spawn nella prossima scena

    private void Start()
    {
        GameObject player = GameObject.FindWithTag("Player");

        if (player != null)
        {
            // Recupera il nome dello spawn salvato, altrimenti usa quello di default della scena attuale
            string lastSpawn = PlayerPrefs.GetString("LastSpawn", _currentSceneSpawnName);
            GameObject spawn = GameObject.Find(lastSpawn);

            if (spawn != null)
            {
                player.transform.position = spawn.transform.position;
                Debug.Log("✅ Player posizionato su: " + lastSpawn);
            }
            else
            {
                Debug.LogWarning("⚠️ SpawnPoint non trovato: " + lastSpawn);
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.collider.CompareTag("Player"))
        {
            // Salva il punto di spawn per la prossima scena
            PlayerPrefs.SetString("LastSpawn", _targetSceneSpawnName);
            PlayerPrefs.Save();  // Assicura che i dati siano salvati

            // Cambia scena
            LevelConnections.ActiveConnection = _connection;
            SceneManager.LoadScene(_targetSceneName);
        }
    }
}*/


//VERSIONE 3 ----------------------------------------------------------------------------------------------------------------------------
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelChanger : MonoBehaviour
{
    [SerializeField] private LevelConnections _connection;
    [SerializeField] private string _targetSceneName;     // Nome della scena da caricare
    [SerializeField] private string _currentSceneSpawnName; // Nome dello spawn nella scena attuale
    [SerializeField] private string _targetSceneSpawnName;  // Nome dello spawn nella prossima scena

    private static bool isFirstSpawn = true, firstSceneChange=false;

    private void Start()
    {
        GameObject player = GameObject.FindWithTag("Player");

        if (player != null)
        {
            if (DeathHomeButton.hasRespawned)
            {
                Debug.Log("Il player è appena rinato, non cambio la posizione.");
                DeathHomeButton.hasRespawned = false;
            }
            else if (isFirstSpawn)
            {
                Transform startingSpawn = GameObject.Find("StartingPoint")?.transform;
                if (startingSpawn != null)
                {
                    player.transform.position = startingSpawn.position;
                    Debug.Log("Prima volta: Player posizionato su StartingPoint -> " + startingSpawn.position);
                }
                else
                {
                    Debug.LogWarning("StartingPoint non trovato.");
                }
                isFirstSpawn = false;
            }
            else if (firstSceneChange)
            {
                // Recupera l'ultimo spawn salvato o usa quello corrente
                string lastSpawn = PlayerPrefs.GetString("LastSpawn", _currentSceneSpawnName);
                Transform spawnTransform = GameObject.Find(lastSpawn)?.transform;

                if (spawnTransform != null)
                {
                    player.transform.position = spawnTransform.position;
                    Debug.Log("Player posizionato su: " + lastSpawn);
                }
                else
                {
                    Debug.LogWarning("SpawnPoint non trovato: " + lastSpawn + ". Uso spawn di default.");
                    player.transform.position = GameObject.Find(_currentSceneSpawnName)?.transform.position ?? player.transform.position;
                }
            }
        }
        else
        {
            Debug.LogError("Player non trovato nella scena.");
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.collider.CompareTag("Player"))
        {
            // Salva il punto di spawn per la prossima scena
            firstSceneChange = true;
            PlayerPrefs.SetString("LastSpawn", _targetSceneSpawnName);
            PlayerPrefs.Save();
            Debug.Log("Spawn salvato per la prossima scena: " + _targetSceneSpawnName);

            // Cambia scena
            LevelConnections.ActiveConnection = _connection;
            SceneManager.LoadScene(_targetSceneName);
        }
    }
}

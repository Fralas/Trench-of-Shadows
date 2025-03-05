using UnityEngine;
using UnityEngine.SceneManagement;

public class SpawnManager : MonoBehaviour
{
    [SerializeField]
    private LevelConnections _connection;

    [SerializeField]
    private string _targetSceneName;

    [SerializeField]
    private Transform _spawnPoint;

    private void Start()
{
    if(_connection == LevelConnections.ActiveConnection)
    {
            FindObjectOfType<PlayerConstroller>().transform.position = _spawnPoint.position; 
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
        var player = other.collider.GetComponent<PlayerConstroller>();
        if (player != null)
        {
            LevelConnections.ActiveConnection = _connection;
            SceneManager.LoadScene(_targetSceneName);
        }
    }
}

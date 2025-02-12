using UnityEngine;

public class PlayerDatas : MonoBehaviour
{
    public static PlayerDatas instance;  // Singleton per mantenere i dati

    public Vector3 lastPosition;  // Salva la posizione del player tra le scene
    public Vector2 spawnPosition; // Coordinate di spawn nella nuova scena
    public bool useSpawnPosition = false; // Flag per usare la posizione di spawn

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);  // Mantiene l'oggetto tra le scene
        }
        else
        {
            Destroy(gameObject);  // Evita duplicati
        }
    }

    // Metodo per salvare le coordinate di spawn
    public void SetSpawnPosition(Vector2 position)
    {
        spawnPosition = position;
        useSpawnPosition = true; // Attiva il flag per usare la posizione di spawn
        Debug.Log("Spawn position set to: " + spawnPosition);
    }

    // Metodo per ottenere la posizione salvata
    public Vector3 GetSavedPosition()
{
    Debug.Log("Getting saved position. useSpawnPosition: " + useSpawnPosition);
    
    if (useSpawnPosition)
    {
        useSpawnPosition = false; // Disabilita il flag dopo l'uso
        lastPosition = spawnPosition; // 🔹 Aggiorna lastPosition con la spawnPosition
        Debug.Log("Using spawn position: " + spawnPosition);
        return spawnPosition;
    }
    
    Debug.Log("Using last position: " + lastPosition);
    return lastPosition;
}


    // Metodo per salvare la posizione del player
   public void SavePlayerPosition(Vector3 position)
{
    if (!useSpawnPosition) // 🔹 Evita di sovrascrivere subito la posizione di spawn
    {
        lastPosition = position;
        Debug.Log("Position saved: " + lastPosition);
    }
}

}
using UnityEngine;
using UnityEngine.SceneManagement;

public class DeathHomeButton : MonoBehaviour
{
    [SerializeField] private GameObject deathScreen;
    [SerializeField] private PlayerDatas player;

    private Transform startingPoint;
    public static bool hasRespawned = false; // 🔥 Variabile globale per segnalare la rinascita

    private void Start()
    {
        LevelChanger levelChanger = FindObjectOfType<LevelChanger>();
        if (levelChanger != null)
        {
            startingPoint = GameObject.Find("StartingPoint")?.transform;
        }
    }

    public void LoadHomeScene()
    {
        Debug.Log("🔄 Tentativo di rianimazione...");

        if (player != null)
        {
            player.HealFull();
        }

        if (startingPoint != null && player != null)
        {
            player.transform.position = startingPoint.position;
            Debug.Log($"➡️ Player riportato a {startingPoint.position}");
        }

        if (deathScreen != null)
        {
            deathScreen.SetActive(false);
        }

        // ✅ Segnaliamo che il player è appena rinato
        hasRespawned = true;

        // Ricarica la scena
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}

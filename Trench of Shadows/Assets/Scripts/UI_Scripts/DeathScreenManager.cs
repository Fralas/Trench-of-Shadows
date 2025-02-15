using UnityEngine;

public class DeathScreenManager : MonoBehaviour
{
    [SerializeField]
    private GameObject deathScreenUI; // Il pannello UI della schermata di morte

    [SerializeField]
    private PlayerDatas player; // Riferimento al PlayerDatas

    private void Start()
    {
        // Assicuriamoci che la schermata di morte sia inizialmente disattivata
        deathScreenUI.SetActive(false);

        // Iscriviamo la funzione alla morte del player
        player.Died.AddListener(ShowDeathScreen);
    }

    private void ShowDeathScreen()
    {
        deathScreenUI.SetActive(true);
    }
}

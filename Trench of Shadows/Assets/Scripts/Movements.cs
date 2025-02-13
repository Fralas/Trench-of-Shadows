using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; // Per gestire il caricamento delle scene

public class Movements : MonoBehaviour
{
    public float moveSpeed = 5f; // Velocità di movimento del player
    private bool isMoving; // Controlla se il player sta attualmente muovendosi
    private Vector2 input; // Memorizza l'input del giocatore

    public LayerMask solidObjLay; // Layer che rappresenta gli oggetti solidi (ostacoli)
    private Rigidbody2D rb; // Riferimento al Rigidbody2D per la fisica del player

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>(); // Assegna il Rigidbody2D del player
    }

    private void OnEnable() //player avviato in scena
    {
        SceneManager.sceneLoaded += OnSceneLoaded; // manda a funz OnSceneLoaded()
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded; // Si disiscrive dall'evento per evitare memory leak
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Debug.Log("Scene loaded: " + scene.name); // Logga il nome della scena caricata

        if (PlayerDatas.instance != null)
        {
            //Vector3 spawnPos = PlayerDatas.instance.GetSavedPosition(); // Ottiene la posizione salvata del player
            Vector3 spawnPos = new Vector3(5f, 3f, 0f);
            transform.position = spawnPos; // Imposta la posizione del player
            Debug.Log("Player spawned at: " + transform.position);

            // Disabilita temporaneamente il Rigidbody2D per evitare problemi con la fisica
            if (rb != null)
            {
                rb.simulated = false;
                StartCoroutine(EnableRigidbodyAfterDelay()); // Riabilita dopo un breve ritardo
            }
        }
    }

    IEnumerator EnableRigidbodyAfterDelay()
    {
        yield return new WaitForSeconds(0.1f); // Attende 0.1 secondi
        if (rb != null)
        {
            rb.simulated = true; // Riattiva il Rigidbody2D
            Debug.Log("Rigidbody2D re-enabled");
        }
    }

    void Update()
    {
        if (!isMoving) // Controlla se il player non sta già muovendosi
        {
            input.x = Input.GetAxisRaw("Horizontal"); // Ottiene input orizzontale
            input.y = Input.GetAxisRaw("Vertical"); // Ottiene input verticale

            if (input != Vector2.zero) // Se c'è input, calcola la nuova posizione
            {
                Vector3 targetPos = transform.position + new Vector3(input.x, input.y, 0);

                if (isWalkable(targetPos)) // Controlla se la posizione è percorribile
                {
                    StartCoroutine(Move(targetPos)); // Avvia il movimento

                    // Salva la nuova posizione se non si sta cambiando scena
                    if (PlayerDatas.instance != null && !PlayerDatas.instance.useSpawnPosition)
                    {
                        PlayerDatas.instance.SavePlayerPosition(targetPos);
                    }
                }
            }
        }
    }

    IEnumerator Move(Vector3 targetPos)
    {
        isMoving = true; // Imposta il flag di movimento
        while ((targetPos - transform.position).sqrMagnitude > Mathf.Epsilon) // Controlla se è vicino al target
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPos, moveSpeed * Time.deltaTime); // Muove il player
            yield return null;
        }
        transform.position = targetPos; // Imposta la posizione finale
        isMoving = false; // Il movimento è terminato
    }

    private bool isWalkable(Vector3 targetPos)
    {
        return Physics2D.OverlapCircle(targetPos, 0.1f, solidObjLay) == null; // Controlla se c'è un ostacolo nella posizione target
    }
}

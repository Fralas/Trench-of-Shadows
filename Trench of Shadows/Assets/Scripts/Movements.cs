using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Movements : MonoBehaviour
{
    public float moveSpeed = 5f;
    private bool isMoving;
    private Vector2 input;

    public LayerMask solidObjLay;
    private Rigidbody2D rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
{
    Debug.Log("Scene loaded: " + scene.name);

    if (PlayerDatas.instance != null)
    {
        Vector3 spawnPos = PlayerDatas.instance.GetSavedPosition(); // 🔹 Ottieni la posizione corretta
        transform.position = spawnPos; // 🔹 Sposta il Player alla nuova posizione
        Debug.Log("Player spawned at: " + transform.position);

        // Disabilita temporaneamente il Rigidbody2D per evitare conflitti di movimento
        if (rb != null)
        {
            rb.simulated = false;
            StartCoroutine(EnableRigidbodyAfterDelay());
        }
    }
}


    IEnumerator EnableRigidbodyAfterDelay()
    {
        yield return new WaitForSeconds(0.1f); // Aspetta 0.1 secondi
        if (rb != null)
        {
            rb.simulated = true;
            Debug.Log("Rigidbody2D re-enabled");
        }
    }

    void Update()
    {
        if (!isMoving)
        {
            input.x = Input.GetAxisRaw("Horizontal");
            input.y = Input.GetAxisRaw("Vertical");

            if (input != Vector2.zero)
            {
                Vector3 targetPos = transform.position + new Vector3(input.x, input.y, 0);

                if (isWalkable(targetPos))
                {
                    StartCoroutine(Move(targetPos));

                    // Salva la nuova posizione in PlayerDatas solo se non è in fase di cambio scena
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
        isMoving = true;
        while ((targetPos - transform.position).sqrMagnitude > Mathf.Epsilon)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPos, moveSpeed * Time.deltaTime);
            yield return null;
        }
        transform.position = targetPos;
        isMoving = false;
    }

    private bool isWalkable(Vector3 targetPos)
    {
        return Physics2D.OverlapCircle(targetPos, 0.1f, solidObjLay) == null;
    }
}
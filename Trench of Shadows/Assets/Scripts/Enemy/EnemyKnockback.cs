using System.Collections;
using UnityEngine;

public class EnemyKnockback : MonoBehaviour
{
    [SerializeField] private float knockbackForce = 10f;  // Forza del knockback
    [SerializeField] private float knockbackDuration = 0.5f;  // Durata dell'effetto

    private Rigidbody2D rb;
    private bool isKnockedBack = false;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();

        if (rb == null)
        {
            Debug.LogError("EnemyKnockback: Nessun Rigidbody2D trovato! Aggiungilo al nemico.");
        }
    }

    public void ApplyKnockback(Vector2 sourcePosition)
    {
        if (isKnockedBack) return;

        isKnockedBack = true;
        Vector2 knockbackDirection = (transform.position - (Vector3)sourcePosition).normalized;

        StartCoroutine(KnockbackRoutine(knockbackDirection));
    }

    private IEnumerator KnockbackRoutine(Vector2 direction)
    {
        float elapsedTime = 0f;
        while (elapsedTime < knockbackDuration)
        {
            transform.position += (Vector3)direction * (knockbackForce * Time.deltaTime);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        isKnockedBack = false;
    }

    private void ResetKnockback()
    {
        isKnockedBack = false;
    }
}

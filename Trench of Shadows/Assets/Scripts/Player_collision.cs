using UnityEngine;

public class PlayerCollision : MonoBehaviour
{
    public GameObject player;  // Riferimento al Player
    public GameObject door;    // Riferimento alla Porta

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject == door)
        {
            Debug.Log("Il Player ha toccato la porta!");
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject == door)
        {
            Debug.Log("Il Player ha colliso con la porta!");
        }
    }
}

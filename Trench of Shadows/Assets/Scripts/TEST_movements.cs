using UnityEngine;

public class PlayerMovement2D : MonoBehaviour
{
    public float speed = 5f;
    private Rigidbody2D rb;
    private Vector2 movement;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = 0;  // Evita la caduta
        rb.freezeRotation = true;  // Blocca la rotazione
    }

    void Update()
    {
        // Usa Input.GetAxisRaw() per un movimento più reattivo
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");
    }

    void FixedUpdate()
    {
        // Usa velocity per garantire che le collisioni funzionino correttamente
        rb.velocity = movement * speed;
    }
}

using UnityEngine;

public class TitleAnimation : MonoBehaviour
{
    public float amplitude = 20f; // Altezza oscillazione
    public float speed = 2f;      // Velocità oscillazione
    private RectTransform rectTransform;
    private Vector2 startPos;

    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        startPos = rectTransform.anchoredPosition; // Salva la posizione iniziale
    }

    void Update()
    {
        float yOffset = Mathf.Sin(Time.time * speed) * amplitude;
        rectTransform.anchoredPosition = startPos + new Vector2(0, yOffset);
    }
}

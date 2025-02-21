using System.Collections;
using UnityEngine;

public class Crop : MonoBehaviour
{
    public Sprite[] growthStages;  // Array con le immagini delle fasi di crescita
    public float growthTime = 5f;  // Tempo in secondi per ogni fase di crescita

    private SpriteRenderer spriteRenderer;
    private int currentStage = 0;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();

        if (growthStages.Length > 0)
        {
            spriteRenderer.sprite = growthStages[currentStage]; // Imposta la prima immagine
            StartCoroutine(GrowCrop()); // Avvia la crescita
        }
        else
        {
            Debug.LogError("Nessuna immagine di crescita assegnata!");
        }
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.E)){
             CropCut();
        }
    }

    private IEnumerator GrowCrop()
    {
        while (currentStage < growthStages.Length - 1)
        {
            yield return new WaitForSeconds(growthTime); // Aspetta il tempo di crescita
            currentStage++;
            spriteRenderer.sprite = growthStages[currentStage]; // Cambia sprite
            //Debug.Log("Crescita: fase " + currentStage);
        }

        //Debug.Log("Raccolto pronto!");
    }

    void CropCut()
{
    if (currentStage == growthStages.Length - 1) // Se è maturo
    {
        Debug.Log("Raccolto ottenuto!");
        Destroy(gameObject); // Distrugge la pianta dopo la raccolta (puoi farla rilasciare un oggetto)
    }
    else
    {
        Debug.Log("Il raccolto non è ancora pronto!");
    }
}

}

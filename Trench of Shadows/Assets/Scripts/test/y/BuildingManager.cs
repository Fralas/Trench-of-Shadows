using UnityEngine;

public class BuildingSystem : MonoBehaviour
{
    public GameObject objectPrefab; // Prefab dell'oggetto da posizionare
    public string sortingLayerName = "Default"; // Nome del sorting layer
    private bool isBuildingMode = false;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            isBuildingMode = !isBuildingMode; // Attiva/disattiva la modalità building
        }

        if (isBuildingMode)
        {
            Vector3 placementPosition = GetMouseWorldPosition();

            if (Input.GetMouseButtonDown(0)) // Click sinistro per piazzare
            {
                PlaceObject(placementPosition);
            }
            else if (Input.GetMouseButtonDown(1)) // Click destro per rimuovere
            {
                RemoveObject(placementPosition);
            }
        }
    }

    Vector3 GetMouseWorldPosition()
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = 0; // Assicura che l'oggetto venga piazzato sul piano 2D
        return mousePos;
    }

    void PlaceObject(Vector3 position)
    {
        if (objectPrefab != null)
        {
            GameObject newObject = Instantiate(objectPrefab, position, Quaternion.identity);
            var renderer = newObject.GetComponent<SpriteRenderer>();
            if (renderer != null)
            {
                renderer.sortingLayerName = sortingLayerName; // Imposta il sorting layer
            }
        }
    }

    void RemoveObject(Vector3 position)
    {
        Collider2D hitCollider = Physics2D.OverlapPoint(position);
        if (hitCollider != null)
        {
            Destroy(hitCollider.gameObject);
        }
    }
}

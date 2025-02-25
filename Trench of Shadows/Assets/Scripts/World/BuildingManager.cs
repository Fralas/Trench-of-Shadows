using UnityEngine;
using UnityEngine.Tilemaps;

public class BuildingSystem : MonoBehaviour
{
    public GameObject objectPrefab;
    public string sortingLayerName = "Default";
    public Tilemap tilemap;
    private bool isBuildingMode = false;
    private GameObject previewObject;
    private Collider2D previewCollider;
    private GameObject lastPlacedObject; // Memorizza l'ultimo oggetto posizionato

    void Start()
    {
        if (objectPrefab != null)
        {
            previewObject = Instantiate(objectPrefab);
            var renderer = previewObject.GetComponent<SpriteRenderer>();
            previewCollider = previewObject.GetComponent<Collider2D>();

            if (renderer != null)
            {
                renderer.color = new Color(1, 1, 1, 0.5f);
                renderer.sortingLayerName = sortingLayerName;
            }

            if (previewCollider != null)
            {
                previewCollider.enabled = false; // Disabilitiamo il collider per l'anteprima
            }

            previewObject.SetActive(false);
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            isBuildingMode = !isBuildingMode;
            Debug.Log(isBuildingMode ? "Edit mode ON" : "Edit mode OFF");
            if (previewObject != null)
            {
                previewObject.SetActive(isBuildingMode);
            }
        }

        if (isBuildingMode)
        {
            Vector3 placementPosition = GetSnappedMousePosition();
            bool isBlocked = IsObjectSolid(placementPosition, previewObject); // Escludiamo il prefab

            if (previewObject != null)
            {
                previewObject.transform.position = placementPosition;

                SpriteRenderer renderer = previewObject.GetComponent<SpriteRenderer>();
                if (renderer != null)
                {
                    renderer.color = isBlocked ? new Color(1, 0, 0, 0.5f) : new Color(1, 1, 1, 0.5f);
                }
            }

            if (Input.GetMouseButtonDown(0))
            {
                if (lastPlacedObject != null)
                {
                    Destroy(lastPlacedObject);
                    lastPlacedObject = null;
                }
                else if (!isBlocked)
                {
                    lastPlacedObject = PlaceObject(placementPosition);
                }
            }
        }
    }

    Vector3 GetSnappedMousePosition()
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = 0;

        Vector3Int cellPosition = tilemap.WorldToCell(mousePos);
        return tilemap.GetCellCenterWorld(cellPosition);
    }

    GameObject PlaceObject(Vector3 position)
    {
        if (objectPrefab != null)
        {
            GameObject newObject = Instantiate(objectPrefab, position, Quaternion.identity);
            var renderer = newObject.GetComponent<SpriteRenderer>();
            if (renderer != null)
            {
                renderer.sortingLayerName = sortingLayerName;
            }
            return newObject;
        }
        return null;
    }

    bool IsObjectSolid(Vector3 position, GameObject ignoreObject)
    {
        Collider2D hitCollider = Physics2D.OverlapPoint(position);

        if (hitCollider != null && hitCollider.gameObject != ignoreObject)
        {
            Debug.Log($"Trovato oggetto: {hitCollider.gameObject.name}, Tag: {hitCollider.gameObject.tag}");
            return hitCollider.CompareTag("solido");
        }

        return false;
    }
}

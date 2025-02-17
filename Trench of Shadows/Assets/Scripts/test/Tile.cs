using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    [SerializeField] private Color _baseColor, _offsetColor;
    [SerializeField] private SpriteRenderer _renderer;
    [SerializeField] private GameObject _highlight;
    [SerializeField] private GameObject placeableObjectPrefab; // Prefab da piazzare

    private GameObject placedObject; // Memorizza l'oggetto piazzato su questa tile

    public void Init(bool isOffset)
    {
        _renderer.color = isOffset ? _offsetColor : _baseColor;
    }

    void OnMouseEnter()
    {
        _highlight.SetActive(true);
    }

    void OnMouseExit()
    {
        _highlight.SetActive(false);
    }

    void OnMouseOver()
    {
        if (Input.GetMouseButtonDown(1)) // Click destro
        {
            PlaceObjectOnTile();
        }
    }

    private void PlaceObjectOnTile()
    {
        if (placedObject == null) // Controlla che non ci sia già un oggetto
        {
            placedObject = Instantiate(placeableObjectPrefab, transform.position, Quaternion.identity);
        }
    }
}

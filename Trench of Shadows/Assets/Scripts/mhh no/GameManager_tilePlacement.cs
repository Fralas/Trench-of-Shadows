using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.AI;
using UnityEngine.UI;
using TMPro;

public class GameManager_tilePlacement : MonoBehaviour
{
   
   enum BuildMode{construct, delete};
   // BuildMode myBuildMode = BuildMode.construct;
   public Tilemap constructionTileMap, moutainTilemap, treeTileMap;
   private TileBase tileToPlace;
   //public NavMeshSurface2d navMeshSurface;
   public Tiledatabaseso tileDatabase;

   [System.Serializable]
   public class TileslnTilemap{
    
   }


    void Start()
    {
        
    }

    void Update()
    {
        
    }
}

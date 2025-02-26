using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu]
public class Tiledatabaseso : ScriptableObject
{
    [System.Serializable]

    public class TileDatabase{
       public TileBase tile; 
       public Sprite sprite;
    }

    public TileDatabase[]arrayOfTilesDatabase;
    
}

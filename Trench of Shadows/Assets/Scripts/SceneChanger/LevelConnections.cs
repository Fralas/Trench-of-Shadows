using UnityEngine;

[CreateAssetMenu(menuName ="Levels/Connection")]
public class LevelConnections : ScriptableObject
{
    public static LevelConnections ActiveConnection { get; set; }
}

using UnityEngine;

[CreateAssetMenu(fileName = "DungeonGenerationData.asset", menuName = "DungeonGenerationData/DungeonData")]
public class DungeonGenerationData : ScriptableObject
{
    public int crawlerAmount;
    public int interationMin;
    public int interationMax;
}

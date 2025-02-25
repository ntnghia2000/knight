using System.Collections.Generic;
using UnityEngine;

public enum Direction
{
    Top = 0,
    Bottom = 1,
    Left = 2,
    Right = 3
}

public class DungeonCrawlerController : MonoBehaviour
{
    public static List<Vector2Int> positionVisited = new List<Vector2Int>();
    private static readonly Dictionary<Direction, Vector2Int> directionMovementMap = new Dictionary<Direction, Vector2Int>
    {
        {Direction.Top, Vector2Int.up },
        {Direction.Bottom, Vector2Int.down },
        {Direction.Left, Vector2Int.left },
        {Direction.Right, Vector2Int.right }
    };

    public static List<Vector2Int> GenerateDungeon(DungeonGenerationData data)
    {
        List<DungeonCrawler> dungeonCrawlers = new List<DungeonCrawler>();

        for (int i = 0; i < data.crawlerAmount; i++) {
            dungeonCrawlers.Add(new DungeonCrawler(Vector2Int.zero));
        }
        int interation = Random.Range(data.interationMin, data.interationMax);
        for (int i = 0; i < interation; i++) {
            foreach(DungeonCrawler dungeonCrawler in dungeonCrawlers) {
                Vector2Int newPos = dungeonCrawler.Move(directionMovementMap);
                positionVisited.Add(newPos);
            }
        }
        return positionVisited;
    }
}

using System.Collections.Generic;
using UnityEngine;

public class DungeonCrawler : MonoBehaviour
{
    private Vector2Int crawlerPosition;

    public Vector2Int CrawlerPosition
    {
        get { return crawlerPosition; }
        set { crawlerPosition = value; }
    }

    public DungeonCrawler(Vector2Int startPos)
    {
        crawlerPosition = startPos;
    }

    public Vector2Int Move(Dictionary<Direction, Vector2Int> directionMovementMap)
    {
        Direction direction = (Direction)Random.Range(0, directionMovementMap.Count);
        crawlerPosition += directionMovementMap[direction];
        return crawlerPosition;
    }
}

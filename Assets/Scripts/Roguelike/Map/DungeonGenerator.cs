using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class DungeonGenerator : MonoBehaviour
{
    public DungeonGenerationData dungeonGenerationData;
    private List<Vector2Int> dungeonRooms;

    private int counter = 0;

    private void Start()
    {
        dungeonRooms = DungeonCrawlerController.GenerateDungeon(dungeonGenerationData);
        SpawnRooms(dungeonRooms);
    }

    private void SpawnRooms(IEnumerable<Vector2Int> rooms)
    {
        RoomController.instance.loadRoom("Start", 0, 0);
        foreach (Vector2Int roomLocation in rooms) {
            if (roomLocation == dungeonRooms[dungeonRooms.Count - 1]) {
                RoomController.instance.loadRoom("Boss", roomLocation.x, roomLocation.y);
            } else {
                RoomController.instance.loadRoom("Empty", roomLocation.x, roomLocation.y);
            }
            counter++;
        }
    }

    private void Update()
    {
        setupRoomDoors();
    }

    private void setupRoomDoors()
    {
        if (counter >= dungeonRooms.Count) {
            counter = 0;
            RoomController.instance.setupDoors();
        }
    }
}

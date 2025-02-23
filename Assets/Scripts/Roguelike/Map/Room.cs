using System;
using UnityEngine;
using UnityEngine.Pool;
using System.Collections.Generic;
using System.Collections;

public class RoomData
{
    private string name;
    private int col;
    private int row;
    private Vector2 roomWorldSize;

    public RoomData(string name, int col, int row, Vector2 roomWorldSize)
    {
        this.name = name;
        this.col = col;
        this.row = row;
        this.roomWorldSize = roomWorldSize;
    }

    public string Name
    {
        get { return name; }
        set { name = value; }
    }

    public int Col
    {
        get { return col; }
        set { col = value; }
    }

    public int Row
    {
        get { return row; }
        set { row = value; }
    }

    public Vector2 RoomWorldSize
    {
        get { return roomWorldSize; }
        set { roomWorldSize = value; }
    }
}

public class Room : MonoBehaviour
{
    [SerializeField] private int roomWidth;
    [SerializeField] private int roomHeight;
    [SerializeField] private List<Door> doors = new List<Door>();

    private int playerLayer = 10;
    private RoomData roomData;
    private Room nextRoomEnter;

    public void initRoom(string name, int col, int row)
    {
        Vector2 roomWorldSize = new Vector2(roomWidth, roomHeight);
        roomData = new RoomData(name, col, row, roomWorldSize);
        foreach(Door door in doors) {
            door.registerCurrrentRoom(this);
        }
    }

    public RoomData RoomData
    {
        get { return roomData; }
    }

    private void Start()
    {
        if (RoomController.instance == null) {
            return;
        }
    }

    public Vector3 getRoomCenter()
    {
        return transform.position;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = new Color(1, 0, 0, 0.5f);
        Gizmos.DrawWireCube(transform.position, new Vector3(roomWidth, roomHeight, 1));
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == playerLayer) {
            RoomController.instance.onPlayerEnterRoom(this);
        }
    }
}

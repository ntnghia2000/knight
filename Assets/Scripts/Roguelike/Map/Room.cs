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

    public void setDoorActivations()
    {
        if (RoomController.instance.doesRoomExisted(roomData.Col + 1, roomData.Row)) {
            Room rightRoom = RoomController.instance.getRoomByColRow(roomData.Col + 1, roomData.Row);
            doors[(int)Direction.Right].registerNextRoomEnter(rightRoom);
            doors[(int)Direction.Right].gameObject.SetActive(true);
            doors[(int)Direction.Right].enableMovement();
        }
        if (RoomController.instance.doesRoomExisted(roomData.Col - 1, roomData.Row)) {
            Room leftRoom = RoomController.instance.getRoomByColRow(roomData.Col - 1, roomData.Row);
            doors[(int)Direction.Left].registerNextRoomEnter(leftRoom);
            doors[(int)Direction.Left].gameObject.SetActive(true);
            doors[(int)Direction.Left].enableMovement();
        }
        if (RoomController.instance.doesRoomExisted(roomData.Col, roomData.Row + 1)) {
            Room topRoom = RoomController.instance.getRoomByColRow(roomData.Col, roomData.Row + 1);
            doors[(int)Direction.Top].registerNextRoomEnter(topRoom);
            doors[(int)Direction.Top].gameObject.SetActive(true);
            doors[(int)Direction.Top].enableMovement();
        }
        if (RoomController.instance.doesRoomExisted(roomData.Col, roomData.Row - 1)) {
            Room bottomRoom = RoomController.instance.getRoomByColRow(roomData.Col, roomData.Row - 1);
            doors[(int)Direction.Bottom].registerNextRoomEnter(bottomRoom);
            doors[(int)Direction.Bottom].gameObject.SetActive(true);
            doors[(int)Direction.Bottom].enableMovement();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == playerLayer) {
            //RoomController.instance.onPlayerEnterRoom(this);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = new Color(1, 0, 0, 0.5f);
        Gizmos.DrawWireCube(transform.position, new Vector3(roomWidth, roomHeight, 1));
    }
}

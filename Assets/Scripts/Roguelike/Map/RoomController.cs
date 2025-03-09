using UnityEngine;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using System.Collections;

public class RoomController : MonoBehaviour
{
    [SerializeField] private int maxRoomAmount;
    [SerializeField] private GameObject roomPrefab;
    [SerializeField] private int roomWidth;
    [SerializeField] private int roomHeight;
    [SerializeField] private int mapWidth;
    [SerializeField] private int mapHeight;

    public static RoomController instance;
    private RoomData currentRoomData;
    Queue<RoomData> roomDatas = new Queue<RoomData>();
    List<Room> loadedRooms = new List<Room>();
    private string currentRoomName;
    private bool isLoadingRoom = false;
    private Room currentRoom;

    public Room CurrentRoom
    {
        get { return currentRoom; }
    }

    private void Awake()
    {
        instance = this;
    }

    public void loadRoom(string name, int col, int row)
    {
        if (doesRoomExisted(col, row)) {
            return;
        }
        string roomName = name + " " + col + " " + row;
        GameObject newRoom = Instantiate(roomPrefab);
        Room roomComp = newRoom.GetComponent<Room>();
        roomComp.initRoom(roomName, col, row);
        registerRoom(roomComp);
        roomDatas.Enqueue(roomComp.RoomData);
    }

    public void registerRoom(Room room)
    {
        RoomData currentRoomData = room.RoomData;
        int xPos = currentRoomData.Col * roomWidth;
        int yPos = currentRoomData.Row * roomHeight;
        room.transform.position = new Vector3(xPos, yPos, 0);
        room.name = currentRoomData.Name;
        room.transform.parent = transform;
        isLoadingRoom = false;
        loadedRooms.Add(room);
    }

    public void setupDoors()
    {
        foreach (Room room in loadedRooms) {
            room.setDoorActivations();
        }
    }

    public Room getRoomByColRow(int col, int row)
    {
        Room foundRoom = loadedRooms.Find(room => room.RoomData.Col == col && room.RoomData.Row == row);
        return foundRoom;
    }

    public bool doesRoomExisted(int col, int row)
    {
        return getRoomByColRow(col, row) != null;
    }

    public void onPlayerEnterRoom(Room room)
    {
        CameraControl.instance.CurrentRoom = room;
        currentRoom = room;
        currentRoomData = room.RoomData;
        CameraControl.instance.IsChangingRoom = true;
    }
}

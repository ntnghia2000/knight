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
    private int totalColumn = 0;
    private int totalRow = 0;
    private int currentCol = 0;
    private int currentRow = 0;
    private Room currentRoom;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        totalColumn = mapWidth / roomWidth;
        totalRow = mapHeight / roomHeight;

        //if (maxRoomAmount > 0) {
        //    for (int i = 0; i < maxRoomAmount; i++) {
        //        string roomName = "Room" + " " + currentCol + " " + currentRow;
        //        loadRoom(roomName, currentCol, currentRow);

        //        currentCol++;
        //        if (currentCol >= totalColumn) {
        //            currentCol = 0;
        //            currentRow++;
        //        }
        //    }
        //}
        //CameraControl.instance.CurrentRoom = loadedRooms[0];
    }

    public void loadRoom(string name, int col, int row)
    {
        if (doesRoomExisted(col, row)) {
            return;
        }
        GameObject newRoom = Instantiate(roomPrefab);
        Room roomComp = newRoom.GetComponent<Room>();
        roomComp.initRoom(name, col, row);
        registerRoom(roomComp);
        roomDatas.Enqueue(roomComp.RoomData);
    }

    IEnumerator waitForLoadingRoom(RoomData data)
    {
        currentRoomName = data.Name;
        AsyncOperation loadRoom = SceneManager.LoadSceneAsync(currentRoomName, LoadSceneMode.Additive);
        while(loadRoom.isDone == false) {
            yield return null;
        }
    }

    public void registerRoom(Room room)
    {
        RoomData currentRoomData = room.RoomData;
        int offsetX = currentRoomData.Col > 0 ? 6 : 0;
        int offsetY = currentRoomData.Row > 0 ? 2 : 0;
        int xPos = currentRoomData.Col * roomWidth + offsetX;
        int yPos = currentRoomData.Row * roomHeight + offsetY;
        room.transform.position = new Vector3(xPos, yPos, 0);
        room.name = currentRoomData.Name;
        room.transform.parent = transform;
        isLoadingRoom = false;
        loadedRooms.Add(room);
    }

    public bool doesRoomExisted(int col, int row)
    {
        return loadedRooms.Find(room => room.RoomData.Col == col && room.RoomData.Row == row) != null;
    }

    public void onPlayerEnterRoom(Room room)
    {
        CameraControl.instance.CurrentRoom = room;
        currentRoom = room;
        currentRoomData = room.RoomData;
        CameraControl.instance.IsChangingRoom = true;
    }
}

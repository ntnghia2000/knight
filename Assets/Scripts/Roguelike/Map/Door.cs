using UnityEngine;

public enum RoomDirection
{
    Top = 0,
    Bottom = 1,
    Left = 2,
    Right = 3
}

public class Door : MonoBehaviour
{
    [SerializeField] private RoomDirection direction;

    private int playerLayer = 10;
    private Room currentRoom;

    public RoomDirection Direction
    {
        get { return direction; }
    }

    public void registerCurrrentRoom(Room room)
    {
        currentRoom = room;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == playerLayer) {
            //RoomController.instance.onPlayerEnterRoom(this);
        }
    }
}

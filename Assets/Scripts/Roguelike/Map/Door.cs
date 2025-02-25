using UnityEngine;

public class Door : MonoBehaviour
{
    [SerializeField] private Direction direction;

    private int playerLayer = 10;
    private Room currentRoom;

    public Direction Direction
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

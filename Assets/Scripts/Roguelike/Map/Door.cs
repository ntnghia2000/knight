using UnityEngine;

public class Door : MonoBehaviour
{
    [SerializeField] private Direction direction;

    private int playerLayer = 10;
    private Room nextRoomEnter;
    private BoxCollider2D doorCollider;

    public Direction Direction
    {
        get { return direction; }
    }

    private void Start()
    {
        doorCollider = GetComponent<BoxCollider2D>();
        gameObject.SetActive(false);
    }

    public void enableMovement()
    {
        doorCollider.isTrigger = false;
    }

    public void disableMovement()
    {
        doorCollider.isTrigger = true;
    }

    public void registerNextRoomEnter(Room room)
    {
        nextRoomEnter = room;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == playerLayer) {
            RoomController.instance.onPlayerEnterRoom(nextRoomEnter);
        }
    }
}

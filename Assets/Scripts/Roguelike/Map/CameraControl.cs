using UnityEngine;
using static Unity.Cinemachine.CinemachineSplineRoll;

public class CameraControl : MonoBehaviour, IObserver
{
    [SerializeField] private float changeRoomSpeed;

    public static CameraControl instance;
    private Room currentRoom;
    private bool isChangingRoom = false;

    public Room CurrentRoom
    {
        get { return currentRoom; }
        set { currentRoom = value; }
    }

    public bool IsChangingRoom
    {
        get { return isChangingRoom; }
        set { isChangingRoom = value; }
    }

    private void Awake()
    {
        instance = this;
    }

    private void Update()
    {
        if (!isChangingRoom) return;

        updatePosition();
    }

    private void updatePosition()
    {
        if (currentRoom == null) return;
        Vector3 targetPosition = getTargetPosition();
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, Time.deltaTime * changeRoomSpeed);
        if (transform.position == targetPosition) {
            isChangingRoom = false;
        }
    }

    private Vector3 getTargetPosition()
    {
        if (currentRoom == null) return Vector3.zero;
        Vector3 targetPos = currentRoom.getRoomCenter();
        targetPos.z = transform.position.z;
        return targetPos;
    }

    public void OnNotify()
    {
        Debug.Log("Initialize unit success");
    }

    public void TriggerAction(PlayerActions action)
    {
        if (action == PlayerActions.TouchingDoor) {
            isChangingRoom = true;
        }
        if (action == PlayerActions.TouchingWall) {
            isChangingRoom = true;
        }
    }
}

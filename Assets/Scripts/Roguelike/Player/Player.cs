using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum PlayerActions
{
    Idle,
    Walk,
    Jump,
    Dash,
    Attack,
    TouchingDoor,
    TouchingWall
}

public class Player : Subject
{
    [SerializeField] private float movingSpeed = 2f;

    private CircleCollider2D playerCollider;
    private Direction touchedDoorDirection;
    private Vector3 newRoomEnterPosition;
    private float horizontalInput;
    private float verticalInput;
    private float movingDistance = 0;

    private bool canMove = false;
    private bool isMovePlayerThroughDoors = false;
    private int doorLayer = 12;
    private int wallLayer = 9;

    private void Start()
    {
        playerCollider = GetComponent<CircleCollider2D>();
        canMove = true;
    }

    private void FixedUpdate()
    {
        Movement();
    }

    void Update()
    {
        movePlayerThroughDoors();

        if (!canMove) return;

        horizontalInput = Input.GetAxis("Horizontal");
        verticalInput = Input.GetAxis("Vertical");

        KeyHandle();
    }

    private void KeyHandle()
    {
        if (horizontalInput == 0 && verticalInput == 0) {
            movingDistance = 0;
        }
    }

    private void Movement()
    {
        movingDistance = Time.deltaTime * movingSpeed;
        if (horizontalInput != 0) {
            gameObject.transform.Translate(Vector2.right * horizontalInput * movingDistance);
        }
        if (verticalInput != 0) {
            gameObject.transform.Translate(Vector2.up * verticalInput * movingDistance);
        }
        if (horizontalInput != 0 || verticalInput != 0) {
            TriggerObserverActions(PlayerActions.Walk);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == doorLayer) {
            getDoorDirection(collision);
            TriggerObserverActions(PlayerActions.TouchingDoor);
            StartCoroutine("moveToNextRoom");
        }
        if (collision.gameObject.layer == wallLayer) {
            TriggerObserverActions(PlayerActions.TouchingWall);
        }
    }

    private void getDoorDirection(Collision2D collision)
    {
        touchedDoorDirection = collision.gameObject.GetComponent<Door>().Direction;
    }

    IEnumerator moveToNextRoom()
    {
        canMove = false;
        playerCollider.isTrigger = true;
        newRoomEnterPosition = getNewRoomEnterPosition();
        isMovePlayerThroughDoors = true;
        new WaitForSeconds(0.5f);
        canMove = true;
        yield return null;
    }

    private Vector3 getNewRoomEnterPosition()
    {
        Vector3 targetPosition = transform.position;
        switch(touchedDoorDirection) {
            case Direction.Top:
                targetPosition.y += 3;
                break;
            case Direction.Bottom:
                targetPosition.y -= 3;
                break;
            case Direction.Left:
                targetPosition.x -= 3;
                break;
            case Direction.Right:
                targetPosition.x += 3;
                break;
        }
        return targetPosition;
    }

    private void movePlayerThroughDoors()
    {
        if (!isMovePlayerThroughDoors) return;

        transform.position = Vector3.MoveTowards(transform.position, newRoomEnterPosition, Time.deltaTime * movingSpeed);
        if (transform.position == newRoomEnterPosition) {
            isMovePlayerThroughDoors = false;
            playerCollider.isTrigger = false;
        }
    }
}

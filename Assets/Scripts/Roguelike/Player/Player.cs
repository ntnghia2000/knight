using UnityEngine;

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

    private float horizontalInput;
    private float verticalInput;
    private float movingDistance = 0;

    private bool isMoving;
    private bool isMovingLeft;
    private bool isMovingRight;
    private bool isMovingUp;
    private bool isMovingDown;
    private int doorLayer = 12;
    private int wallLayer = 9;

    private void FixedUpdate()
    {
        Movement();
    }

    void Update()
    {
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
            TriggerObserverActions(PlayerActions.TouchingDoor);
        }
        if (collision.gameObject.layer == wallLayer) {
            TriggerObserverActions(PlayerActions.TouchingWall);
        }
    }
}

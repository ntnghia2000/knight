using UnityEngine;

public class Player : MonoBehaviour
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

    void Start()
    {
        
    }

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
       
    }
}

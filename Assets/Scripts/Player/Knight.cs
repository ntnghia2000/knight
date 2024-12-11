using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Knight : MonoBehaviour
{
    [Header("Collision")]
    [SerializeField] private LayerMask whatIsGround;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private Transform wallCheck;
    [SerializeField] private Transform backCheck;
    [SerializeField] private float checkRadius;

    [Header("Physic variable")]
    [SerializeField] private float moveSpeed;
    [SerializeField] private float jumpForce;
    [SerializeField] private float dashDistance = 8f;
    [SerializeField] private float decelerateDistance = 8f;
    [SerializeField] private float knockBackForce = 0.5f;
    [SerializeField] private ParticleSystem dust;

    [Header("Timer")]
    [SerializeField] private float dashingTime = 0.4f;
    [SerializeField] private float stopAttackTime = 0.3f;
    [SerializeField] private float HurtingTimeCoolDown = 1.5f;
    [SerializeField] private float decelerateTime = 0.4f;

    Rigidbody2D _knightRigidBody;
    Animator _knightAnimator;

    private bool isGround;
    private bool isWall;
    private bool backTouching;
    private bool isAttack;
    private bool isDead;
    private bool facingRight = true;
    private bool canJump;
    private bool canMove;
    private bool falling;
    private bool isDashing;
    private bool isHurting = false;
    private bool stopAttack = false;
    private bool jumpAttack;
    private bool spamDusk;
    private bool dashCheck = true;
    private bool isDecelerate = false;
    private bool canDoubleJump = false;

    private float horizontalInput;
    private float gravity;
    
    private LevelManager levelManager;

    private void Awake() {
        _knightRigidBody = GetComponent<Rigidbody2D>();
        _knightAnimator = GetComponent<Animator>();
        gravity = _knightRigidBody.gravityScale;
        isDead = false;
    }

    private void Start() {
        if (GameObject.FindGameObjectWithTag("LevelManager") != null) {
            levelManager = GameObject.FindGameObjectWithTag("LevelManager").GetComponent<LevelManager>();
            transform.position = levelManager.respawnPoint;
        }
    }

    private void FixedUpdate() {
        isGround = Physics2D.OverlapCircle(groundCheck.position, checkRadius, whatIsGround);
        isWall = Physics2D.OverlapCircle(wallCheck.position, checkRadius, whatIsGround);
        backTouching = Physics2D.OverlapCircle(backCheck.position, checkRadius, whatIsGround);

        updateKnightAnimation();

        if (isDashing == false && isDead == false && isHurting == false && horizontalInput != 0) {
            MoveHandle();
        }
    }

    void MoveHandle() {
        
        if (Input.GetKey(KeyCode.LeftArrow)) {
            if (!_knightAnimator.GetCurrentAnimatorStateInfo(0).IsTag("Attack")) {
                _knightRigidBody.linearVelocity = new Vector2(moveSpeed * horizontalInput, _knightRigidBody.linearVelocity.y);
            }
            if (facingRight) {
                Flip();
            }
            
        } else if (Input.GetKey(KeyCode.RightArrow)) {
            if (!_knightAnimator.GetCurrentAnimatorStateInfo(0).IsTag("Attack")) {
                _knightRigidBody.linearVelocity = new Vector2(moveSpeed * horizontalInput, _knightRigidBody.linearVelocity.y);
            }
            if (!facingRight) {
                Flip();
            }
        } else {
            if (_knightRigidBody) {
                _knightRigidBody.linearVelocity = new Vector2(0f, _knightRigidBody.linearVelocity.y);
            }
        }
    }

    private void updateKnightAnimation() {
        if (_knightAnimator) {
            _knightAnimator.SetBool("isGround", isGround);
            _knightAnimator.SetFloat("yVelocity", _knightRigidBody.linearVelocity.y);
            _knightAnimator.SetBool("walk", canMove);
            _knightAnimator.SetBool("attack", isAttack);
            _knightAnimator.SetBool("dash", isDashing);
            _knightAnimator.SetBool("jumpAttack", jumpAttack);
            _knightAnimator.SetBool("jump", canJump);
            _knightAnimator.SetBool("fall", falling);
        }
    }

    void Update() {
        horizontalInput = Input.GetAxis("Horizontal");

        KeyHandle();
        HandleOnAir();
        CheckingKnightBehaviours();
    }

    private void CheckingKnightBehaviours() {
        if (isGround && jumpAttack) {
            jumpAttack = false;
        }

        if (horizontalInput != 0) {
            canMove = true;
        } else {
            canMove = false;
        }

        if (isDashing == true && isWall == true) {
            isDashing = false;
        }
    }

    private void KeyHandle() {
        if (isDead == false) {
            if (Input.GetKeyDown(KeyCode.Space) && backTouching == false) {
                Jump();
                DoubleJump();
            }
            if (Input.GetKeyDown(KeyCode.X)) {
                if (stopAttack == false) {
                    Attack();
                    StartCoroutine(StopAttack());
                }
            }
            if (Input.GetKeyDown(KeyCode.Z)) {
                bool canDash = isGround == true && isDashing == false && canMove == true;
                if (canDash) {
                    isDashing = true;
                    if (facingRight == true) {
                        StartCoroutine(Dash(1));
                    } else {
                        StartCoroutine(Dash(-1));
                    }
                }
            }

            if (Input.GetKeyUp(KeyCode.LeftArrow)) {
                isDecelerate = true;
                StartCoroutine(Deceleration(-1f));
            }
            if (Input.GetKeyUp(KeyCode.RightArrow)) {
                isDecelerate = true;
                StartCoroutine(Deceleration(1f));
            }
        }
    }

    public void SetIsDead(bool var) {
        isDead = var;
    }

    public bool GetIsDead() {
        return isDead;
    }

    public bool GetIsHurting() {
        return isHurting;
    }

    private void Flip() {
        if (backTouching == false) {
            CreateDust();
            facingRight = !facingRight;
            Vector3 scaler = transform.localScale;
            scaler.x *= -1;
            transform.localScale = scaler;
        }
    }

    private void Jump() {
        if (isGround == true && isDashing == false) {
            _knightRigidBody.linearVelocity = Vector2.up * jumpForce;
            canJump = true;
            canDoubleJump = true;
        }
    }

    private void DoubleJump() {
        if (isGround == false && canDoubleJump == true) {
            _knightRigidBody.linearVelocity = Vector2.up * jumpForce;
            canDoubleJump = false;
        }
    }

    private void HandleOnAir() {
        if (!isGround) {
            _knightAnimator.SetLayerWeight(1, 1);
        } else {
            _knightAnimator.SetLayerWeight(1, 0);
            falling = false;
        }

        if (_knightRigidBody.linearVelocity.y < 0) {
            falling = true;
            canJump = false;
        }
    }

    void Attack() {
        if (isGround == true && !this._knightAnimator.GetCurrentAnimatorStateInfo(0).IsTag("Attack")) {
            isAttack = true;
        }

        if (isGround == false && !this._knightAnimator.GetCurrentAnimatorStateInfo(1).IsName("jumpattack")) {
            jumpAttack = true;
        } else {
            jumpAttack = false;
        }
    }

    public bool GetIsWall() {
        return isWall;
    }

    public void setAttackToFalse() {
        isAttack = false;
    }

    public void setJumpAttackToFalse() {
        jumpAttack = false;
    }

    public void Dead() {
        if (_knightAnimator){
            _knightAnimator.SetTrigger("Die");
        }

        if (_knightRigidBody){
            _knightRigidBody.linearVelocity = new Vector2(0f, 0f);
        }
    }

    public bool canShoot(){
        return isGround || isWall || (!isGround && !isWall);
    }

    public void DoKnockBack(Vector3 objectPos) {
        if (objectPos.x >= transform.position.x && transform.localScale.x < 0) {
            Flip();
        } else if (objectPos.x <= transform.position.x && transform.localScale.x > 0) {
            Flip();
        }
        
        _knightRigidBody.linearVelocity = new Vector2(-transform.localScale.x * knockBackForce, knockBackForce);
        StartCoroutine(HurtingTime(HurtingTimeCoolDown));
    }

    private IEnumerator Dash(float dir) {
        if (isDashing == true) {
            //CreateDust();
            _knightRigidBody.linearVelocity = new Vector2(_knightRigidBody.linearVelocity.x, 0f);
            _knightRigidBody.AddForce(new Vector2(dashDistance * dir, 0f), ForceMode2D.Impulse);
        }
        yield return new WaitForSeconds(dashingTime);
        isDashing = false;
        _knightRigidBody.AddForce(new Vector2(0f, 0f));
    }

    private IEnumerator Deceleration(float dir) {
        if (isDecelerate == true) {
            _knightRigidBody.linearVelocity = new Vector2(_knightRigidBody.linearVelocity.x, 0f);
            _knightRigidBody.AddForce(new Vector2(decelerateDistance + (dir * Time.deltaTime), 0f), ForceMode2D.Impulse);
        }
        yield return new WaitForSeconds(decelerateTime);
        isDecelerate = false;
    }

    private IEnumerator StopAttack() {
        stopAttack = true;
        yield return new WaitForSeconds(stopAttackTime);
        stopAttack = false;
    }

    private IEnumerator HurtingTime(float time) {
        isHurting = true;
        yield return new WaitForSeconds(time);
        isHurting = false;
    }

    private void OnTriggerEnter2D(Collider2D col) {
        if (col.tag == "CheckPoint") {
            HideIntro.hideText = true;
            levelManager.UpdateCheckPoint(transform.position, Life.lifesCount, Score.gemAmount);
        } 
        if (col.tag == "DeadZone") {
            isDead = true;
        }
        if (col.tag == "Finish") {
            SceneManager.LoadScene(0);
        }
    }

    private void CreateDust() {
        //dust.Play();
    }
}

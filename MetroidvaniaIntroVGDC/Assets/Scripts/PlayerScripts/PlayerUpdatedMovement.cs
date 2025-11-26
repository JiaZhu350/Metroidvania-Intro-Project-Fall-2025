
using System;
using Unity.VisualScripting.ReorderableList;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Interactions;

public class PlayerUpdatedMovement : MonoBehaviour
{
    public InputSystem_Actions actions;

    [Header("Player Movement Settings:")]
    [SerializeField] public float speed;

    [SerializeField] public float jumpForce;

    [SerializeField] public float highJumpForce;

    [Header("Ground & Wall Check Settings:")]
    public Transform groundCheckTransform;

    public Transform leftWallCheckTransform;

    public Transform rightWallCheckTransform;

    public Vector2 groundCheckSize;

    public Vector2 leftWallSize;

    public Vector2 rightWallSize;

    bool doubleJumpUsed;

    bool doubleJumpMidAir;
    bool doubleJumpMidAirUsed;

    public LayerMask groundLayer;

    public LayerMask leftWallLayer;
    public LayerMask rightWallLayer;

    public SpriteRenderer spriteRenderer;
    bool isGrounded;
    bool isTouchingLeftWall;
    bool isTouchingRightWall;

    bool normal = false;
    float previousTime = 0;
    float currentTime = 0;
    float newTime = 0;
    public float move;
    bool moveCondition = false;


    private float previousMove;

    public PlayerTongueGun grapplingGun;

    private float actualTime;
    public Rigidbody2D rb;

    public bool performed;
    public bool canceled;

    public PlayerAnimations playerAnimations;

    private Animator animator;

    public float gravity;

    public float maxFallSpeed;

    void Awake()
    {
        actions = new InputSystem_Actions();
    }
    void OnEnable()
    {
        actions.Player.Enable();
        actions.Player.Move.performed += InitializeMovement;
        actions.Player.Jump.performed += NormalJumping;
        actions.Player.Climb.performed += ClimbingCheck;
        

        actions.Player.Move.canceled += InitializeMovement;
        actions.Player.Jump.canceled += NormalJumping;
        actions.Player.Climb.canceled += ClimbingCheck;
        
    }

    void OnDisable()
    {
        actions.Player.Disable();
        actions.Player.Move.performed -= InitializeMovement;
        actions.Player.Jump.performed -= NormalJumping;
        actions.Player.Climb.performed -= ClimbingCheck;

    }

    //MOVEMENT MECHANICS
    void InitializeMovement(InputAction.CallbackContext ctx)
    {
        move = ctx.ReadValue<Vector2>().x;
        performed = ctx.performed;
        canceled = ctx.canceled;
        if (ctx.performed)
        {
            animator.SetTrigger("TrigRun");
            moveCondition = true;
            previousTime = 0;
        }
        if (ctx.canceled)
        {
            //FIX TRIG IDLE TURNING ON WHEN MASHING A/D QUICKLY
            //Transition animations in same animator controller smoother. (Youtube)
            animator.SetTrigger("TrigIdle");
            moveCondition = false;
            previousTime = 0;
        }
    }
    //Works but need to stop fast if change direction
    private void FlipCharacterX()
    {
        if (move == 1)
        {
            //Moving Right
            spriteRenderer.flipX = false;
        }
        if (move == -1)
        {
            //Moving Left
            spriteRenderer.flipX = true;
        }
    }
    void Movement()
    {
        float movementSpeed = move * speed;
        float fastMovementSpeed = movementSpeed * 2f;
        float updatedVelocity = grapplingGun.updatedVelocity;
        if(rb.linearVelocityY < 0)
        {
            if(rb.linearVelocityY < maxFallSpeed*-1f)
            {
                rb.linearVelocityY = maxFallSpeed*-1f;
            }
            rb.gravityScale = 1.5f;
            //Debug.Log("Gravity: " + rb.gravityScale);
        }
        if(rb.linearVelocityY > 0)
        {
            rb.gravityScale = 1f;
        }
        if(moveCondition && (updatedVelocity > 0))
        {
            //Test later(swing keeps momentum of run if continosuly holding down the key)
        }
        if (moveCondition)
        {
            if (newTime >= 1)
            {
                //Debug.Log("FAST");
                if (previousMove != move)
                {
                    previousTime = 0;
                    newTime = 0;
                }
                rb.linearVelocityX = Mathf.Lerp(movementSpeed, fastMovementSpeed, actualTime);
                previousMove = move;
            }
            if (newTime < 1)
            {
                //Debug.Log("SLOW");
                if (previousMove != move)
                {
                    previousTime = 0;
                    newTime = 0;
                }
                rb.linearVelocityX = movementSpeed;
                currentTime = previousTime;
                newTime = currentTime + Time.deltaTime;
                previousTime = newTime;
                previousMove = move;
            }
        }
        if (!moveCondition && isGrounded)
        {
            //Debug.Log("SLOWWWdown");
            rb.linearVelocityX = Mathf.Lerp(rb.linearVelocityX, 0, actualTime);
            previousTime = 0;
            newTime = 0;
        }
        if (!moveCondition && !isGrounded)
        {
            TimeFunction();
            rb.linearVelocityX = Mathf.Lerp(updatedVelocity, 0, actualTime*0.1f);
            previousTime = 0;
            newTime = 0;
        }
    }
    private bool condition;
    //JUMPING MECHANICS
    void NormalJumping(InputAction.CallbackContext ctx)
    {
        if (ctx.performed)
        {
            DoubleJumpChecker();
            condition = true;
            //Debug.Log("PRINTS");
            falling = false;
            isJumping=true;
        }
        if (ctx.canceled)
        {
            isJumping = false;
            condition = false;
            ResetTime();
            //Debug.Log("GG");
        }
    }
    private bool falling = false;
    void JumpForce()
        {
            Debug.Log(condition);
            if(condition && (rb.linearVelocityY != highJumpForce) && !falling && (doubleJumpCounter == 1))
            {
                //Debug.Log("RISE");
                rb.linearVelocityY = Mathf.Lerp(jumpForce,highJumpForce,currentTimeJump*0.5f);
                doubleJumpUsed = false;
            }
            if(condition && (rb.linearVelocityY == highJumpForce) && (doubleJumpCounter == 1))
            {
                falling = true;
                //Debug.Log("FALL");
                //Debug.Log("g "+rb.gravityScale);
            }
            if(condition && doubleJumpCounter == 0 && !doubleJumpUsed)
            {
                rb.linearVelocityY = jumpForce;
                doubleJumpUsed = true;
            }
            //Debug.Log("rb.linear"+rb.linearVelocityY);
            //Debug.Log("Time: " + currentTimeJump);
            //Debug.Log("counter" + doubleJumpCounter);
        }
    private int doubleJumpCounter = 1;
    void JumpingMechanic()
    {
        if (isGrounded || isTouchingLeftWall || isTouchingRightWall)
        {
            DoubleJumpCounterReset();
            JumpForce();
        }
        if (!isGrounded && !isTouchingLeftWall && !isTouchingRightWall)
        {
            JumpForce();
        }
    }
    void DoubleJumpCounterReset()
    {
        doubleJumpCounter = 1;
    }
    void DoubleJumpChecker()
    {
        if(!condition)
        {
            doubleJumpCounter --;
        }
    }

    private bool isJumping;
    public float jumpHangTimeThreshold;

    public float jumpHangTimeGravity;
    void AirTime()
    {
        if((isJumping || isTouchingLeftWall || isTouchingRightWall) && Mathf.Abs(rb.linearVelocityY) < jumpHangTimeThreshold)
        {
            rb.gravityScale = rb.gravityScale * jumpHangTimeGravity;
            Debug.Log("gravity fall "+rb.gravityScale);
        }
    }


    //CLIMBING MECHANICS
    void ClimbingCheck(InputAction.CallbackContext ctx)
    {
        //float gravity = rb.gravityScale;
        if (ctx.performed && (isTouchingLeftWall || isTouchingRightWall))
        {
            //rb.linearVelocityY = gravity;
        }
    }
    void Climbing()
    {
        if (isTouchingLeftWall && move == -1)
        {
            rb.linearVelocityX = 0;
        }
        if(isTouchingRightWall && move == 1)
        {
            rb.linearVelocityX = 0;
        }
    }
    
    float currentTimeJump = 0;
    float previousTimeJump = 0;
    float newTimeJump = 0;
    void TimeFunction()
    {
        if(condition)
        {
            //Debug.Log(currentTimeJump);
            currentTimeJump = previousTimeJump;
            newTimeJump = currentTimeJump + Time.deltaTime;
            previousTimeJump = newTimeJump;
        }
    }
    void ResetTime()
    {
        currentTimeJump = 0;
        newTimeJump = 0;
        previousTimeJump = 0;
    }

    //BASE MECHANICS
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame

    void Update()
    {
        //Debug.Log("Velocity: " + rb.linearVelocityY);
        TimeFunction();
        AirTime();
        JumpingMechanic();
        Movement();
        FlipCharacterX();
        isGrounded = Physics2D.OverlapBox(groundCheckTransform.position, groundCheckSize, 0f, groundLayer);
        isTouchingLeftWall = Physics2D.OverlapBox(leftWallCheckTransform.position, leftWallSize, 0f, leftWallLayer);
        isTouchingRightWall = Physics2D.OverlapBox(rightWallCheckTransform.position, rightWallSize, 0f, rightWallLayer);
        Climbing();

    }
    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(groundCheckTransform.position, groundCheckSize);
        Gizmos.DrawWireCube(leftWallCheckTransform.position,leftWallSize);
        Gizmos.DrawWireCube(rightWallCheckTransform.position, rightWallSize);
    }
}


using System;
using Unity.VisualScripting.ReorderableList;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Interactions;

public class PlayerMovement : MonoBehaviour
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
    public bool isGrounded;
    public bool isTouchingLeftWall;
    public bool isTouchingRightWall;

    bool normal = false;
    float previousTime = 0;
    float currentTime = 0;
    float newTime = 0;

    bool high = false;
    public float move;
    float climb;
    bool moveCondition = false;

    public float climbSpeed;


    private float previousMove;

    public PlayerTongueGun grapplingGun;

    private float time;
    private float actualTime;
    public Rigidbody2D rb;

    public bool performed;
    public bool canceled;

    public PlayerAnimations playerAnimations;

    private Animator animator;

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
            moveCondition = true;
            previousTime = 0;
        }
        if (ctx.canceled)
        {
            //FIX TRIG IDLE TURNING ON WHEN MASHING A/D QUICKLY
            //Transition animations in same animator controller smoother. (Youtube)
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
        Debug.Log(moveCondition);
        if(moveCondition && (updatedVelocity > 0))
        {
            //Test later(swing keeps momentum of run if continosuly holding down the key)
        }
        if (moveCondition)
        {
            if (newTime >= 1)
            {
                Debug.Log("FAST");
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
                Debug.Log("SLOW");
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
            Debug.Log("SLOWWWdown");
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
    void TimeFunction()
    {
        time += Time.deltaTime;
        actualTime = time % 60f;
        if(actualTime > 1)
        {
            time = 0;
        }
    }

    //JUMPING MECHANICS
    public bool jumpPerformed = false;
    void NormalJumping(InputAction.CallbackContext ctx)
    {
        if (ctx.performed)
        {
            jumpPerformed = true;
            if (ctx.interaction is PressInteraction)
            {
                normal = true;
                high = false;
                JumpingMechanic();
            }
            if (ctx.interaction is HoldInteraction)
            {
                normal = false;
                high = true;
                JumpingMechanic();
            }
        }
        if(ctx.canceled)
        {
            jumpPerformed = false;
        }
    }
   
    void JumpingMechanic()
    {
        if (isGrounded || isTouchingLeftWall || isTouchingRightWall)
        {
            JumpForce();
            doubleJumpUsed = false;
        }
        if (!isGrounded && (!doubleJumpUsed || doubleJumpMidAir) && !isTouchingLeftWall && !isTouchingRightWall && !doubleJumpMidAirUsed)
            {
                JumpForce();
                doubleJumpUsed = true;
                doubleJumpMidAirUsed = true;
            }
    }
    void JumpForce()
        {
            if (normal)
            {
            rb.linearVelocityY = jumpForce;
                normal = false;
            }
            if (high && isGrounded)
            {
            rb.linearVelocityY = highJumpForce;
            high = false;
            }
            else
            {
            rb.linearVelocityY = jumpForce;
            }
        }
    void DoubleJumpMidAirCheck()
    {
        if (!isGrounded && !isTouchingLeftWall && !isTouchingRightWall)
        {
            doubleJumpMidAir = true;
        }
        else
        {
            doubleJumpMidAir = false;
            doubleJumpMidAirUsed = false;
        }
    }
    


    //CLIMBING MECHANICS
    void ClimbingCheck(InputAction.CallbackContext ctx)
    {
        float gravity = rb.gravityScale;
        Debug.Log(gravity);
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
        Movement();
        FlipCharacterX();
        isGrounded = Physics2D.OverlapBox(groundCheckTransform.position, groundCheckSize, 0f, groundLayer);
        isTouchingLeftWall = Physics2D.OverlapBox(leftWallCheckTransform.position, leftWallSize, 0f, leftWallLayer);
        isTouchingRightWall = Physics2D.OverlapBox(rightWallCheckTransform.position, rightWallSize, 0f, rightWallLayer);
        //Climbing();
        DoubleJumpMidAirCheck();

    }
    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(groundCheckTransform.position, groundCheckSize);
        Gizmos.DrawWireCube(leftWallCheckTransform.position,leftWallSize);
        Gizmos.DrawWireCube(rightWallCheckTransform.position, rightWallSize);
    }
}

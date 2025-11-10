
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Interactions;

public class PlayerMovement : MonoBehaviour
{
    public InputSystem_Actions actions;

    public float speed;

    public float jumpForce;

    public float highJumpForce;

    public Transform groundCheckTransform;

    public Transform leftWallCheckTransform;

    public Transform rightWallCheckTransform;

    public float groundCheckRadius;

    public float leftWallCheckRadius;

    public float rightWallCheckRadius;

    public bool doubleJumpUnlocked;

    public bool climbUnlocked;

    bool doubleJumpUsed;

    bool doubleJumpMidAir;
    bool doubleJumpMidAirUsed;

    public LayerMask groundLayer;

    public LayerMask leftWallLayer;
    public LayerMask rightWallLayer;
    bool isGrounded;
    bool isTouchingLeftWall;
    bool isTouchingRightWall;

    bool normal = false;

    bool high = false;
    float move;
    float climb;

    public float climbSpeed;


    Rigidbody2D rb;

    void Awake()
    {
        actions = new InputSystem_Actions();
    }
    void OnEnable()
    {
        actions.Player.Enable();
        actions.Player.Move.performed += Movement;
        actions.Player.Jump.performed += NormalJumping;
        actions.Player.Move.performed += ClimbingCheck;
        

        actions.Player.Move.canceled += Movement;
        actions.Player.Jump.canceled += NormalJumping;
        actions.Player.Move.canceled += ClimbingCheck;
        
    }

    void OnDisable()
    {
        actions.Player.Disable();
        actions.Player.Move.performed -= Movement;
        actions.Player.Jump.performed -= NormalJumping;
        actions.Player.Move.performed -= ClimbingCheck;
        
    }
    void Movement(InputAction.CallbackContext ctx)
    {
        move = ctx.ReadValue<Vector2>().x;
    }
    void NormalJumping(InputAction.CallbackContext ctx)
    {
        if (ctx.performed)
        {
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
    }
   
    void JumpingMechanic()
    {
        if (isGrounded || isTouchingLeftWall || isTouchingRightWall)
        {
            JumpForce();
            doubleJumpUsed = false;
        }

        if (doubleJumpUnlocked)
        {
            if (!isGrounded && (!doubleJumpUsed || doubleJumpMidAir) && !isTouchingLeftWall && !isTouchingRightWall &&
                !doubleJumpMidAirUsed)
            {
                JumpForce();
                doubleJumpUsed = true;
                doubleJumpMidAirUsed = true;
            }
        }
    }
    void JumpForce()
        {
            if (normal)
            {
            rb.linearVelocityY = jumpForce;
                normal = false;
            }
            if (high)
            {
            rb.linearVelocityY = highJumpForce;
                high = false;
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
    
    void ClimbingCheck(InputAction.CallbackContext ctx)
    {
        if (climbUnlocked)
        {
            climb = ctx.ReadValue<Vector2>().y;
        }
    }
    void Climbing()
    {
        if (isTouchingLeftWall || isTouchingRightWall)
        {
            rb.linearVelocityY = climb * climbSpeed;
        }
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        isGrounded = Physics2D.OverlapCircle(groundCheckTransform.position, groundCheckRadius, groundLayer);
        rb.linearVelocityX = move * speed;
        if (climbUnlocked)
        {
            isTouchingLeftWall = Physics2D.OverlapCircle(leftWallCheckTransform.position, leftWallCheckRadius, leftWallLayer);
            isTouchingRightWall = Physics2D.OverlapCircle(rightWallCheckTransform.position, rightWallCheckRadius, rightWallLayer);
            Climbing();
        }
        if (doubleJumpUnlocked)
        {
            DoubleJumpMidAirCheck();
        }

    }
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(groundCheckTransform.position, groundCheckRadius);
        Gizmos.DrawWireSphere(leftWallCheckTransform.position, leftWallCheckRadius);
        Gizmos.DrawWireSphere(rightWallCheckTransform.position, rightWallCheckRadius);
    }
}

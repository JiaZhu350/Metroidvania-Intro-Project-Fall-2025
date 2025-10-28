
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    public InputSystem_Actions actions;

    public float speed;

    public float jumpforce;

    public Transform groundCheckTransform;

    public Transform leftWallCheckTransform;

    public Transform rightWallCheckTransform;

    public float groundCheckRadius;

    public float leftWallCheckRadius;

    public float rightWallCheckRadius;

    bool doubleJumpUsed;

    bool doubleJumpMidAir;
    bool doubleJumpMidAirUsed;

    public LayerMask groundLayer;

    public LayerMask leftWallLayer;
    public LayerMask rightWallLayer;
    bool isGrounded;
    bool isTouchingLeftWall;
    bool isTouchingRightWall;
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
        actions.Player.Jump.performed += Jumping;
        actions.Player.Move.performed += ClimbingCheck;

        actions.Player.Move.canceled += Movement;
        actions.Player.Jump.canceled += Jumping;
        actions.Player.Move.canceled += ClimbingCheck;
    }

    void OnDisable()
    {
        actions.Player.Disable();
        actions.Player.Move.performed -= Movement;
        actions.Player.Jump.performed -= Jumping;
        actions.Player.Move.performed -= ClimbingCheck;

    }
    void Movement(InputAction.CallbackContext ctx)
    {
        move = ctx.ReadValue<Vector2>().x;
    }
    void Jumping(InputAction.CallbackContext ctx)
    {
        jumpforce = ctx.duration * 2;
        if (ctx.performed)
        {
            if (isGrounded || isTouchingLeftWall || isTouchingRightWall)
            {
                rb.linearVelocityY = jumpforce;
                doubleJumpUsed = false;
            }
            if (!isGrounded && (!doubleJumpUsed || doubleJumpMidAir) && !isTouchingLeftWall && !isTouchingRightWall && !doubleJumpMidAirUsed)
            {
                rb.linearVelocityY = jumpforce;
                doubleJumpUsed = true;
                doubleJumpMidAirUsed = true;
            }

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
        climb = ctx.ReadValue<Vector2>().y;
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
        isTouchingLeftWall = Physics2D.OverlapCircle(leftWallCheckTransform.position, leftWallCheckRadius, leftWallLayer);
        isTouchingRightWall = Physics2D.OverlapCircle(rightWallCheckTransform.position, rightWallCheckRadius, rightWallLayer);
        rb.linearVelocityX = move * speed;
        Climbing();
        DoubleJumpMidAirCheck();
    }
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(groundCheckTransform.position, groundCheckRadius);
        Gizmos.DrawWireSphere(leftWallCheckTransform.position, leftWallCheckRadius);
        Gizmos.DrawWireSphere(rightWallCheckTransform.position, rightWallCheckRadius);
    }
}

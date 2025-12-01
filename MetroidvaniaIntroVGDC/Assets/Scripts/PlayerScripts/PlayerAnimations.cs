using System.Runtime.CompilerServices;
using UnityEngine;
public class PlayerAnimations : MonoBehaviour
{
    private Animator animator;
    private float move;

    private float velocity_x;
    private float velocity_y;

    public bool performed;
    public bool canceled;

    private bool isGrounded;
    private bool isTouchingLeftWall;
    private bool isTouchingRightWall;
    public PlayerMovement playerMovement;
    public PlayerClawAttack playerClawAttack;
    public PlayerHealth playerHealth;
    void Start()
    {
        animator = GetComponent<Animator>();
    }
    public void MovingAnimation()
    {
        if(velocity_x == 0)
        {
            animator.SetBool("Running", false);
        }
        if(Mathf.Abs(velocity_x) > 0.1f)
        {
            animator.SetBool("Running", true);
        }
    }
    public void JumpingAndFallingAnimation()
    {
       if (isGrounded)
        {
            animator.SetBool("Jumping", false);
            animator.SetBool("Falling", false);
        }
        if(Mathf.Abs(velocity_y) > 0.1f && !isGrounded)
        {
            if(velocity_y > 0)
            {
                animator.SetBool("Jumping", true);
                animator.SetBool("Falling", false);
            }
            if(velocity_y <= 0)
            {
                animator.SetBool("Falling", true);
                animator.SetBool("Jumping", false);
            }
        } 
    }
    public void WallJumpingAnimation()
    {
         if(isTouchingLeftWall || isTouchingRightWall)
        {
            if(!isGrounded)
            {
                animator.SetBool("WallJumping", true);
                if(isTouchingLeftWall)
                {
                    playerMovement.spriteRenderer.flipX = false;
                }
                if(isTouchingRightWall)
                {
                    playerMovement.spriteRenderer.flipX = true;
                }
            }
            else
            {
                animator.SetBool("WallJumping", false);
            }
        }
        else
        {
            animator.SetBool("WallJumping", false);
        }
    }
    public void AttackAnimation()
    {
        if(playerClawAttack.performed)
        {
            animator.SetBool("Attacking", true);
        }
        else
        {
            animator.SetBool("Attacking", false);
        }
    }
    public void DamageAnimation()
    {
        if(playerHealth.currentHealth < playerHealth.previousHealth)
        {
            animator.SetBool("Damage", true);
        }
        else
        {
            animator.SetBool("Damage", false);
        }
    }
    public void DeathAnimation()
    {
        if(playerHealth.dead)
        {
            animator.SetBool("Dead", true);
        }
        else
        {
            animator.SetBool("Dead", false);
        }
    }
    // Update is called once per frame
    void Update()
    {
        move = playerMovement.move;
        velocity_x = playerMovement.rb.linearVelocityX;
        velocity_y = playerMovement.rb.linearVelocityY;
        performed = playerMovement.performed;
        canceled = playerMovement.canceled;
        isGrounded = playerMovement.isGrounded;
        isTouchingLeftWall = playerMovement.isTouchingLeftWall;
        isTouchingRightWall = playerMovement.isTouchingRightWall;
    }
    void FixedUpdate()
    {
        MovingAnimation();
        JumpingAndFallingAnimation();
        WallJumpingAnimation();
        AttackAnimation();
        DamageAnimation();
        DeathAnimation();
    }
}
using System.Runtime.CompilerServices;
using NUnit.Framework;
using NUnit.Framework.Constraints;
using UnityEngine;
public class PlayerAnimations : MonoBehaviour
{
    private Animator animator;
    private float move;
    private string currentState;
    private float velocity_x;
    private float velocity_y;

    private float healthDifference;

    public bool performed;
    public bool canceled;

    private bool attackPerformed;

    private bool isGrounded;
    private bool isTouchingLeftWall;
    private bool isTouchingRightWall;
    private float currentHealth;
    private float updatedHealth;
    private Sprite spriteff;
    //private Sprite attackSprite = 0013;
    public PlayerMovement playerMovement;
    public PlayerClawAttack playerClawAttack;
    public PlayerHealth playerHealth;

    private bool attackCondition = false;
    const string Running = "Run";
    const string Jumping = "Jump";
    const string Falling = "Falling";
    const string WallJump = "WallJump";
    const string Attacking = "Attack";
    const string Damage = "Damage";
    const string Dead = "Death";
    const string Idle = "Idle";


    void Start()
    {
        animator = GetComponent<Animator>();
        updatedHealth = playerHealth.currentHealth;
    }
    void ChangeAnimationState(string newState)
    {
        if(currentState == newState) return;
        animator.Play(newState);
        currentState = newState;
    }
    public void IdleAnimation()
    {
        if(velocity_x == 0 && isGrounded && !playerHealth.dead && currentHealth == updatedHealth && !attackCondition)
        {
            Debug.Log("Idle");
            ChangeAnimationState(Idle);
        }
    }
    public void MovingAnimation()
    {
        if(Mathf.Abs(velocity_x) != 0 && isGrounded && !playerHealth.dead && currentHealth == updatedHealth && !attackCondition)
        {
            Debug.Log("Running");
            ChangeAnimationState(Running);
        }
    }
    public void JumpingAndFallingAnimation()
    {
        if(Mathf.Abs(velocity_y) > 0.1f && !isGrounded && !playerHealth.dead && currentHealth == updatedHealth && !attackCondition)
        {
            if(velocity_y > 0)
            {
                ChangeAnimationState(Jumping);
            }
            if(velocity_y <= 0)
            {
                ChangeAnimationState(Falling);
            }
        } 
    }
    public void WallJumpingAnimation()
    {
         if(isTouchingLeftWall || isTouchingRightWall)
        {
            if(!isGrounded && !playerHealth.dead && playerHealth.currentHealth == playerHealth.previousHealth)
            {
                ChangeAnimationState(WallJump);
            }
        }
    }
    public void AttackAnimation()
    {
        if((attackPerformed || attackCondition) && !playerHealth.dead && currentHealth == updatedHealth)
        {
            ChangeAnimationState(Attacking);
            if(spriteff.name != "0013_0")
            {
                attackCondition = true;
            }
            if(spriteff.name == "0013_0")
            {
                attackCondition = false;
            }
            Debug.Log(playerMovement.spriteRenderer.sprite);
        }
    }
    public void DamageAnimation()
    {
        if(currentHealth != updatedHealth && !playerHealth.dead)
        {
            ChangeAnimationState(Damage);
        }
    }
    public void DeathAnimation()
    {
        if(playerHealth.dead)
        {
            ChangeAnimationState(Dead);
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
        currentHealth = playerHealth.currentHealth;
        attackPerformed = playerClawAttack.performed;
        spriteff = playerMovement.spriteRenderer.sprite;
        

    }
    void FixedUpdate()
    {
        IdleAnimation();
        MovingAnimation();
        JumpingAndFallingAnimation();
        WallJumpingAnimation();
        AttackAnimation();
        DamageAnimation();
        DeathAnimation();
    }
}
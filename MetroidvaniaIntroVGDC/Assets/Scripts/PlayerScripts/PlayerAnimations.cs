using System.Runtime.CompilerServices;
using UnityEngine;
public class PlayerAnimations : MonoBehaviour
{
    private Animator animator;
    private float move;

    private float velocity;

    public bool performed;
    public bool canceled;

    bool startCondition;

    bool endCondition;
    public PlayerMovement playerMovement;
    void Start()
    {
        animator = GetComponent<Animator>();
    }
    public void MovingAnimation()
    {
        if(performed || (velocity > 0) || (velocity < 0))
        {
            animator.SetTrigger("TrigRun");
        }
        if(canceled)
        {
            animator.SetTrigger("TrigIdle");
        }
    }

    void Animation()
    {
        if(performed && startCondition)
        {
            animator.SetTrigger("TrigRun");
            startCondition = false;
        }
        if(canceled && endCondition)
        {
            animator.SetTrigger("TrigRun");
        }
    }
    // Update is called once per frame
    void Update()
    {
        move = playerMovement.move;
        velocity = playerMovement.rb.linearVelocityX;
        performed = playerMovement.performed;
        canceled = playerMovement.canceled;
    }
}

using UnityEngine;

public class PlayerAnimController : MonoBehaviour
{
    public Animator animator;
    public AnimationClip idleUpAnim;
    public AnimationClip idleDownAnim;
    public AnimationClip idleSideAnim;
    public AnimationClip walkUpAnim;
    public AnimationClip walkDownAnim;
    public AnimationClip walkSideAnim;

    private Vector2 movement;
    private bool isFacingRight = true;
    private string currentState;
    private Vector2 lastNonZeroMovement;

    private void Start()
    {
        if (animator == null)
        {
            animator = GetComponent<Animator>();
        }
        PlayAnimation("IdleDown");
        currentState = "IdleDown";
        lastNonZeroMovement = Vector2.down;
    }

    private void Update()
    {
        float moveHorizontal = Input.GetAxisRaw("Horizontal");
        float moveVertical = Input.GetAxisRaw("Vertical");

        movement = new Vector2(moveHorizontal, moveVertical).normalized;
        bool isMoving = movement.magnitude > 0.1f;

        if (isMoving)
        {
            lastNonZeroMovement = movement;
        }

        string newState = DetermineAnimationState(isMoving);
        if (newState != currentState)
        {
            PlayAnimation(newState);
            currentState = newState;
        }

        if (moveHorizontal != 0)
        {
            if (moveHorizontal > 0 && isFacingRight)
            {
                Flip();
            }
            else if (moveHorizontal < 0 && !isFacingRight)
            {
                Flip();
            }
        }
    }

    private string DetermineAnimationState(bool isMoving)
    {
        if (!isMoving)
        {
            if (Mathf.Abs(lastNonZeroMovement.x) > Mathf.Abs(lastNonZeroMovement.y))
            {
                return "IdleSide";
            }
            else if (lastNonZeroMovement.y > 0)
            {
                return "IdleUp";
            }
            else
            {
                return "IdleDown";
            }
        }
        else
        {
            if (Mathf.Abs(movement.x) > Mathf.Abs(movement.y))
            {
                return "WalkSide";
            }
            else if (movement.y > 0)
            {
                return "WalkUp";
            }
            else
            {
                return "WalkDown";
            }
        }
    }

    private void PlayAnimation(string stateName)
    {
        switch (stateName)
        {
            case "IdleUp":
                if (idleUpAnim) animator.Play(idleUpAnim.name);
                break;
            case "IdleDown":
                if (idleDownAnim) animator.Play(idleDownAnim.name);
                break;
            case "IdleSide":
                if (idleSideAnim) animator.Play(idleSideAnim.name);
                break;
            case "WalkUp":
                if (walkUpAnim) animator.Play(walkUpAnim.name);
                break;
            case "WalkDown":
                if (walkDownAnim) animator.Play(walkDownAnim.name);
                break;
            case "WalkSide":
                if (walkSideAnim) animator.Play(walkSideAnim.name);
                break;
        }
    }

    private void Flip()
    {
        isFacingRight = !isFacingRight;
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }
}
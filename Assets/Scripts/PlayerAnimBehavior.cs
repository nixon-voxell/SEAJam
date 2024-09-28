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
    private Vector2 lastNonZeroMovement;
    private bool isFacingRight = true;
    private string currentState;

    private void Start()
    {
        if (animator == null)
        {
            animator = GetComponent<Animator>();
        }
        lastNonZeroMovement = Vector2.down; 
    }

    private void Update()
    {
        float moveHorizontal = Input.GetAxisRaw("Horizontal");
        float moveVertical = Input.GetAxisRaw("Vertical");

        movement = new Vector2(moveHorizontal, moveVertical).normalized;
        bool isMoving = movement.magnitude > 0;

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
            isFacingRight = (moveHorizontal < 0); 
            UpdateFacingDirection();
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
                if (idleUpAnim != null)
                    animator.Play(idleUpAnim.name);
                break;
            case "IdleDown":
                if (idleDownAnim != null)
                    animator.Play(idleDownAnim.name);
                break;
            case "IdleSide":
                if (idleSideAnim != null)
                    animator.Play(idleSideAnim.name);
                break;
            case "WalkUp":
                if (walkUpAnim != null)
                    animator.Play(walkUpAnim.name);
                break;
            case "WalkDown":
                if (walkDownAnim != null)
                    animator.Play(walkDownAnim.name);
                break;
            case "WalkSide":
                if (walkSideAnim != null)
                    animator.Play(walkSideAnim.name);
                break;
            default:
                Debug.LogWarning($"Animation state '{stateName}' not recognized.");
                break;
        }
    }

    private void UpdateFacingDirection()
    {
        Vector3 theScale = transform.localScale;
        theScale.x = Mathf.Abs(theScale.x) * (isFacingRight ? 1 : -1); 
        transform.localScale = theScale;
    }
}

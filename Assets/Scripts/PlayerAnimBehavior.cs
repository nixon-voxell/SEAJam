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

    private void Start()
    {
        if (animator == null)
        {
            animator = GetComponent<Animator>();
        }
    }

    private void Update()
    {
        float moveHorizontal = Input.GetAxisRaw("Horizontal");
        float moveVertical = Input.GetAxisRaw("Vertical");

        movement = new Vector2(moveHorizontal, moveVertical).normalized;

        // Prioritize walk down animation
        if (moveVertical < 0)
        {
            PlayAnimation("WalkDown");
            currentState = "WalkDown";
        }
        else if (moveVertical > 0)
        {
            PlayAnimation("WalkUp");
            currentState = "WalkUp";
        }
        else if (moveHorizontal != 0)
        {
            PlayAnimation("WalkSide");
            currentState = "WalkSide";
            isFacingRight = (moveHorizontal < 0);
            UpdateFacingDirection();
        }
        else if (currentState != "IdleUp")
        {
            PlayAnimation("IdleUp");
            currentState = "IdleUp";
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
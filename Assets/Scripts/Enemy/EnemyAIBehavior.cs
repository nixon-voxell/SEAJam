using UnityEngine;
using System.Collections;

public class EnemyPatrolAI : MonoBehaviour
{
    public NavMeshMini navMeshMini;
    public float moveSpeed = 2f, idleTime = 2f;
    public Animator animator;
    public AnimationClip idleUpAnim, idleDownAnim, idleSideAnim, walkUpAnim, walkDownAnim, walkSideAnim;

    private Vector3 startPosition, endPosition, movement;
    private bool movingToEnd = true;

     void Start()
    {
        if (navMeshMini == null)
        {
            Debug.LogError("NavMeshMini not assigned to EnemyPatrolAI!");
            enabled = false;
            return;
        }

        startPosition = navMeshMini.GetStartPosition();
        endPosition = navMeshMini.GetEndPosition();
        transform.position = startPosition;

        animator ??= GetComponent<Animator>();
        SetFacingDirection(!navMeshMini.startFacingRight);

        StartCoroutine(PatrolCoroutine());
    }

    IEnumerator PatrolCoroutine()
    {
        while (true)
        {
            // Move to end position
            yield return StartCoroutine(MoveToPosition(endPosition));
            
            // Idle at end position
            yield return StartCoroutine(IdleAtPosition());

            // Move back to start position
            yield return StartCoroutine(MoveToPosition(startPosition));

            // Idle at start position
            yield return StartCoroutine(IdleAtPosition());
        }
    }

    IEnumerator MoveToPosition(Vector3 targetPosition)
    {
        movement = (targetPosition - transform.position).normalized;
        while (Vector3.Distance(transform.position, targetPosition) > 0.05f)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);

            if (navMeshMini.isHorizontal)
                SetFacingDirection(movement.x < 0); // Inverted: Face left when moving right
            else
                SetFacingDirection(movement.y > 0); // Face right when moving up (unchanged for vertical movement)

            UpdateAnimation();
            yield return null;
        }
    }

    IEnumerator IdleAtPosition()
    {
        movement = Vector3.zero;
        UpdateAnimation();
        yield return new WaitForSeconds(idleTime);
    }

    void UpdateAnimation()
    {
        if (movement.magnitude < 0.1f)
        {
            PlayAnimation(navMeshMini.isHorizontal ? idleSideAnim : (movement.y >= 0 ? idleUpAnim : idleDownAnim));
        }
        else if (navMeshMini.isHorizontal)
        {
            PlayAnimation(walkSideAnim);
        }
        else
        {
            PlayAnimation(movement.y > 0 ? walkUpAnim : walkDownAnim);
        }
    }

    void PlayAnimation(AnimationClip clip)
    {
        if (clip != null)
            animator.Play(clip.name);
        else
            Debug.LogWarning("Animation clip not assigned for " + gameObject.name);
    }

    void SetFacingDirection(bool faceRight)
    {
        Vector3 newScale = transform.localScale;
        newScale.x = Mathf.Abs(newScale.x) * (faceRight ? 1 : -1);
        transform.localScale = newScale;
    }
}
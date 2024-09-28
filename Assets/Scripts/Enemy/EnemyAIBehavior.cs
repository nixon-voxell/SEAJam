using UnityEngine;
using System.Collections;

public class EnemyPatrolAI : MonoBehaviour
{
    public float patrolDistance = 5f, moveSpeed = 2f, idleTime = 2f;
    public bool isHorizontal = true, startFacingRight = true;
    public Animator animator;
    public AnimationClip idleUpAnim, idleDownAnim, idleSideAnim, walkUpAnim, walkDownAnim, walkSideAnim;

    private Vector3 startPosition, endPosition, movement;
    private bool movingToEnd = true;

    void Start()
    {
        startPosition = transform.position;
        CalculateEndPosition();
        StartCoroutine(PatrolCoroutine());
        animator ??= GetComponent<Animator>();
        SetFacingDirection(startFacingRight);
    }

    void CalculateEndPosition() => endPosition = startPosition + (isHorizontal ? Vector3.right : Vector3.up) * patrolDistance;

    IEnumerator PatrolCoroutine()
    {
        while (true)
        {
            Vector3 targetPosition = movingToEnd ? endPosition : startPosition;
            movement = (targetPosition - transform.position).normalized;

            while (Vector3.Distance(transform.position, targetPosition) > 0.05f)
            {
                transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);

                if (isHorizontal)
                    SetFacingDirection(movement.x > 0 ? true : false);  // Invert: face left when moving right and vice versa

                UpdateAnimation();
                yield return null;
            }

            movement = Vector3.zero;
            UpdateAnimation();
            yield return new WaitForSeconds(idleTime);
            movingToEnd = !movingToEnd;
        }
    }

    void UpdateAnimation()
    {
        if (movement.magnitude < 0.1f)
            PlayAnimation(idleUpAnim);
        else if (Mathf.Abs(movement.x) > Mathf.Abs(movement.y))
            PlayAnimation(walkSideAnim);
        else
            PlayAnimation(movement.y > 0 ? walkUpAnim : walkDownAnim);
    }

    void PlayAnimation(AnimationClip clip) => animator.Play(clip.name);

    void SetFacingDirection(bool faceLeft)
    {
        Vector3 newScale = transform.localScale;
        newScale.x = Mathf.Abs(newScale.x) * (faceLeft ? -1 : 1);  // Invert: now scale -1 is applied when moving right
        transform.localScale = newScale;
    }

    public void ToggleDirection()
    {
        isHorizontal = !isHorizontal;
        CalculateEndPosition();
        SetFacingDirection(!startFacingRight);
        startFacingRight = !startFacingRight;
    }
}

using UnityEngine;
using System.Collections;

public class EnemyPatrolAI : MonoBehaviour
{
    public float patrolDistance = 5f;
    public float moveSpeed = 2f;
    public float idleTime = 2f;
    public bool isHorizontal = true;

    private Vector3 startPosition;
    private Vector3 endPosition;
    private bool movingToEnd = true;

    private void Start()
    {
        startPosition = transform.position;
        CalculateEndPosition();
        StartCoroutine(PatrolCoroutine());
    }

    private void CalculateEndPosition()
    {
        if (isHorizontal)
        {
            endPosition = startPosition + Vector3.right * patrolDistance;
        }
        else
        {
            endPosition = startPosition + Vector3.up * patrolDistance;
        }
    }

    private IEnumerator PatrolCoroutine()
    {
        while (true)
        {
            Vector3 targetPosition = movingToEnd ? endPosition : startPosition;

            while (transform.position != targetPosition)
            {
                transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);
                yield return null;
            }

            yield return new WaitForSeconds(idleTime);
            movingToEnd = !movingToEnd;
        }
    }
    public void ToggleDirection()
    {
        isHorizontal = !isHorizontal;
        CalculateEndPosition();
    }
}
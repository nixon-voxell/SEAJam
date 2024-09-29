using UnityEngine;

public class NavMeshMini : MonoBehaviour
{
    public bool isHorizontal = true;
    public bool startFacingRight = true;
    
    public Vector3 startPoint = Vector3.zero;
    public Vector3 endPoint = Vector3.right * 5f;

    private void OnValidate()
    {
        UpdatePath();
    }

    private void Start()
    {
        UpdatePath();
    }

    private void UpdatePath()
    {
        // Ensure the path is either horizontal or vertical
        if (isHorizontal)
        {
            endPoint.y = startPoint.y;
            endPoint.z = startPoint.z;
        }
        else
        {
            endPoint.x = startPoint.x;
            endPoint.z = startPoint.z;
        }
    }

    public Vector3 GetStartPosition() => transform.position + startPoint;
    public Vector3 GetEndPosition() => transform.position + endPoint;

    public float GetPathLength() => Vector3.Distance(GetStartPosition(), GetEndPosition());

    private void OnDrawGizmos()
    {
        Vector3 start = GetStartPosition();
        Vector3 end = GetEndPosition();

        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(start, end);
        Gizmos.DrawSphere(start, 0.1f);
        Gizmos.DrawSphere(end, 0.1f);

        // Draw direction arrow
        Vector3 direction = (end - start).normalized;
        Vector3 arrowStart = Vector3.Lerp(start, end, 0.8f);
        Vector3 arrowEnd = Vector3.Lerp(start, end, 0.9f);
        Gizmos.DrawLine(arrowStart, arrowEnd);
        
        Vector3 right = Vector3.Cross(Vector3.up, direction).normalized;
        Gizmos.DrawLine(arrowEnd, arrowEnd - direction * 0.2f + right * 0.1f);
        Gizmos.DrawLine(arrowEnd, arrowEnd - direction * 0.2f - right * 0.1f);
    }
}
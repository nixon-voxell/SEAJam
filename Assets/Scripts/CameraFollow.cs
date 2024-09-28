using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private float m_Speed;
    [SerializeField] private Transform m_TargetTransform;
    public GameObject movementRange;
    private bool m_IsFollowing;
    
    
    private void Update()
    {
        m_IsFollowing = movementRange.GetComponent<TriggerCameraFollow>().c_EnableFollow;

        if (m_IsFollowing == true)
        {
            // Make sure factor is less than 1.0 to prevent overshoot.
            float factor = this.m_Speed * Time.deltaTime;
            factor = Mathf.Min(factor, 1.0f);

            Vector3 position = this.transform.position;

            // Move the camera.
            position = Vector2.Lerp(position, this.m_TargetTransform.position, factor);
            position.z = this.transform.position.z;
            this.transform.position = position;
        }
    }

}

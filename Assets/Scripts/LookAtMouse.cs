using UnityEngine;

public class LookAtMouse2D : MonoBehaviour
{
    [SerializeField] private Vector3 m_RotationOffset = Vector3.zero;
    private Camera m_Camera;

    


    private void Start()
    {
        PlayerMovement.OnOverridePlayerControl += TogglePlayerControl;


        this.m_Camera = Camera.main;
    }

    private void Update()
    {
        Vector3 mousePosition = this.m_Camera
            .ScreenToWorldPoint(Input.mousePosition);

        Vector2 direction = mousePosition - transform.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90.0f;

        transform.rotation = Quaternion.Euler(m_RotationOffset.x, m_RotationOffset.y, angle + m_RotationOffset.z);
    }

    void TogglePlayerControl(bool allowControl)
    {
        this.enabled = allowControl;
    }
}

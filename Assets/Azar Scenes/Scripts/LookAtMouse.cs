using UnityEngine;

public class LookAtMouse2D : MonoBehaviour
{
    [SerializeField] private Camera mainCamera;
    [SerializeField] private bool useMainCameraIfNull = true;
    [SerializeField] private bool invertRotation = false;
    [SerializeField] private Vector3 rotationOffset = Vector3.zero;

    private void Start()
    {
        if (mainCamera == null && useMainCameraIfNull)
        {
            mainCamera = Camera.main;
        }

        if (mainCamera == null)
        {
            enabled = false;
        }
    }

    private void Update()
    {
        Vector3 mousePosition = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        Vector2 direction = mousePosition - transform.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg + -90;
        
        if (invertRotation)
        {
            angle += 180f;
        }

        transform.rotation = Quaternion.Euler(rotationOffset.x, rotationOffset.y, angle + rotationOffset.z);
    }
}
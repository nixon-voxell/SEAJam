using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private void Start()
    {
    }

    private void Update()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        Vector2 movement = new Vector2(horizontal, vertical).normalized;

        Debug.Log($"{movement}");
    }
}

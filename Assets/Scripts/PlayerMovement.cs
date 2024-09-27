using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float m_Speed = 1.0f;

    private void Update()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");
        Vector3 movement = new Vector3(horizontal, vertical, 0.0f).normalized;

        // Move the player based on input.
        this.transform.position += this.m_Speed * movement * Time.deltaTime;
    }
}

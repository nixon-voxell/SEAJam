using UnityEngine;
using Voxell.Util;

public class PlayerMovement : MonoBehaviour
{
    [Tooltip("Base walking speed of the player.")]
    [SerializeField] private float m_Speed;
    [Tooltip("Amount of additional speed added to the base speed when stamina is being used.")]
    [SerializeField] private float m_BoostSpeed;
    [SerializeField] private PlayerStamina m_Stamina;

    private void Awake()
    {
        this.m_Stamina.Init();
    }

    private void Update()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");
        Vector3 movement = new Vector3(horizontal, vertical, 0.0f).normalized;

        bool useStamina = Input.GetButton("Jump");

        float speed = this.m_Speed;
        if (useStamina && this.m_Stamina.DepleteStamina())
        {
            speed += this.m_BoostSpeed;
        }

        // Move the player based on input.
        this.transform.position += movement * speed * Time.deltaTime;
    }
}

[System.Serializable]
public struct PlayerStamina
{
    [SerializeField] private float m_MaxStamina;

    [Tooltip("Amount of stamina to deplete per second.")]
    [SerializeField] private float m_DepletionFactor;

    [SerializeField, InspectOnly] private float m_CurrStamina;

    public void Init()
    {
        this.m_CurrStamina = this.m_MaxStamina;
    }

    public float GetMaxStamina()
    {
        return this.m_MaxStamina;
    }

    public float CurrStamina()
    {
        return this.m_CurrStamina;
    }

    /// <summary>
    /// Checks if the player still has stamina left.
    /// </summary>
    public bool HasStamina()
    {
        return this.CurrStamina() > 0.0f;
    }

    /// <summary>
    /// Depletes stamina and returns true if there is stamina left,
    /// and false if there isn't any left.
    /// </summary>
    public bool DepleteStamina()
    {
        if (this.HasStamina())
        {
            this.m_CurrStamina -= this.m_DepletionFactor * Time.deltaTime;
            // Clamp above 0.0f
            this.m_CurrStamina = Mathf.Max(this.m_CurrStamina, 0.0f);
            return true;
        }

        return false;
    }
}

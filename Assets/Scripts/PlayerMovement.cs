using UnityEngine;
using Voxell.Util;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMovement : MonoBehaviour
{
    [Tooltip("Base walking speed of the player.")]
    [SerializeField] private float m_Speed;
    [Tooltip("Amount of additional speed added to the base speed when stamina is being used.")]
    [SerializeField] private float m_BoostSpeed;
    [Tooltip("How fast the player can achieve the target speed.")]
    [SerializeField] private float m_SpeedLerpFactor;
    [SerializeField] private PlayerStamina m_Stamina;

    private float m_CurrSpeed;
    private Rigidbody2D m_Rigidbody;
    private Vector3 m_TargetPosition;

    public delegate void OverridePlayerControl(bool allowControl);
    public static OverridePlayerControl OnOverridePlayerControl;

    private void Awake()
    {
        this.m_CurrSpeed = 0.0f;
        this.m_Stamina.Init();
        this.m_Rigidbody = this.GetComponent<Rigidbody2D>();
        this.m_TargetPosition = this.transform.position;
    }

    private void Start()
    {
        OnOverridePlayerControl += TogglePlayerControl;
    }

    private void FixedUpdate()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");
        Vector3 movement = new Vector3(horizontal, vertical, 0.0f).normalized;

        bool useStamina = Input.GetButton("Jump");

        float targetSpeed = this.m_Speed;
        if (useStamina && this.m_Stamina.DepleteStamina())
        {
            targetSpeed += this.m_BoostSpeed;
        }

        this.m_CurrSpeed = Mathf.Lerp(
            this.m_CurrSpeed,
            targetSpeed,
            this.m_SpeedLerpFactor * Time.deltaTime
        );

        // Move the player based on input.
        this.m_TargetPosition = this.m_Rigidbody.position;
        this.m_TargetPosition += movement * this.m_CurrSpeed * Time.deltaTime;
        this.m_Rigidbody.MovePosition(this.m_TargetPosition);

        if (movement == Vector3.zero)
        {
            this.m_Stamina.RegenStamina();
        }
    }

    private void OnDestroy()
    {
        OnOverridePlayerControl = null;
    }
    void TogglePlayerControl(bool allowControl)
    {
        this.enabled = allowControl;
    }
}

[System.Serializable]
public struct PlayerStamina
{
    [SerializeField] private float m_MaxStamina;

    [Tooltip("Amount of stamina to deplete per second.")]
    [SerializeField] private float m_DepletionFactor;

    [Tooltip("Amount of stamina to regenerate per second.")]
    [SerializeField] private float m_RegenFactor;

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

    public void RegenStamina()
    {
        this.m_CurrStamina += this.m_RegenFactor * Time.deltaTime;
        this.m_CurrStamina = Mathf.Min(this.m_CurrStamina, this.m_MaxStamina);
    }
}

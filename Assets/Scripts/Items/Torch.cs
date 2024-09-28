using UnityEngine;
using UnityEngine.Rendering.Universal;
using Voxell.Util;

public class Torch : UsableItemBase
{
    [SerializeField] private Light2D m_Light;
    [SerializeField] private float m_MaxBatteryLevel;
    [Tooltip("Amount of battery level to deplete per second.")]
    [SerializeField] private float m_DepletionFactor;

    [SerializeField, InspectOnly] private bool m_FlashlightStatus;
    [SerializeField, InspectOnly] private float m_CurrBatteryLevel;

    public void RefillBattery()
    {
        this.m_DepletionFactor = this.m_MaxBatteryLevel;
    }

    public bool HasBattery()
    {
        return this.m_CurrBatteryLevel > 0.0f;
    }

    public override bool UseItem()
    {
        this.m_FlashlightStatus = !this.m_FlashlightStatus;
        return false;
    }

    private void Awake()
    {
        this.m_FlashlightStatus = false;
        this.RefillBattery();
    }

    private void Update()
    {
        // Slowly depletes the battery level of the flashlight.
        if (this.m_FlashlightStatus == true && this.HasBattery())
        {
            this.m_CurrBatteryLevel -= this.m_DepletionFactor * Time.deltaTime;
        }
    }
}

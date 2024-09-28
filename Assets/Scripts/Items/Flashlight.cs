using UnityEngine;
using UnityEngine.Rendering.Universal;
using Voxell.Util;

public class Flashlight : UsableItemBase
{
    [Header("Lighting")]
    [SerializeField] private float m_MaxIntensity = 2.0f;
    [SerializeField] private Light2D m_Light;
    [SerializeField] private float m_MaxBatteryLevel;
    [Tooltip("Amount of battery level to deplete per second.")]
    [SerializeField] private float m_DepletionFactor;

    [SerializeField, InspectOnly] private bool m_FlashlightStatus;
    [SerializeField, InspectOnly] private float m_CurrBatteryLevel;

    public void RefillBattery()
    {
        this.m_CurrBatteryLevel = this.m_MaxBatteryLevel;
    }

    public bool HasBattery()
    {
        return this.m_CurrBatteryLevel > 0.0f;
    }

    public override void UseItem()
    {
        this.m_FlashlightStatus = !this.m_FlashlightStatus;
    }

    public override void SetHideItem(bool hide)
    {
        base.SetHideItem(hide);
        if (hide)
        {
            this.m_FlashlightStatus = false;
            this.m_Light.intensity = 0.0f;
        }
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


        if (this.m_FlashlightStatus)
        {
            this.m_Light.intensity = Mathf.Lerp(
                0.0f,
                this.m_MaxIntensity,
                this.m_CurrBatteryLevel / this.m_MaxBatteryLevel
            );
        }
        else
        {
            this.m_Light.intensity = 0.0f;
        }
    }
}

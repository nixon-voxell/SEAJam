using UnityEngine;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
    public static GameManager Singleton;

    [Header("UI References")]
    public UI_GeigerCounter_Behavior geigerUI;
    public BatteryOverlay batteryOverlay;

    [Header("Player Equipment")]
    public Flashlight playerFlashlight;
    public GeigerCounterBatteryManager geigerCounterBattery;

    [Header("Radiation Management")]
    private List<RadiationRadius> radiationSources = new List<RadiationRadius>();

    void Awake()
    {
        if (Singleton == null)
        {
            Singleton = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        if (geigerUI == null)
        {
            Debug.LogError("UI_GeigerCounter_Behavior not assigned in GameManager!");
        }
        if (batteryOverlay == null)
        {
            Debug.LogError("BatteryOverlay not assigned in GameManager!");
        }
        if (playerFlashlight == null)
        {
            Debug.LogError("Flashlight not assigned in GameManager!");
        }
        if (geigerCounterBattery == null)
        {
            Debug.LogError("GeigerCounterBatteryManager not assigned in GameManager!");
        }
        FindRadiationSources();
    }

    void Update()
    {
        if (geigerUI != null)
        {
            float highestRadiationLevel = GetHighestRadiationLevel();
            geigerUI.UpdateUI(highestRadiationLevel);
        }
    }

    // Battery Overlay Management
    public void ShowBatteryOverlay()
    {
        Debug.Log("ShowBatteryOverlay called");
        if (batteryOverlay != null)
        {
            Debug.Log("batteryOverlay is not null, calling Show()");
            batteryOverlay.Show();
        }
        else
        {
            Debug.LogError("BatteryOverlay is not assigned in GameManager!");
        }
    }

    public void HideBatteryOverlay()
    {
        if (batteryOverlay != null)
        {
            batteryOverlay.Hide();
        }
    }

    // Battery Usage Functions
    public void RefillFlashlightBattery()
    {
        if (playerFlashlight != null)
        {
            playerFlashlight.RefillBattery();
            Debug.Log("Flashlight battery refilled.");
        }
        else
        {
            Debug.LogWarning("Flashlight not found!");
        }
    }

    public void RefillGeigerCounterBattery()
    {
        if (geigerCounterBattery != null)
        {
            geigerCounterBattery.ReplaceBattery();
            Debug.Log("Geiger counter battery replaced.");
        }
        else
        {
            Debug.LogWarning("GeigerCounterBatteryManager not found!");
        }
    }

    // Radiation Management
    void FindRadiationSources()
    {
        radiationSources.Clear();
        RadiationRadius[] sources = FindObjectsOfType<RadiationRadius>();
        radiationSources.AddRange(sources);
    }

    float GetHighestRadiationLevel()
    {
        float highestLevel = 0f;
        foreach (RadiationRadius source in radiationSources)
        {
            float level = source.GetCurrentRadiationLevel();
            if (level > highestLevel)
            {
                highestLevel = level;
            }
        }
        return highestLevel;
    }

    public void RegisterRadiationSource(RadiationRadius source)
    {
        if (!radiationSources.Contains(source))
        {
            radiationSources.Add(source);
        }
    }

    public void UnregisterRadiationSource(RadiationRadius source)
    {
        radiationSources.Remove(source);
    }
}
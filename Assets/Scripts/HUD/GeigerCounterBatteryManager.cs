using UnityEngine;
using UnityEngine.UI;  // Required for working with UI Image
using DG.Tweening;

public class GeigerCounterBatteryManager : MonoBehaviour
{
    public static GeigerCounterBatteryManager Instance;

    public Sprite geigerActiveSprite;     // Sprite when the geiger counter is active
    public Sprite geigerDeadSprite;       // Sprite when the geiger counter is dead
    public Image currentImage;            // The UI Image component to display sprites
    public float flickerDuration = 0.2f;  // Duration of flicker when draining
    public float flickerIntensity = 0.5f; // Flicker intensity when draining

    public float currentRadiation = 0f;
    public float maxRadiation = 100f;
    public float minRadiationRate = 5f;   // Minimum radiation rate when far from a source
    public float maxRadiationRate = 50f;  // Maximum radiation rate when close to a source
    public float radiationDecreaseRate = 5f;
    public float batteryLife = 100f;      // Battery life percentage (0-100)
    public float batteryDrainRate = 1f;   // Battery drain rate per second
    public float batteryLowThreshold = 20f; // When battery is considered low

    public bool isBatteryDead = false;

    private void Awake()
    {
        // Singleton pattern
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        // Ensure the starting sprite is the active one
        if (currentImage != null && geigerActiveSprite != null)
        {
            currentImage.sprite = geigerActiveSprite;  // Set initial sprite to active state
        }
    }

    private void Update()
    {
        if (!isBatteryDead)
        {
            DrainBatteryOverTime(); // Continuously drain battery over time
            CheckBatteryStatus();   // Check if the battery should be considered dead

            if (batteryLife <= batteryLowThreshold)
            {
                // Flicker effect as battery drains
                FlickerGeiger();
            }
        }
        else
        {
            currentImage.sprite = geigerDeadSprite;  // Show dead sprite when battery is dead
        }

        this.DecreaseRadiation();
    }

    public void IncreaseRadiation(float rate)
    {
        currentRadiation += rate * Time.deltaTime;
        currentRadiation = Mathf.Min(currentRadiation, maxRadiation);
        Debug.Log($"Radiation increased to {currentRadiation}");
    }

    private void DecreaseRadiation()
    {
        currentRadiation -= radiationDecreaseRate * Time.deltaTime;
        currentRadiation = Mathf.Max(currentRadiation, 0f);
        Debug.Log($"Radiation decreased to {currentRadiation}");
    }

    // Drain battery over time
    private void DrainBatteryOverTime()
    {
        batteryLife -= batteryDrainRate * Time.deltaTime;
        batteryLife = Mathf.Clamp(batteryLife, 0f, 100f);

        if (batteryLife <= 0f)
        {
            DrainBattery();
        }
    }

    // Check the battery status and log warnings if it's low
    private void CheckBatteryStatus()
    {
        if (batteryLife <= batteryLowThreshold && batteryLife > 0f)
        {
            Debug.LogWarning("Geiger counter battery is low!");
        }
    }

    public bool IsBatteryDead()
    {
        return isBatteryDead;
    }

    public void ReplaceBattery()
    {
        isBatteryDead = false;
        batteryLife = 100f;
        currentImage.sprite = geigerActiveSprite; // Switch back to active sprite
        Debug.Log("Geiger counter battery replaced.");
    }

    public void DrainBattery()
    {
        isBatteryDead = true;
        currentImage.sprite = geigerDeadSprite;  // Switch to dead sprite
        Debug.Log("Geiger counter battery is now dead.");
    }

    // Get the current radiation level as a percentage
    public float GetRadiationLevel()
    {
        return currentRadiation / maxRadiation;
    }

    // Flicker effect when battery is low (UI Image flickering)
    private void FlickerGeiger()
    {
        if (currentImage != null)
        {
            currentImage.DOKill(); // Kill any existing flicker animations
            currentImage.DOFade(flickerIntensity, flickerDuration).SetLoops(2, LoopType.Yoyo).OnComplete(() =>
            {
                // Reset back to full alpha after flicker
                currentImage.DOFade(1f, flickerDuration);
            });
        }
    }
}

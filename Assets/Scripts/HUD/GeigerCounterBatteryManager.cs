using UnityEngine;

public class GeigerCounterBatteryManager : MonoBehaviour
{
    public static GeigerCounterBatteryManager Instance;

    public float currentRadiation = 0f;
    public float maxRadiation = 100f;
    public float minRadiationRate = 5f; // Minimum radiation rate when far from a source
    public float maxRadiationRate = 50f; // Maximum radiation rate when close to a source
    public float radiationDecreaseRate = 5f;
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

    public void IncreaseRadiation(float rate)
    {
        if (!isBatteryDead)
        {
            currentRadiation += rate * Time.deltaTime;
            currentRadiation = Mathf.Min(currentRadiation, maxRadiation);
            Debug.Log($"Radiation increased to {currentRadiation}");
        }
        else
        {
            Debug.LogWarning("Cannot increase radiation, Geiger counter battery is dead.");
        }
    }

    public void DecreaseRadiation()
    {
        if (!isBatteryDead)
        {
            currentRadiation -= radiationDecreaseRate * Time.deltaTime;
            currentRadiation = Mathf.Max(currentRadiation, 0f);
            Debug.Log($"Radiation decreased to {currentRadiation}");
        }
        else
        {
            Debug.LogWarning("Cannot decrease radiation, Geiger counter battery is dead.");
        }
    }

    public bool IsBatteryDead()
    {
        return isBatteryDead;
    }

    public void ReplaceBattery()
    {
        isBatteryDead = false;
        Debug.Log("Geiger counter battery replaced.");
    }

    public void DrainBattery()
    {
        isBatteryDead = true;
        Debug.Log("Geiger counter battery is now dead.");
    }

    public float GetRadiationLevel()
    {
        return currentRadiation / maxRadiation;
    }
}

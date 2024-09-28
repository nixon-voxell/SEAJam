using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GeigerCounterBatteryManager : MonoBehaviour
{
    public Image geigerCounterImage;
    public Sprite onSprite;
    public Sprite offSprite;
    public float batteryLife = 100f;
    public float batteryDrainRate = 1f;
    public float lowBatteryThreshold = 20f;
    public float flickerInterval = 0.5f;

    private bool isBatteryDead = false;
    private UI_GeigerCounter_Behavior uiGeigerCounter;

    private void Start()
    {
        uiGeigerCounter = GetComponent<UI_GeigerCounter_Behavior>();
        if (uiGeigerCounter == null)
        {
            Debug.LogError("UI_GeigerCounter_Behavior not found on the same GameObject!");
        }

        if (geigerCounterImage == null)
        {
            Debug.LogError("Geiger Counter Image not assigned in GeigerCounterBatteryManager!");
        }

        StartCoroutine(DrainBattery());
    }

    private IEnumerator DrainBattery()
    {
        while (batteryLife > 0)
        {
            yield return new WaitForSeconds(1f);
            batteryLife -= batteryDrainRate;
            batteryLife = Mathf.Max(batteryLife, 0f);

            if (batteryLife <= lowBatteryThreshold && !isBatteryDead)
            {
                StartCoroutine(FlickerEffect());
            }

            if (batteryLife <= 0)
            {
                BatteryDead();
            }
        }
    }

    private IEnumerator FlickerEffect()
    {
        while (batteryLife > 0 && batteryLife <= lowBatteryThreshold)
        {
            geigerCounterImage.color = new Color(1f, 1f, 1f, 0.5f);
            yield return new WaitForSeconds(flickerInterval);
            geigerCounterImage.color = Color.white;
            yield return new WaitForSeconds(flickerInterval);
        }
    }

    private void BatteryDead()
    {
        isBatteryDead = true;
        geigerCounterImage.sprite = offSprite;
        geigerCounterImage.color = Color.white;
        StopAllCoroutines();
    }

    public void ReplaceBattery()
    {
        batteryLife = 100f;
        isBatteryDead = false;
        geigerCounterImage.sprite = onSprite;
        geigerCounterImage.color = Color.white;
        StartCoroutine(DrainBattery());
    }

    public bool IsBatteryDead()
    {
        return isBatteryDead;
    }
}
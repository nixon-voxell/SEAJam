using UnityEngine;
using TMPro;
using DG.Tweening;
using UnityEngine.SceneManagement; // For restarting the scene

public class UI_GeigerCounter_Behavior : MonoBehaviour
{
    public TimerManagerUI timerManagerUI;
    public TextMeshProUGUI radiationText;
    public float maxShakeIntensity = 0.5f;
    public float minShakeIntensity = 0.1f;
    public float shakeDuration = 0.1f;
    public float textFadeOutThreshold = 0.7f;
    public float maxShakeInterval = 1f;
    public float minShakeInterval = 0.1f;

    private Vector3 originalPosition;
    private float nextShakeTime = 0f;
    private GeigerCounterBatteryManager batteryManager;

    void Start()
    {
        if (radiationText == null)
        {
            Debug.LogError("TextMeshProUGUI not assigned in UI_GeigerCounter_Behavior!");
        }
        originalPosition = transform.localPosition;

        batteryManager = GeigerCounterBatteryManager.Instance;
        if (batteryManager == null)
        {
            Debug.LogError("GeigerCounterBatteryManager instance not found!");
        }
    }

    void LateUpdate()
    {
        // Get the current radiation level from the singleton
        float radiationLevel = batteryManager.GetRadiationLevel();

        // Check if radiation level reached the maximum threshold (1.0)
        if (radiationLevel >= 1.0f)
        {
            // RestartScene(); // Restart the game or scene
            timerManagerUI.LoseGameSequence();
        }
        else
        {
            UpdateUI(radiationLevel);
        }
    }

    public void UpdateUI(float radiationLevel)
    {
        if (batteryManager.IsBatteryDead())
        {
            StopShaking();
            radiationText.text = "---";
            return;
        }

        UpdateText(radiationLevel);

        if (radiationLevel > 0)
        {
            if (Time.time >= nextShakeTime)
            {
                ShakeUI(radiationLevel);
                CalculateNextShakeTime(radiationLevel);
            }
        }
        else
        {
            StopShaking();
        }
    }

    private void UpdateText(float radiationLevel)
    {
        if (radiationText != null)
        {
            // Update the radiation level text based on the current value (keep between 0 and 1)
            radiationText.text = $"{radiationLevel:F2}"; // Show decimal numbers like 0.32, 0.45, etc.

            // Fade out the text if the radiation level exceeds the threshold
            if (radiationLevel > textFadeOutThreshold)
            {
                float alpha = 1 - ((radiationLevel - textFadeOutThreshold) / (1 - textFadeOutThreshold));
                radiationText.alpha = Mathf.Clamp01(alpha);
            }
            else
            {
                radiationText.alpha = 1;
            }
        }
    }

    private void ShakeUI(float radiationLevel)
    {
        float shakeIntensity = Mathf.Lerp(minShakeIntensity, maxShakeIntensity, radiationLevel);

        transform.DOKill();
        transform.localPosition = originalPosition;

        Vector2 randomDirection = Random.insideUnitCircle.normalized;
        Vector3 shakeOffset = new Vector3(randomDirection.x, randomDirection.y, 0) * shakeIntensity;

        transform.DOLocalMove(originalPosition + shakeOffset, shakeDuration)
            .SetEase(Ease.OutElastic)
            .OnComplete(() => transform.localPosition = originalPosition);
    }

    private void CalculateNextShakeTime(float radiationLevel)
    {
        float interval = Mathf.Lerp(maxShakeInterval, minShakeInterval, radiationLevel);
        nextShakeTime = Time.time + interval;
    }

    private void StopShaking()
    {
        transform.DOKill();
        transform.localPosition = originalPosition;
    }

    // private void RestartScene()
    // {
    //     Debug.Log("Radiation reached maximum! Restarting scene...");
    //     SceneManager.LoadScene(SceneManager.GetActiveScene().name); // Restart the current scene
    // }
}

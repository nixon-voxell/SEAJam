using UnityEngine;
using TMPro;
using DG.Tweening;

public class UI_GeigerCounter_Behavior : MonoBehaviour
{
    public TextMeshProUGUI radiationText;
    public float maxShakeIntensity = 0.5f;
    public float minShakeIntensity = 0.1f;
    public float shakeDuration = 0.1f;
    public float textFadeOutThreshold = 0.7f;
    public float maxShakeInterval = 1f;
    public float minShakeInterval = 0.1f;

    private Vector3 originalPosition;
    private bool isShaking = false;
    private float nextShakeTime = 0f;
    private GeigerCounterBatteryManager batteryManager;

    void Start()
    {
        if (radiationText == null)
        {
            Debug.LogError("TextMeshProUGUI not assigned in UI_GeigerCounter_Behavior!");
        }
        originalPosition = transform.localPosition;

        batteryManager = GetComponent<GeigerCounterBatteryManager>();
        if (batteryManager == null)
        {
            Debug.LogError("GeigerCounterBatteryManager not found on the same GameObject!");
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
            radiationText.text = $"{radiationLevel:F2}";
            
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
}
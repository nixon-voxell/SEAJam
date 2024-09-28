using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

public class BatteryOverlay : MonoBehaviour 
{
    public Button flashlightButton;
    public Button geigerButton;
    public TextMeshProUGUI useLabel;

    [Header("Animation Settings")]
    [SerializeField] private float popInDuration = 0.3f;
    [SerializeField] private float popOutDuration = 0.2f;
    [SerializeField] private float startScale = 0.8f;
    [SerializeField] private Ease popInEase = Ease.OutBack;
    [SerializeField] private Ease popOutEase = Ease.InBack;

    private RectTransform overlayRect;

    private void Awake()
    {
        overlayRect = GetComponent<RectTransform>();

        if (flashlightButton == null || geigerButton == null)
        {
            Debug.LogError("Buttons are not assigned in the inspector!");
            return;
        }

        if (flashlightButton.GetComponent<Button>() == null || geigerButton.GetComponent<Button>() == null)
        {
            Debug.LogError("Button components are missing!");
            return;
        }

        Canvas canvas = GetComponentInParent<Canvas>();
        if (canvas != null && canvas.GetComponent<GraphicRaycaster>() == null)
        {
            Debug.LogWarning("Canvas is missing a GraphicRaycaster component!");
        }
    }

    private void Start()
    {
        HideInstantly();
        
        flashlightButton.onClick.AddListener(OnFlashlightButtonClick);
        geigerButton.onClick.AddListener(OnGeigerButtonClick);

        Debug.Log("Button listeners added.");
    }

    public void Show()
    {
        gameObject.SetActive(true);
        AnimatePopIn();
        Debug.Log("BatteryOverlay shown.");
    }

    private void AnimatePopIn()
    {
        overlayRect.localScale = Vector3.one * startScale;
        overlayRect.DOScale(Vector3.one, popInDuration).SetEase(popInEase);
    }

    public void Hide()
    {
        AnimatePopOut();
    }

    private void AnimatePopOut()
    {
        DOTween.Kill(overlayRect);
        overlayRect.DOScale(Vector3.one * startScale, popOutDuration)
            .SetEase(popOutEase)
            .OnComplete(() => {
                gameObject.SetActive(false);
                Debug.Log("BatteryOverlay hidden.");
            });
    }

    private void HideInstantly()
    {
        overlayRect.localScale = Vector3.one * startScale;
        gameObject.SetActive(false);
    }

    private void OnFlashlightButtonClick()
    {
        Debug.Log("Flashlight button clicked.");
        UseBatteryOnFlashlight();
        Hide();
    }

    private void OnGeigerButtonClick()
    {
        Debug.Log("Geiger button clicked.");
        UseBatteryOnGeiger();
        Hide();
    }

    private void UseBatteryOnFlashlight()
    {
        Flashlight flashlight = FindObjectOfType<Flashlight>();
        if (flashlight != null)
        {
            flashlight.RefillBattery();
            Debug.Log("Flashlight battery refilled.");
        }
        else
        {
            Debug.LogWarning("No Flashlight found in the scene.");
        }
    }

    private void UseBatteryOnGeiger()
    {
        Debug.Log("Recharged the Geiger counter");
    }

    private void OnDisable()
    {
        DOTween.Kill(overlayRect);
    }
}
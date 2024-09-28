using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;
using System;

public class BatteryOverlay : MonoBehaviour
{
    public Button flashlightButton;
    public Button geigerButton;
    public TextMeshProUGUI useLabel;

    [SerializeField] private float animationDuration = 0.5f;
    [SerializeField] private Vector3 hiddenPosition = new Vector3(0, -100, 0);
    [SerializeField] private Vector3 visiblePosition = Vector3.zero;

    private void Start()
    {
        HideInstantly();
        
        flashlightButton.onClick.AddListener(OnFlashlightButtonClick);
        geigerButton.onClick.AddListener(OnGeigerButtonClick);
    }

    public void Show()
    {
        gameObject.SetActive(true);

        flashlightButton.transform.DOLocalMove(visiblePosition, animationDuration).SetEase(Ease.OutQuad);
        geigerButton.transform.DOLocalMove(visiblePosition, animationDuration).SetEase(Ease.OutQuad);
        useLabel.transform.DOLocalMove(visiblePosition, animationDuration).SetEase(Ease.OutQuad);
        useLabel.DOFade(1f, animationDuration);
    }

    public void Hide()
    {
        Sequence hideSequence = DOTween.Sequence();

        hideSequence.Join(flashlightButton.transform.DOLocalMove(hiddenPosition, animationDuration).SetEase(Ease.InQuad));
        hideSequence.Join(geigerButton.transform.DOLocalMove(hiddenPosition, animationDuration).SetEase(Ease.InQuad));
        hideSequence.Join(useLabel.transform.DOLocalMove(hiddenPosition, animationDuration).SetEase(Ease.InQuad));
        hideSequence.Join(useLabel.DOFade(0f, animationDuration));

        hideSequence.OnComplete(() => gameObject.SetActive(false));
    }

    private void HideInstantly()
    {
        flashlightButton.transform.localPosition = hiddenPosition;
        geigerButton.transform.localPosition = hiddenPosition;
        useLabel.transform.localPosition = hiddenPosition;
        useLabel.color = new Color(useLabel.color.r, useLabel.color.g, useLabel.color.b, 0f);
        gameObject.SetActive(false);
    }

    private void OnFlashlightButtonClick()
    {
        Debug.Log("Battery used on Flashlight");
        UseBatteryOnFlashlight();
        Hide();
    }

    private void OnGeigerButtonClick()
    {
        Debug.Log("Battery used on Geiger counter");
        UseBatteryOnGeiger();
        Hide();
    }

    private void UseBatteryOnFlashlight()
    {
        Flashlight flashlight = FindObjectOfType<Flashlight>();
        if (flashlight != null)
        {
            flashlight.RefillBattery();
        }
        else
        {
            Debug.LogWarning("No Flashlight found in the scene.");
        }
    }

    private void UseBatteryOnGeiger()
    {
        Debug.Log("rechatged the geiger");
    }
}
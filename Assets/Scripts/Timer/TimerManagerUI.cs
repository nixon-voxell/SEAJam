using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System;

public class TimerManagerUI : MonoBehaviour
{
    [Header("References")]
    [SerializeField] TextMeshProUGUI _TimerText;
    [SerializeField] Image _ExplosionFlash;
    [SerializeField] Button _ReloadGameButton;
    [SerializeField] TextMeshProUGUI _ReloadLabel;
    [SerializeField] Animator _TimerAnimator;
    [SerializeField] Animator _FlashAnimator;

    #region Properties

    const string _TimerFormat = "{0}:{1}";

    #endregion

    public void OnTimerChange(float timer)
    {
        UpdateTimerText(timer);

        if (timer > 0) return;

        LoseGameSequence();
    }

    void UpdateTimerText(float timer)
    {
        int _minutes = Math.Max(Mathf.FloorToInt(timer / 60f), 0);
        int _seconds = Math.Max(Mathf.FloorToInt(timer - _minutes * 60), 0);

        _TimerText.text = string.Format(_TimerFormat, _minutes.ToString("00"), _seconds.ToString("00"));
    }

    void LoseGameSequence()
    {
        ShakeCamera();
        _TimerAnimator.enabled = true;
        _FlashAnimator.gameObject.SetActive(true);
    }

    void ShakeCamera()
    {
        Camera _mainCamera = Camera.main;

        if (_mainCamera == null) return;

        if (!_mainCamera.TryGetComponent(out CameraShake _shaker)) return;

        _shaker.StartShake();
    }

    public void ResetUI()
    {
        _TimerAnimator.enabled = false;
        _FlashAnimator.gameObject.SetActive(false);
    }

    //IEnumerator FlashTimer()
    //{
    //    _TimerText.text = "00:00";
    //    WaitForSeconds _delayBetweenFlash = new WaitForSeconds(.2f);

    //    for (int i = 0; i < 3; i++)
    //    {
    //        _TimerText.enabled = false;
    //        yield return _delayBetweenFlash;
    //        _TimerText.enabled = true;
    //        yield return _delayBetweenFlash;
    //    }
    //}

    //IEnumerator ExplosionFlash()
    //{
    //    _ExplosionFlash.enabled = true;
    //    float _flashDuration = 3;
    //    float _flashTimer = 0;
    //    Color _flashColor = Color.white;
    //    _flashColor.a = 0;

    //    while (_flashTimer < _flashDuration)
    //    {
    //        _flashColor.a = Mathf.Lerp(0, 1, _flashTimer / _flashDuration);
    //        _ExplosionFlash.color = _flashColor;
    //        _flashTimer += Time.deltaTime;
    //        yield return null;
    //    }

    //    _ReloadLabel.gameObject.SetActive(true);
    //    _ReloadGameButton.interactable = true;
    //}

    public void OnClickReloadGame()
    {
        SceneManager.LoadScene(1);
        Time.timeScale = 1;
    }
}

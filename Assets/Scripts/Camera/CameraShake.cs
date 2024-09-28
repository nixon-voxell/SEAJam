using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    [Header("Developer")]
    [SerializeField] float _Duration;
    [SerializeField] AnimationCurve _ShakeCurve;

    #region Properties

    Coroutine _ShakeRoutine;

    #endregion

    public void StartShake()
    {
        if (_ShakeRoutine != null) StopCoroutine(_ShakeRoutine);
        _ShakeRoutine = StartCoroutine(ShakeCamera());
    }

    IEnumerator ShakeCamera()
    {
        Vector3 _initialPosition = transform.position;
        Vector3 _shakeOffset = Vector3.zero;
        float _timer = 0;
        float _shake;

        while(_timer < _Duration)
        {
            _shake = _ShakeCurve.Evaluate(_timer / _Duration);
            _shakeOffset.x = UnityEngine.Random.Range(-_shake, _shake);
            _shakeOffset.y = UnityEngine.Random.Range(-_shake, _shake);
            transform.position = _initialPosition + _shakeOffset;
            _timer += Time.deltaTime;
            yield return null;
        }
    }
}

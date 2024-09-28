using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class LightFlicker2D : MonoBehaviour
{
    [Header("Developer")]
    [SerializeField] Vector2 _MinMaxLightIntensity;
    [SerializeField] Vector2 _MinMaxIntensityChangeDelay;

    #region Properties

    Coroutine FlickerRoutine;
    Light2D m_Light;

    #endregion

    private void Awake()
    {
        m_Light = GetComponent<Light2D>();
    }

    private void Start()
    {
        if (FlickerRoutine != null) StopCoroutine(FlickerRoutine);
        FlickerRoutine = StartCoroutine(LightFlicker());
    }

    IEnumerator LightFlicker()
    {
        float _lightIntensity = 0;
        float _delay = 0;
        WaitForEndOfFrame _updateDelay = new WaitForEndOfFrame();

        while(true)
        {
            _lightIntensity = UnityEngine.Random.Range(_MinMaxLightIntensity.x, _MinMaxLightIntensity.y);
            _delay = UnityEngine.Random.Range(_MinMaxIntensityChangeDelay.x, _MinMaxIntensityChangeDelay.y);
            m_Light.intensity = _lightIntensity;
            yield return new WaitForSeconds(_delay);
        }
    }

}

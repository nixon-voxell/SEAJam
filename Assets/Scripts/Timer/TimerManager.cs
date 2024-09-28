using System.Collections;
using UnityEngine;

public class TimerMananger : MonoBehaviour
{
    [Header("Developer")]
    [Tooltip("Timer in seconds")] [SerializeField] int _TotalTimer;

    #region Properties

    float _CurrentTimer;

    #endregion

    #region Delegates

    public delegate void FloatValueChangeDelegate(float floatValue);
    public static FloatValueChangeDelegate OnTimerChange;

    #endregion

    private void Start()
    {
        StartCoroutine(StartTimer());
    }

    private void OnDestroy()
    {
        OnTimerChange = null;
    }

    IEnumerator StartTimer()
    {
        _CurrentTimer = _TotalTimer;

        while(_CurrentTimer >= 0)
        {
            yield return null;
            _CurrentTimer -= Time.deltaTime;
            OnTimerChange(_CurrentTimer);
        }
    }

    /// <summary>
    /// Psuedo stop game
    /// </summary>
    public void LoseGame()
    {

#if DEVELOPMENT_BUILD || UNITY_EDITOR
        Debug.Log("Stop");
#endif


        Time.timeScale = 0;
    }
}

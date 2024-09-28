using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TimerManager : MonoBehaviour
{
    public static TimerManager Instance;

    [Header("Developer")]
    [Tooltip("Timer in seconds")] [SerializeField] int _TotalTimer;

    [Header("References")]
    [SerializeField] TimerManagerUI _TimerUI;

    #region Properties

    public float CurrentTimer { get; private set; }
    Coroutine _CountdownRoutine;

    #endregion

    private void Awake()
    {
        if(Instance != null)
        {
            Destroy(gameObject);
            this.enabled = false;
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        _CountdownRoutine = StartCoroutine(StartTimer());
        SceneManager.sceneLoaded += ResetTimer;
    }

    private void OnDestroy()
    {
        if(Instance == this)
        {
            Instance = null;
            SceneManager.sceneLoaded -= ResetTimer;
        }
    }

    IEnumerator StartTimer()
    {
        CurrentTimer = _TotalTimer;

        while(CurrentTimer >= 0)
        {
            yield return null;
            CurrentTimer -= Time.deltaTime;
            _TimerUI.OnTimerChange(CurrentTimer);
        }
    }

    void ResetTimer(Scene loadedScene, LoadSceneMode loadMode)
    {
        if (_CountdownRoutine != null)
        {
            StopCoroutine(_CountdownRoutine);
            _CountdownRoutine = null;
        }

        if (_CountdownRoutine != null) StopCoroutine(_CountdownRoutine);
        _CountdownRoutine = StartCoroutine(StartTimer());

        _TimerUI.ResetUI();
    }

    public void StopTimer()
    {
        if (_CountdownRoutine != null)
        {
            StopCoroutine(_CountdownRoutine);
            _CountdownRoutine = null;
        }
    }
}

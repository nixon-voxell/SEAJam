using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PuzzleManager : MonoBehaviour
{
    public static PuzzleManager Instance;

    [Header("References")]
    [SerializeField] CanvasGroup _WinGroup;
    [SerializeField] TextMeshProUGUI _WinText;
    [SerializeField] Button _ReloadButton;
    [SerializeField] Image _DipToWhite;
    [SerializeField] TextMeshProUGUI _RetryText;

    #region Properties

    List<Puzzle> _Puzzles = new List<Puzzle>();

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
        SceneManager.sceneLoaded += ResetSelf;
    }

    private void Start()
    {
        BeginListeningForPuzzleSolved();
    }

    private void OnDestroy()
    {
        if(Instance == this)
        {
            Instance = null;
            SceneManager.sceneLoaded -= ResetSelf;
        }
    }

    void BeginListeningForPuzzleSolved()
    {
        foreach(Puzzle _puzzle in _Puzzles)
        {
            _puzzle.OnPuzzleSolved += CheckAllPuzzlesSolved;
        }
    }

    public void AttachNewPuzzle(Puzzle newPuzzle)
    {
        _Puzzles.Add(newPuzzle);
        newPuzzle.OnPuzzleSolved += CheckAllPuzzlesSolved;
    }

    void CheckAllPuzzlesSolved(Puzzle solvedPuzzle)
    {
        foreach(Puzzle _puzzle in _Puzzles)
        {
            ///Some puzzles not solved yet
            if (!_puzzle.Solved) return;
        }

        TimerManager.Instance.StopTimer();
        _WinGroup.VisibleAndBlocks(true);
        Time.timeScale = 0;
        _WinText.text = string.Empty;
        TimeSpan _timeLeft = new TimeSpan(0,0, Mathf.FloorToInt(TimerManager.Instance.CurrentTimer));

        Sequence _winSequence = DOTween.Sequence();
        _winSequence.Append(_DipToWhite.DOFade(1,1));
        _winSequence.Append(_WinText.DOText($"CONGRATULATIONS! You have saved the day!\nYou had {_timeLeft.ToString("mm")} minutes and {_timeLeft.ToString("ss")} seconds to spare", 4));
        _winSequence.AppendInterval(1);
        _winSequence.AppendCallback(() =>
        {
            _ReloadButton.interactable = true;
            _RetryText.gameObject.SetActive(true);
        });
        _winSequence.SetUpdate(true);
    }

    public void OnClickReloadGame()
    {
        SceneManager.LoadScene(0);
        Time.timeScale = 1;
    }

    void ResetSelf(Scene loadedScene, LoadSceneMode loadMode)
    {
        foreach(Puzzle _puzzle in _Puzzles)
        {
            _puzzle.OnPuzzleSolved -= CheckAllPuzzlesSolved;
        }

        ResetUI();
        _Puzzles.Clear();
    }

    void ResetUI()
    {
        _WinGroup.VisibleAndBlocks(false);
        _DipToWhite.color = new Color(1,1,1,0);
        _WinText.text = string.Empty;
        _ReloadButton.interactable = false;
        _RetryText.gameObject.SetActive(false);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Puzzle : MonoBehaviour
{
    [Header("Puzzle References")]
    [SerializeField] protected ProximityPromptSystem _Interactor;

    #region Properties

    public bool Solved { get; private set; } = false;

    public delegate void PuzzleSolved(Puzzle solvedPuzzle);
    public PuzzleSolved OnPuzzleSolved;

    #endregion

    protected void SolvedPuzzle()
    {
        Solved = true;
        _Interactor.gameObject.SetActive(false);
        OnPuzzleSolved?.Invoke(this);
    }

    private void OnDestroy()
    {
        OnPuzzleSolved = null;
    }

    protected abstract void CheckPuzzleCompleted();
}

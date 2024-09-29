using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public abstract class Puzzle : MonoBehaviour, IInteractable
{
    [Header("Puzzle References")]
    [SerializeField] protected ProximityPromptSystem _Interactor;
    [SerializeField] protected SpriteRenderer _PuzzleMapRenderer;

    #region Properties

    public bool Solved { get; private set; } = false;

    public delegate void PuzzleSolved(Puzzle solvedPuzzle);
    public PuzzleSolved OnPuzzleSolved;

    #endregion

    private void Start()
    {
        _PuzzleMapRenderer.color = Color.red;

        PuzzleManager.Instance.AttachNewPuzzle(this);
    }

    protected void SolvedPuzzle()
    {
        Solved = true;
        _Interactor.gameObject.SetActive(false);
        _PuzzleMapRenderer.color = Color.green;
        OnPuzzleSolved?.Invoke(this);
    }

    private void OnDestroy()
    {
        OnPuzzleSolved = null;
    }

    protected abstract void CheckPuzzleCompleted();

    public void Interact()
    {
        CheckPuzzleCompleted();
    }
}

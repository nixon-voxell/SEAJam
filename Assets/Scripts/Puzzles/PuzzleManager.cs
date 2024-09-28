using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] Puzzle[] _Puzzles;


    private void Start()
    {
        BeginListeningForPuzzleSolved();

    }

    void BeginListeningForPuzzleSolved()
    {
        foreach(Puzzle _puzzle in _Puzzles)
        {
            _puzzle.OnPuzzleSolved += CheckAllPuzzlesSolved;
        }
    }

    void CheckAllPuzzlesSolved(Puzzle solvedPuzzle)
    {
        foreach(Puzzle _puzzle in _Puzzles)
        {
            if (!_puzzle.Solved) return;
        }

        Debug.Log("Game WIN");
    }
}

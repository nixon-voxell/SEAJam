using System.Collections.Generic;
using UnityEngine;

public class WireGameManager : Puzzle
{
    [Header("References")]
    [SerializeField] WireDragger[] _ConnectorWires;
    [SerializeField] WireReceiver[] _ReceiverWires;
    [SerializeField] CanvasGroup _GameGroup;

    private void Awake()
    {
        BeginListeningForConnection();
    }

    void BeginListeningForConnection()
    {
        foreach(WireDragger _dragger in _ConnectorWires)
        {
            _dragger.OnConnectionSuccessful += CheckPuzzleCompleted;
        }
    }

    public void StartGame()
    {
        RandomizeWirePositions();
        _GameGroup.VisibleAndBlocks(true);
        PlayerMovement.OnOverridePlayerControl?.Invoke(false);
    }

    void RandomizeWirePositions()
    {
        float _segment = 1 / (float)(_ConnectorWires.Length + 1);
        List<int> _leftRandomPositions = new List<int>();
        List<int> _rightRandomPositions = new List<int>();

        for (int i = 1; i < _ConnectorWires.Length + 1; i++)
        {
            _leftRandomPositions.Add(i);
            _rightRandomPositions.Add(i);
        }

        _leftRandomPositions = ShuffleList(_leftRandomPositions);
        _rightRandomPositions = ShuffleList(_rightRandomPositions);

        for (int j = 0; j < _leftRandomPositions.Count; j++)
        {
            _ConnectorWires[j].RectTransform.anchorMax = new Vector2(_ConnectorWires[j].RectTransform.anchorMax.x, _segment * _leftRandomPositions[j]);
            _ConnectorWires[j].RectTransform.anchorMin = new Vector2(_ConnectorWires[j].RectTransform.anchorMin.x, _segment * _leftRandomPositions[j]);
            _ConnectorWires[j].RectTransform.anchoredPosition = Vector2.zero;

            _ReceiverWires[j].RectTransform.anchorMax = new Vector2(_ReceiverWires[j].RectTransform.anchorMax.x, _segment * _rightRandomPositions[j]);
            _ReceiverWires[j].RectTransform.anchorMin = new Vector2(_ReceiverWires[j].RectTransform.anchorMin.x, _segment * _rightRandomPositions[j]);
            _ReceiverWires[j].RectTransform.anchoredPosition = Vector2.zero;
        }
    }

    List<int> ShuffleList(List<int> intList)
    {
        List<int> _possiblePositions = new List<int>(intList.Count);

        for (int j = 1; j < intList.Count + 1; j++)
        {
            _possiblePositions.Add(j);
        }

        List<int> _finalPosition = new List<int>();

        for (int i = intList.Count - 1; i >= 0; i--)
        {
            int _randomIndex = UnityEngine.Random.Range(0, _possiblePositions.Count);
            _finalPosition.Add(_possiblePositions[_randomIndex]);
            _possiblePositions.RemoveAt(_randomIndex);
        }

        return _finalPosition;
    }

    public void OnClickCloseGame()
    {
        if(!Solved) ResetGame();

        _GameGroup.VisibleAndBlocks(false);
        PlayerMovement.OnOverridePlayerControl?.Invoke(true);
    }

    void ResetGame()
    {
        foreach(WireDragger _connector in _ConnectorWires)
        {
            _connector.ResetConnection();
        }
    }

    protected override void CheckPuzzleCompleted()
    {
        foreach(WireDragger _connector in _ConnectorWires)
        {
            if (!_connector.Connected) return;
        }

        SolvedPuzzle();
        OnClickCloseGame();

    }
}

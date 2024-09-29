using UnityEngine;
using UnityEngine.Events;
using System.Collections;

public enum KeyItems
{
    NULL,
    PIPE,
    WRENCH
}

public class KeyItemFix : Puzzle
{
    [Header("Developer")]
    [SerializeField] Sprite _FixedSprite;
    [SerializeField] float _ProgressionDuration = 0;
    [SerializeField] KeyItems _RequiredKeyItem;

    [Header("References")]
    [SerializeField] SpriteRenderer _RendererToReplaceWhenFixed;
    [SerializeField] SpriteRenderer _ProgressionMeter;
    [SerializeField] GameObject _Bar;
    [SerializeField] UnityEvent _FixedEvent;
    public System.Action<float> ProgressionAction;

    #region Properties

    float _ProgressionTimer;
    Coroutine ProgressRoutine;

    #endregion

    protected override void CheckPuzzleCompleted()
    {
        UsableItemBase _currentItem = Inventory.Singleton.GetCurrentActiveItem();

        if (!IsUsingCorrectItem(_currentItem)) return;

        if (_ProgressionDuration > 0 && _ProgressionTimer < _ProgressionDuration)
        {
            if (ProgressRoutine != null) return;

            ProgressRoutine = StartCoroutine(ProgressTimer());
            return;
        }

        SolvedPuzzle();
    }

    bool IsUsingCorrectItem(UsableItemBase currentItem)
    {
        switch (_RequiredKeyItem)
        {
            case KeyItems.PIPE:
                if (currentItem == null) return false;
                return currentItem.GetType() == typeof(Pipe);
            case KeyItems.WRENCH:
                if (currentItem == null) return false;
                return currentItem.GetType() == typeof(Wrench);
            case KeyItems.NULL:
                return true;
        }

        return false;
    }


    IEnumerator ProgressTimer()
    {
        UsableItemBase _currentItem = Inventory.Singleton.GetCurrentActiveItem();

        while (Input.GetKey(KeyCode.E) && _Interactor.PlayerInRange && IsUsingCorrectItem(_currentItem))
        {
            _ProgressionTimer += Time.deltaTime;
            _Bar.SetActive(true);
            _ProgressionMeter.size = new Vector2(_ProgressionTimer / _ProgressionDuration, _ProgressionMeter.size.y);

            if (_ProgressionTimer > _ProgressionDuration)
            {
                SolvedPuzzle();
                break;
            }

            _currentItem = Inventory.Singleton.GetCurrentActiveItem();

            ProgressionAction?.Invoke(_ProgressionTimer / _ProgressionDuration);
            yield return null;
        }

        ProgressRoutine = null;
    }

    protected override void SolvedPuzzle()
    {
        UsableItemBase _currentItem = Inventory.Singleton.GetCurrentActiveItem();
        Inventory.Singleton.RemoveItem();
        Destroy(_currentItem.gameObject);
        if (_FixedSprite != null && _RendererToReplaceWhenFixed != null)
        {
            _RendererToReplaceWhenFixed.sprite = _FixedSprite;
        }
        _FixedEvent?.Invoke();
        base.SolvedPuzzle();
    }
}

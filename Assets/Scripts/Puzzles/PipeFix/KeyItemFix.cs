using UnityEngine;

public class KeyItemFix : Puzzle
{
    [Header("Developer")]
    [SerializeField] Sprite _FixedSprite;
    [SerializeField] UsableItemBase _RequiredItem;

    [Header("References")]
    [SerializeField] SpriteRenderer _RendererToReplaceWhenFixed;


    protected override void CheckPuzzleCompleted()
    {
        if (Inventory.Singleton.GetCurrentActiveItem() != _RequiredItem) return;

        _RequiredItem.UseItem();
        _RendererToReplaceWhenFixed.sprite = _FixedSprite;
        SolvedPuzzle();
    }
}

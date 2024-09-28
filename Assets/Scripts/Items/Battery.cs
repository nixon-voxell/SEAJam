using UnityEngine;

public class Battery : UsableItemBase
{
    public override void UseItem()
    {
        GameManager.Singleton.ShowBatteryOverlay();
    }

}
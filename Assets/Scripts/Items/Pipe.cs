using UnityEngine;

public class Pipe : UsableItemBase
{
    public override void UseItem()
    {
        base.PickupItem();

        GameManager.Singleton.PickUpPipe();

    }

    public override void PickupItem()
    {
        base.PickupItem();
    }

    public override void DropItem()
    {
        base.DropItem();
        GameManager.Singleton.UsePipe(); 
    }
}
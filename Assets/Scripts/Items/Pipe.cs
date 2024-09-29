using UnityEngine;

public class Pipe : UsableItemBase
{
    public override void UseItem()
    {
        PickupAndPrepareForUse();
    }

    private void PickupAndPrepareForUse()
    {
        base.PickupItem();

        GameManager.Singleton.PickUpPipe();

        Debug.Log("Pipe picked up and ready for repair task.");

    }

    public override void DropItem()
    {
        base.DropItem();
        GameManager.Singleton.UsePipe(); 
        Debug.Log("Pipe dropped before use.");
    }

    public void UseForRepair()
    {
        GameManager.Singleton.UsePipe();
        Debug.Log("Pipe used for repair.");
        
        Inventory.Singleton.RemoveItem();
        Destroy(gameObject);
    }
}
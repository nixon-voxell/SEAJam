using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pipe : UsableItemBase
{
    public override void UseItem()
    {
        Inventory.Singleton.RemoveItem();
        Destroy(gameObject);
    }
}

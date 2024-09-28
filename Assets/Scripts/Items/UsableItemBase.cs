using UnityEngine;

public abstract class UsableItemBase : MonoBehaviour
{
    [SerializeField] private Sprite m_Icon;
    [SerializeField] private Color m_Color = Color.white;
    [SerializeField] private Vector3 m_PositionOffset;

    public Sprite GetSprite()
    {
        return this.m_Icon;
    }

    public Color GetColor()
    {
        return this.m_Color;
    }

    public abstract bool UseItem();

    public virtual void PickupItem()
    {
        GameObject player = PlayerSingleton.Player;
        this.transform.SetParent(player.transform);
        this.transform.SetLocalPositionAndRotation(this.m_PositionOffset, Quaternion.identity);

        Inventory inventory = Inventory.Singleton;
        inventory.SlotItem(this);

    }

    public virtual void DropItem()
    {
        this.transform.SetParent(null);

        Inventory inventory = Inventory.Singleton;
        inventory.RemoveItem();
    }
}

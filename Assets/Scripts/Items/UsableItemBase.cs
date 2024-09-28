using UnityEngine;

public abstract class UsableItemBase : MonoBehaviour
{
    [SerializeField] private Sprite m_Icon;
    [SerializeField] private Color m_Color = Color.white;
    [SerializeField] private Vector3 m_PositionOffset;
    [SerializeField] private ProximityPromptSystem m_Proximity;

    public Sprite GetSprite()
    {
        return this.m_Icon;
    }

    public Color GetColor()
    {
        return this.m_Color;
    }

    public abstract void UseItem();

    public virtual void PickupItem()
    {
        GameObject player = PlayerSingleton.Player;
        this.transform.SetParent(player.transform.Find("ItemHolder"));
        this.transform.SetLocalPositionAndRotation(this.m_PositionOffset, Quaternion.identity);

        Inventory inventory = Inventory.Singleton;
        inventory.SlotItem(this);

        this.m_Proximity.gameObject.SetActive(false);

    }

    public virtual void DropItem()
    {
        this.transform.SetParent(null);

        Inventory inventory = Inventory.Singleton;
        inventory.RemoveItem();

        this.m_Proximity.gameObject.SetActive(true);
    }
}

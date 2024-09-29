using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using Voxell.Util;

public class Inventory : MonoBehaviour
{
    public static Inventory Singleton;

    [Header("Slot References")]
    [SerializeField] private Image[] m_SlotImgs;
    [SerializeField, InspectOnly] private UsableItemBase[] m_Items;

    [Header("Animation Settings")]
    [SerializeField] private float m_EnlargeScale = 1.05f;
    [SerializeField] private float m_AnimationDuration = 0.2f;
    [SerializeField] private Ease m_AnimationEase = Ease.OutQuad;

    [Header("Color Settings")]
    [SerializeField] private Color m_ActiveColor = new Color(1.5f, 1.5f, 0.8f);
    [SerializeField] private Color m_InactiveColor = Color.white;

    private int m_CurrActiveSlot;

    public void SlotItem(UsableItemBase item)
    {
        if (this.m_Items[this.m_CurrActiveSlot] != null)
        {
            this.m_Items[this.m_CurrActiveSlot].DropItem();
        }

        this.m_Items[this.m_CurrActiveSlot] = item;
        var image = this.m_SlotImgs[this.m_CurrActiveSlot];
        image.sprite = item.GetSprite();
        image.color = item.GetColor();
    }

    public void RemoveItem()
    {
        this.m_Items[this.m_CurrActiveSlot] = null;
        var image = this.m_SlotImgs[this.m_CurrActiveSlot];
        image.sprite = null;
        image.color = this.m_ActiveColor;
    }

    private void Awake()
    {
        Inventory.Singleton = this;
    }

    private void Start()
    {
        this.m_Items = new UsableItemBase[this.m_SlotImgs.Length];

        this.m_CurrActiveSlot = 1;
        this.ActivateSlot(0);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            this.ActivateSlot(0);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            this.ActivateSlot(1);
        }

        var currItem = this.m_Items[this.m_CurrActiveSlot];
        if (currItem != null)
        {
            if (Input.GetButtonDown("Fire1"))
            {
                currItem.UseItem();
            }

            if (Input.GetKeyDown(KeyCode.Q))
            {
                currItem.DropItem();
            }
        }
    }

    private void ActivateSlot(int slotNumber)
    {
        if (this.m_CurrActiveSlot == slotNumber)
        {
            return;
        }

        Image activeSlot = this.m_SlotImgs[slotNumber];

        // Deactive every slots first
        foreach (Image slot in this.m_SlotImgs)
        {
            slot.transform.DOScale(Vector3.one, this.m_AnimationDuration).SetEase(m_AnimationEase);
            slot.DOColor(this.m_InactiveColor, this.m_AnimationDuration).SetEase(m_AnimationEase);
        }

        // Activate slot
        activeSlot.transform.DOScale(m_EnlargeScale, m_AnimationDuration).SetEase(m_AnimationEase);
        activeSlot.DOColor(m_ActiveColor, m_AnimationDuration).SetEase(m_AnimationEase);

        this.m_CurrActiveSlot = slotNumber;
        this.SetCorrectItemHideStatus();
    }

    private void SetCorrectItemHideStatus()
    {
        // Hide all items first
        foreach (UsableItemBase item in this.m_Items)
        {
            if (item != null)
            {
                item.SetHideItem(true);
            }
        }

        // Unhide the selected item
        var selectedItem = this.m_Items[this.m_CurrActiveSlot];
        if (selectedItem != null)
        {
            selectedItem.SetHideItem(false);
        }
    }

    public int GetCurrentActiveSlot()
    {
        return m_CurrActiveSlot;
    }

    public UsableItemBase GetCurrentActiveItem()
    {
        return m_Items[m_CurrActiveSlot];
    }
}

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
        this.m_Items[this.m_CurrActiveSlot] = item;
        var image = this.m_SlotImgs[this.m_CurrActiveSlot];
        image.sprite = item.GetSprite();
        image.color = item.GetColor();
    }

    private void Awake()
    {
        Inventory.Singleton = this;
    }

    private void Start()
    {
        this.m_CurrActiveSlot = 0;
        ActivateSlot(this.m_CurrActiveSlot);

        this.m_Items = new UsableItemBase[this.m_SlotImgs.Length];
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            ActivateSlot(0);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            ActivateSlot(1);
        }
    }

    private void ActivateSlot(int slotNumber)
    {
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

        m_CurrActiveSlot = slotNumber;
    }

    public int GetCurrentActiveSlot()
    {
        return m_CurrActiveSlot;
    }
}

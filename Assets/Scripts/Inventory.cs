using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class Inventory : MonoBehaviour
{
    [Header("Slot References")]
    [SerializeField] private Image m_Slot1;
    [SerializeField] private Image m_Slot2;

    [Header("Animation Settings")]
    [SerializeField] private float m_ActivationScale = 1.05f;
    [SerializeField] private float m_DeactivationScale = 1f;
    [SerializeField] private float m_AnimationDuration = 0.2f;
    [SerializeField] private Ease m_AnimationEase = Ease.OutQuad;

    [Header("Color Settings")]
    [SerializeField] private Color m_ActiveColor = new Color(1f, 1f, 0.8f);
    [SerializeField] private Color m_InactiveColor = Color.white;

    private int m_CurrActiveSlot = 0;

    private void Start()
    {
        ActivateSlot(0, true);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            ActivateSlot(1);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            ActivateSlot(2);
        }
    }

    private void ActivateSlot(int slotNumber, bool immediate = false)
    {
        if (slotNumber == m_CurrActiveSlot) return;

        var activeSlot = slotNumber == 1 ? m_Slot1 : m_Slot2;
        var inactiveSlot = slotNumber == 1 ? m_Slot2 : m_Slot1;

        if (immediate)
        {
            activeSlot.transform.localScale = Vector3.one * m_ActivationScale;
            activeSlot.color = m_ActiveColor;
            inactiveSlot.transform.localScale = Vector3.one * m_DeactivationScale;
            inactiveSlot.color = m_InactiveColor;
        }
        else
        {
            activeSlot.transform.DOScale(m_ActivationScale, m_AnimationDuration).SetEase(m_AnimationEase);
            activeSlot.DOColor(m_ActiveColor, m_AnimationDuration).SetEase(m_AnimationEase);

            inactiveSlot.transform.DOScale(m_DeactivationScale, m_AnimationDuration).SetEase(m_AnimationEase);
            inactiveSlot.DOColor(m_InactiveColor, m_AnimationDuration).SetEase(m_AnimationEase);
        }

        m_CurrActiveSlot = slotNumber;
    }

    public int GetCurrentActiveSlot()
    {
        return m_CurrActiveSlot;
    }
}

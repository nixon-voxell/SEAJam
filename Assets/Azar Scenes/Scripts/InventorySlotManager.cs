
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class InventorySlotManager : MonoBehaviour
{
    [Header("Slot References")]
    [SerializeField] private GameObject slot1;
    [SerializeField] private GameObject slot2;

    [Header("Animation Settings")]
    [SerializeField] private float activationScale = 1.05f;
    [SerializeField] private float deactivationScale = 1f;
    [SerializeField] private float animationDuration = 0.2f;
    [SerializeField] private Ease animationEase = Ease.OutQuad;

    [Header("Color Settings")]
    [SerializeField] private Color activeColor = new Color(1f, 1f, 0.8f); 
    [SerializeField] private Color inactiveColor = Color.white;

    private int currentActiveSlot = 1;

    // CLAIMER: If we have different sprites for active slot: will need to do many changes otherwise this works on modulation
    private Image slot1Image;
    private Image slot2Image;

    private void Start()
    {
        slot1Image = slot1.GetComponent<Image>();
        slot2Image = slot2.GetComponent<Image>();

        ActivateSlot(1, true);
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
        if (slotNumber == currentActiveSlot) return;

        GameObject slotToActivate = slotNumber == 1 ? slot1 : slot2;
        GameObject slotToDeactivate = slotNumber == 1 ? slot2 : slot1;
        Image imageToActivate = slotNumber == 1 ? slot1Image : slot2Image;
        Image imageToDeactivate = slotNumber == 1 ? slot2Image : slot1Image;

        if (immediate)
        {
            slotToActivate.transform.localScale = Vector3.one * activationScale;
            imageToActivate.color = activeColor;
            slotToDeactivate.transform.localScale = Vector3.one * deactivationScale;
            imageToDeactivate.color = inactiveColor;
        }
        else
        {
            slotToActivate.transform.DOScale(activationScale, animationDuration).SetEase(animationEase);
            imageToActivate.DOColor(activeColor, animationDuration).SetEase(animationEase);

            slotToDeactivate.transform.DOScale(deactivationScale, animationDuration).SetEase(animationEase);
            imageToDeactivate.DOColor(inactiveColor, animationDuration).SetEase(animationEase);
        }

        currentActiveSlot = slotNumber;
    }

    public int GetCurrentActiveSlot()
    {
        return currentActiveSlot;
    }
}
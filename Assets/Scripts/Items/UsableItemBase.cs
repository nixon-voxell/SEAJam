using UnityEngine;
using UnityEngine.UI;

public abstract class UsableItemBase : MonoBehaviour
{
    [SerializeField] private Image m_Icon;

    public Image GetImage()
    {
        return this.m_Icon;
    }

    public abstract bool UseItem();
}

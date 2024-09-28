using UnityEngine;

public abstract class UsableItemBase : MonoBehaviour
{
    [SerializeField] private Sprite m_Icon;
    [SerializeField] private Color m_Color = Color.white;

    public Sprite GetSprite()
    {
        return this.m_Icon;
    }

    public Color GetColor()
    {
        return this.m_Color;
    }

    public abstract bool UseItem();
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ExtensionMethods
{
    public static void VisibleAndBlocks(this CanvasGroup canvasGroup, bool active)
    {
        canvasGroup.alpha = active ? 1 : 0;
        canvasGroup.blocksRaycasts = active;
    }

    public static Vector2 xy(this Vector3 position)
    {
        return new Vector2 (position.x, position.y);
    }
}

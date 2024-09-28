using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class WireReceiver : MonoBehaviour, IDropHandler
{
    #region Properties

    Image m_Image;
    public RectTransform RectTransform { get; private set; }

    #endregion

    private void Awake()
    {
        m_Image = GetComponent<Image>();
        RectTransform = GetComponent<RectTransform>();
    }

    public void OnDrop(PointerEventData eventData)
    {
        WireDragger _droppedWire = eventData.pointerDrag.GetComponent<WireDragger>();

        if (_droppedWire.color == m_Image.color) _droppedWire.SuccessfulConnection(this);
    }
}

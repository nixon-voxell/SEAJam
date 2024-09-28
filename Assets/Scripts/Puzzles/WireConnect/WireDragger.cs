using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class WireDragger : UILineRenderer, IDragHandler, IEndDragHandler, IBeginDragHandler
{
    #region Properties

    Vector3 _initialPosition;
    public Image Image { get; private set; }
    public bool Connected { get; private set; } = false;
    public RectTransform RectTransform { get; private set; }

    public delegate void ConnectionSuccessful();
    public ConnectionSuccessful OnConnectionSuccessful;

    #endregion

    protected override void Awake()
    {
        base.Awake();
        Image = GetComponent<Image>();
        RectTransform = GetComponent<RectTransform>();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        _Points.Add(eventData.position);
        _initialPosition = transform.position;
        raycastTarget = false;
        SetVerticesDirty();
    }

    public void OnDrag(PointerEventData eventData)
    {
        _Points[2] = eventData.position - transform.position.xy();
        SetVerticesDirty();
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (Connected) return;

        _Points.RemoveAt(2);
        raycastTarget = true;
        SetVerticesDirty();
    }

    public void SuccessfulConnection(WireReceiver receiver)
    {
        Connected = true;
        Vector2 _connectedPosition = receiver.transform.position - transform.position;
        _connectedPosition.x -= receiver.RectTransform.rect.width;
        _Points[2] = _connectedPosition;
        SetVerticesDirty();
        OnConnectionSuccessful?.Invoke();
    }

    public void ResetConnection()
    {
        Connected = false;
        raycastTarget = true;
        if (_Points.Count > 2) _Points.RemoveAt(2);
        SetVerticesDirty();
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UILineRenderer : Graphic
{
    [Header("Developer")]
    [SerializeField] protected List<Vector2> _Points = new List<Vector2>();
    [SerializeField] float _Width;

    protected override void OnPopulateMesh(VertexHelper vh)
    {
        vh.Clear();

        if (_Points.Count <= 1) return;

        float _height = rectTransform.rect.height;
        float _xOffset = _Width * -rectTransform.pivot.x;
        float _yOffset = _height * -rectTransform.pivot.y;

        foreach(Vector3 _point in _Points)
        {
            DrawVerticesForPoint(_point, vh);
        }

        for (int i = 0; i < _Points.Count - 1; i++)
        {
            int _index = i * 2;
            vh.AddTriangle(_index + 0, _index + 1, _index + 3);
            vh.AddTriangle(_index + 3, _index + 2, _index + 0);
        }
    }

    void DrawVerticesForPoint(Vector2 point, VertexHelper vh)
    {
        UIVertex vertex = UIVertex.simpleVert;
        vertex.color = color;

        vertex.position = new Vector3(0, -_Width / 2);
        vertex.position += new Vector3(point.x, point.y);
        vh.AddVert(vertex);

        vertex.position = new Vector3(0, _Width / 2);
        vertex.position += new Vector3(point.x, point.y);
        vh.AddVert(vertex);
    }
}

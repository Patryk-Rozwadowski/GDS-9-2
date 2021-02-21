using System;
using UnityEngine;

public class Grid {
    private int _width, _height;
    private float cellSize;
    private int[,] _gridArray;

    public Grid(int width, int height, float cellSize) {
        _width = width;
        _height = height;
        this.cellSize = cellSize;

        _gridArray = new int[_width, _height];
        Debug.Log($"{nameof(Grid)} - Grid width {_width} and height {_height}");

        for (var x = 0; x < _gridArray.GetLength(0); x++) {
            for (var y = 0; y < _gridArray.GetLength(1); y++) {
                var cellCenter = GetWorldPosition(x, y) + new Vector3(cellSize, cellSize) * .5f;
                CreateWorldText($"X:{x.ToString()}\nY:{y.ToString()}", null, cellCenter, 20, Color.black, TextAlignment.Center,
                    TextAnchor.MiddleCenter);
                Debug.DrawLine(GetWorldPosition(x, y), GetWorldPosition(x, y + 1), Color.white, 100f);
                Debug.DrawLine(GetWorldPosition(x, y), GetWorldPosition(x + 1, y), Color.white, 100f);
            }
        }

        Debug.DrawLine(GetWorldPosition(0, height), GetWorldPosition(width, height), Color.white, 100f);
        Debug.DrawLine(GetWorldPosition(width, 0), GetWorldPosition(width, height), Color.white, 100f);
        Debug.Log($"Height {height} width {width}");
        // Debug.DrawLine(GetWorldPosition(x,y), GetWorldPosition(x + 1,y ), Color.white, 100f); 
    }
    

    private Vector3 GetWorldPosition(int x, int y) => new Vector3(x, y) * cellSize;

    private TextMesh CreateWorldText(string text, Transform parent, Vector3 localPosition, int fontSize, Color color,
        TextAlignment textAligment, TextAnchor textAnchor) {
        GameObject gameObject = new GameObject("Grid_text", typeof(TextMesh));
        Transform transform = gameObject.transform;
        transform.SetParent(parent, false);
        transform.localPosition = localPosition;
        TextMesh textMesh = gameObject.GetComponent<TextMesh>();
        textMesh.anchor = textAnchor;
        textMesh.alignment = textAligment;
        textMesh.text = text;
        textMesh.fontSize = fontSize;
        textMesh.color = color;
        // textMesh.GetComponent<MeshRenderer>().sortingOrder = sortingOrder
        return textMesh;
    }
}
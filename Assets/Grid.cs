using UnityEngine;

public class Grid {
    private int _width, _height;
    private float _cellSize;
    private int[,] _gridArray;
    private TextMesh[,] _debugTextArray;
    
    // TODO Debug mode - nice to have
    private bool _debugMode = false;

    public Grid(int width, int height, float cellSize) {
        _width = width;
        _height = height;
        _cellSize = cellSize;

        _gridArray = new int[width, height];
        _debugTextArray = new TextMesh[width, height];

        Debug.Log($"{nameof(Grid)} - Grid width {_width} and height {_height}");

        for (var x = 0; x < _gridArray.GetLength(0); x++) {
            for (var y = 0; y < _gridArray.GetLength(1); y++) {
                if (_debugMode) DrawDebugCoordinates(x, y);

                var cellCenter = GetWorldPosition(x, y) + new Vector3(cellSize, cellSize) * .5f;
                
                _debugTextArray[x, y] = CreateWorldText(
                    _gridArray[x, y].ToString(),
                    null,
                    cellCenter,
                    20,
                    Color.black,
                    TextAlignment.Center,
                    TextAnchor.MiddleCenter
                );

                DrawWall(x, y, x, y + 1);
                DrawWall(x, y, x + 1, y);
            }
        }

        DrawWall(0, height, width, height);
        DrawWall(width, 0, width, height);
        SetValue(0, 0, 20);
    }


    private Vector3 GetWorldPosition(int x, int y) => new Vector3(x, y) * _cellSize;

    private TextMesh CreateWorldText(
        string text,
        Transform parent,
        Vector3 localPosition,
        int fontSize,
        Color color,
        TextAlignment textAligment,
        TextAnchor textAnchor
    ) {
        var gameObject = new GameObject("Grid_text", typeof(TextMesh));
        var transform = gameObject.transform;
        var textMesh = gameObject.GetComponent<TextMesh>();
        transform.SetParent(parent, false);
        transform.localPosition = localPosition;
        textMesh.anchor = textAnchor;
        textMesh.alignment = textAligment;
        textMesh.text = text;
        textMesh.fontSize = fontSize;
        textMesh.color = color;
        return textMesh;
    }

    private void SetValue(int x, int y, int value) {
        if (x >= 0 && y >= 0 && x < _width && y < _height) {
            _gridArray[x, y] = value;
            _debugTextArray[x, y].text = _gridArray[x, y].ToString();
        }
        else {
            Debug.Log("Invalid values");
        }
    }

    private void DrawDebugCoordinates(int x, int y) {
        var cellCenter = GetWorldPosition(x, y) + new Vector3(_cellSize, _cellSize) * .5f;
        CreateWorldText($"X:{x.ToString()}\nY:{y.ToString()}", null, cellCenter, 20, Color.black, TextAlignment.Center,
            TextAnchor.MiddleCenter);
    }

    private void DrawWall(int startX, int startY, int endX, int endY) {
        Debug.DrawLine(GetWorldPosition(startX, startY), GetWorldPosition(endX, endY), Color.white, 100f);
    }
}
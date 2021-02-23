﻿using UnityEngine;

public class Grid<TGridObject> {
    private int _width, _height;
    private float _cellSize;
    private TGridObject[,] _gridArray;
    private TextMesh[,] _debugTextArray;
    private Vector3 _originPosition;
    private Camera _mainCamera;

    // TODO Debug mode - nice to have
    private bool _debugMode = false;

    public Grid(int width, int height, float cellSize, Vector3 originPosition) {
        _width = width;
        _height = height;
        _cellSize = cellSize;
        _originPosition = originPosition;

        _gridArray = new TGridObject[width, height];
        _debugTextArray = new TextMesh[width, height];
  
        _mainCamera = Camera.main;

        Debug.Log($"{nameof(Grid)} - Grid width {_width} and height {_height}");

        for (var x = 0; x < _gridArray.GetLength(0); x++) {
            for (var y = 0; y < _gridArray.GetLength(1); y++) {
                var cellCenter = GetWorldPosition(x, y) + new Vector3(cellSize, cellSize) * .5f;

                if (_debugMode) GridUtils.DrawDebugCoordinates(x, y, cellSize, originPosition);
                else
                    _debugTextArray[x, y] = GridUtils.CreateWorldText(
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
    }

    public TGridObject GetValue(int x, int y) {
        if (x >= 0 && y >= 0 && x < _width && y < _height) {
            return _gridArray[x, y];
        }
        else {
            return default(TGridObject);
        };
    }

    public TGridObject GetValue(Vector3 worldPosition) {
        GetXY(worldPosition, out var x, out var y);
        return GetValue(x, y);
    }

    public void SetValue(int x, int y, TGridObject value) {
        if (x >= 0 && y >= 0 && x < _width && y < _height) {
            _gridArray[x, y] = value;
            _debugTextArray[x, y].text = _gridArray[x, y].ToString();
        }
        else {
            Debug.Log("Invalid values");
        }
    }

    public void SetValue(Vector3 worldPosition, TGridObject value) {
        int x, y;
        GetXY(worldPosition, out x, out y);
        SetValue(x, y, value);
    }

    private void GetXY(Vector3 worldPosition, out int x, out int y) {
        x = Mathf.FloorToInt((worldPosition - _originPosition).x / _cellSize);
        y = Mathf.FloorToInt((worldPosition - _originPosition).y / _cellSize);
    }

    private Vector3 GetWorldPosition(int x, int y) => new Vector3(x, y) * _cellSize + _originPosition;

    private void DrawWall(int startX, int startY, int endX, int endY) {
        Debug.DrawLine(GetWorldPosition(startX, startY), GetWorldPosition(endX, endY), Color.white, 100f);
    }
}
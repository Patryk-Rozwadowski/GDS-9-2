using System;
using UnityEngine;

public class Grid<TGridObject> {
    private readonly float _cellSize;

    // TODO Debug mode - nice to have
    private bool _debugMode = true;
    private readonly TextMesh[,] _debugTextArray;
    private readonly TGridObject[,] _gridArray;
    private Camera _mainCamera;
    private readonly Vector3 _originPosition;

    private GameObject _respawn;

    private readonly int _width;
    private readonly int _height;

    public Grid(
        int width,
        int height,
        float cellSize,
        Vector3 originPosition,
        Func<Grid<TGridObject>, int, int, TGridObject> createDefaultGridObject
    ) {
        _width = width;
        _height = height;
        _cellSize = cellSize;
        _originPosition = originPosition;

        _gridArray = new TGridObject[width, height];
        _debugTextArray = new TextMesh[width, height];

        _mainCamera = Camera.main;

        Debug.Log($"{nameof(Grid)} - Grid width {_width} and height {_height}");

        for (var x = 0; x < _gridArray.GetLength(0); x++)
        for (var y = 0; y < _gridArray.GetLength(1); y++)
            _gridArray[x, y] = createDefaultGridObject(this, x, y);

        OnGridObjectChanged += (sender, eventArgs) => {
            _debugTextArray[eventArgs.x, eventArgs.y].text = _gridArray[eventArgs.x, eventArgs.y]?.ToString();
        };
    }

    public event EventHandler<OnGridObjectChangedEventArgs> OnGridObjectChanged;

    public int GetWidth() {
        return _gridArray.GetLength(0);
    }

    public int GetHeight() {
        return _gridArray.GetLength(1);
    }

    public float GetCellSize() {
        return _cellSize;
    }

    public TGridObject GetGridObject(Vector3 worldPosition) {
        GetXY(worldPosition, out var x, out var y);
        return GetGridObject(x, y);
    }


    public void SetRespawn(GameObject respawn) {
        _respawn = respawn;
    }

    public TGridObject GetGridObject(int x, int y) {
        if (x >= 0 && y >= 0 && x < _width && y < _height)
            return _gridArray[x, y];
        return default;
    }

    public void TriggerGridObjectChanged(int x, int y) {
        Debug.Log("TriggerGridObjectChanged event");
        OnGridObjectChanged?.Invoke(this, new OnGridObjectChangedEventArgs {x = x, y = y});
    }

    public void SetGridObject(Vector3 worldPosition, TGridObject value) {
        int x, y;
        GetXY(worldPosition, out x, out y);
        SetGridObject(x, y, value);
    }

    public void GetXY(Vector3 worldPosition, out int x, out int y) {
        x = Mathf.FloorToInt((worldPosition - _originPosition).x / _cellSize);
        y = Mathf.FloorToInt((worldPosition - _originPosition).y / _cellSize);
    }

    private Vector3 GetWorldPosition(int x, int y) {
        return new Vector3(x, y) * _cellSize + _originPosition;
    }

    private void DrawWall(int startX, int startY, int endX, int endY) {
        Debug.DrawLine(GetWorldPosition(startX, startY), GetWorldPosition(endX, endY), Color.white, 100f);
    }

    public void SetGridObject(int x, int y, TGridObject value) {
        if (x >= 0 && y >= 0 && x < _width && y < _height) {
            _gridArray[x, y] = value;
            _debugTextArray[x, y].text = _gridArray[x, y].ToString();
            if (OnGridObjectChanged != null) OnGridObjectChanged(this, new OnGridObjectChangedEventArgs {x = x, y = y});
        }
        else {
            Debug.Log("Invalid values");
        }
    }

    public class OnGridObjectChangedEventArgs : EventArgs {
        public int x, y;
    }
}
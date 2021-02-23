using UnityEngine;

public class GridObject : MonoBehaviour {
    private Grid<HeatMapGridObject> _grid;

    private void Start() {
        _grid = new Grid<HeatMapGridObject>(10, 15, 10f, Vector3.zero,
            (grid, x, y) => new HeatMapGridObject(grid, x, y));
    }

    private void Update() {
        if (Input.GetMouseButtonDown(1)) {
            var mouseClickPosition = CursorUtils.GetMouseWorldPosition();
            HeatMapGridObject heatMapGridObjectGridObject = _grid.GetGridObject(mouseClickPosition);

            if (heatMapGridObjectGridObject != null) {
                heatMapGridObjectGridObject.AddValue(5);
            }
        }
    }
}

public class HeatMapGridObject {
    private const int min = 0;
    private const int max = 100;
    private int _x, _y, _value;

    private Grid<HeatMapGridObject> _grid;

    public HeatMapGridObject(Grid<HeatMapGridObject> grid, int x, int y) {
        _grid = grid;
        _x = x;
        _y = y;
    }

    public void AddValue(int addValue) {
        _value += addValue;
        _value = Mathf.Clamp(_value, min, max);
        _grid.TriggerGridObjectChanged(_x, _y);
    }

    public float GetValueNormalized() {
        return (float) _value / max;
    }

    public override string ToString() {
        return _value.ToString();
    }
}
using UnityEngine;

public class GridObject : MonoBehaviour {
    private Grid _grid;
    private void Start() {
        _grid = new Grid(10, 15, 10f);
    }

    private void Update() {
        if (Input.GetMouseButtonDown(0)) {
            _grid.SetValue(_grid.GetMouseWorldPosition(), 100);
        }

        if (Input.GetMouseButtonDown(1)) {
            Debug.Log(_grid.GetValue(_grid.GetMouseWorldPosition()));
        }
    }
}
using UnityEngine;

public class GridObject : MonoBehaviour {
    private Grid<bool> _grid;
    private void Start() {
        _grid = new Grid<bool>(10, 15, 10f, Vector3.zero);
    }

    private void Update() {
        if (Input.GetMouseButtonDown(0)) {
        }

        if (Input.GetMouseButtonDown(1)) {
            Debug.Log(_grid.GetValue(CursorUtils.GetMouseWorldPosition()));
        }
    }
}
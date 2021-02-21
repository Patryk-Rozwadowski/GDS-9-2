using System;
using UnityEngine;

public class GridPlacementController : MonoBehaviour {
    [SerializeField] private GameObject unit;
    private Vector2 _gridPosition;
    private int _gridWidthSize, _gridHeightSize;

    private void Start() {
        CheckIfGridHasCorrectSize();
        _gridWidthSize = Resources.Load<GridSo>("Data/GridData").gridWidth;
        _gridHeightSize = Resources.Load<GridSo>("Data/GridData").gridHeight;
    }

    private void LateUpdate() {
        var unitPosition = unit.transform.position;
        _gridPosition.x = Mathf.Floor(unitPosition.x / _gridWidthSize) * _gridWidthSize + (_gridWidthSize * .5f);
        _gridPosition.y = Mathf.Floor(unitPosition.y / _gridHeightSize) * _gridHeightSize + (_gridHeightSize * .5f);

        unit.transform.position = _gridPosition;
    }

    private void CheckIfGridHasCorrectSize() {
        if (_gridWidthSize == 0 || _gridHeightSize == 0) {
            Debug.LogError("Grid cannot be size 0!");
        }
    }
}
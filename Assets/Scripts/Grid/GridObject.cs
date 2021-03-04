using System.Collections.Generic;
using UnityEngine;

public class GridObject : MonoBehaviour {
    [SerializeField] private GameObject objectOnMap;
    [SerializeField] private GameObject walkablePrefab, unwalkable;
    public static GridPathfinding gridPathfinding;
    private Vector3 _objectOnGridPosition;
    private int _gridWidth, _gridHeight;
    private void Start() {
        gridPathfinding = new GridPathfinding(Vector3.zero, new Vector3(100, 80), 10f);
        gridPathfinding.RaycastWalkable();
        gridPathfinding.PrintMap(walkablePrefab.transform, unwalkable.transform);

        _gridHeight = gridPathfinding.gridMapHeight;
        _gridWidth = gridPathfinding.gridMapWidth;

        _objectOnGridPosition.x = Mathf.Floor(objectOnMap.transform.position.x / _gridWidth) * _gridWidth;
        _objectOnGridPosition.y = Mathf.Floor(objectOnMap.transform.position.y / _gridHeight) * _gridHeight;
        _objectOnGridPosition.z = 0;
        objectOnMap.transform.position = _objectOnGridPosition;
    }
}
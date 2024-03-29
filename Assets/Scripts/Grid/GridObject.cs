﻿using System.Collections.Generic;
using UnityEngine;

public class GridObject : MonoBehaviour {
    private static GridPathfinding gridPathfinding;

    [Tooltip("Objects on map are all objects which has to be snapped to grid tiles.")] [SerializeField]
    private List<GameObject> objectsOnMap;

    [Tooltip("GameObject prefabs for grid tiles.")] [SerializeField]
    private GameObject walkablePrefab, unwalkable;

    [Tooltip("Test object to check in editor mode how grid snapping is working.")] [SerializeField]
    private GameObject testObjectForGridSnapTests;

    private bool _debugMode = true;
    private int _nodeSize;
    private Vector3 _objectOnGridPosition;
    private Vector3 _worldOrigin;

    private void Start() {
        // TODO nice to have - scriptable objects for sizes
        _nodeSize = 17;
        var mapWidth = 10;
        var mapHeight = 8;
        _worldOrigin = new Vector3(0, 0);

        gridPathfinding = new GridPathfinding(
            _worldOrigin + new Vector3(1, 1) * _nodeSize * .5f,
            new Vector3(mapWidth, mapHeight) * _nodeSize, _nodeSize
        );
        gridPathfinding.RaycastWalkable();
        gridPathfinding.PrintMap((vec, size, color) =>
            Instantiate(walkablePrefab, vec, Quaternion.identity));
        foreach (var objectOnMap in objectsOnMap) {
            if (objectOnMap == null) return;

            var origin = new Vector3(0, 0);
            var cellSize = 17;
            var cellCenter = cellSize / 2;
            // Objects has to be in objectOnMap list in order to snap to grid
            var objectOnMapTransformPosition = objectOnMap.transform.position;
            // _objectOnGridPosition.x = Mathf.Floor(objectOnMapTransformPosition.x / _nodeSize) * _nodeSize;
            // _objectOnGridPosition.y = Mathf.Floor(objectOnMapTransformPosition.y / _nodeSize) * _nodeSize;
            // _objectOnGridPosition.z = 0;
            objectOnMap.transform.position = new Vector3(cellCenter + gameObject.transform.position.x * cellSize,
                cellCenter + gameObject.transform.position.y * cellSize) + new Vector3(1, 1) * 0.5f;
        }

        if (testObjectForGridSnapTests == null) {
            Debug.Log("No test object is provided");
            _debugMode = false;
        }

        ;
    }

    // private void LateUpdate() {
    //     // TODO add method to eleminate duplicate code
    //     foreach (var objectOnMap in objectsOnMap) {
    //         if (objectOnMap == null) return;
    //         var origin = new Vector3(0,0);
    //         var cellSize = 17;
    //         var cellCenter = cellSize / 2;
    //         // Objects has to be in objectOnMap list in order to snap to grid
    //         var objectOnMapTransformPosition = objectOnMap.transform.position;
    //         // _objectOnGridPosition.x = Mathf.Floor(objectOnMapTransformPosition.x / _nodeSize) * _nodeSize;
    //         // _objectOnGridPosition.y = Mathf.Floor(objectOnMapTransformPosition.y / _nodeSize) * _nodeSize;
    //         // _objectOnGridPosition.z = 0;
    //         objectOnMap.transform.position = new Vector3(cellCenter + (gameObject.transform.position.x* cellSize),  cellCenter +(gameObject.transform.position.y * cellSize)) + new Vector3(1,1) * 0.5f;
    //     }
    // }
}
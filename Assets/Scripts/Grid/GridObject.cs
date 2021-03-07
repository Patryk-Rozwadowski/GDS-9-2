﻿using System.Collections.Generic;
using UnityEngine;

public class GridObject : MonoBehaviour {
    [Tooltip("Objects on map are all objects which has to be snapped to grid tiles.")]
    [SerializeField] private List<GameObject> objectsOnMap;
    
    [Tooltip("GameObject prefabs for grid tiles.")]
    [SerializeField] private GameObject walkablePrefab, unwalkable;
    
    [Tooltip("Test object to check in editor mode how grid snapping is working.")]
    [SerializeField] private GameObject testObjectForGridSnapTests;
    
    private static GridPathfinding _gridPathfinding;
    private Vector3 _objectOnGridPosition;
    private int _nodeSize;
    private bool _debugMode = true;
    
    private void Start() {
        // TODO nice to have - scriptable objects for sizes
        _nodeSize = 10;
        _gridPathfinding = new GridPathfinding(Vector3.zero, new Vector3(100, 80), _nodeSize);
        _gridPathfinding.RaycastWalkable();
        _gridPathfinding.PrintMap(walkablePrefab.transform, unwalkable.transform);

        foreach (var objectOnMap in objectsOnMap) {
            if (objectOnMap == null) return;
            
            // Objects has to be in objectOnMap list in order to snap to grid
            var objectOnMapTransformPosition = objectOnMap.transform.position;
            _objectOnGridPosition.x = Mathf.Floor(objectOnMapTransformPosition.x / _nodeSize) * _nodeSize;
            _objectOnGridPosition.y = Mathf.Floor(objectOnMapTransformPosition.y / _nodeSize) * _nodeSize;
            _objectOnGridPosition.z = 0;
            objectOnMap.transform.position = _objectOnGridPosition;
        }
        
        if (testObjectForGridSnapTests == null) {
            Debug.Log("No test object is provided");
            _debugMode = false;
        };
    }
    
    private void LateUpdate() {
        // TODO add method to eleminate duplicate code
        if (_debugMode == false) return;
        var testObjectForGridSnapTestsTransformPositon = testObjectForGridSnapTests.transform.position;
        _objectOnGridPosition.x = Mathf.Floor(testObjectForGridSnapTestsTransformPositon.x / _nodeSize) * _nodeSize;
        _objectOnGridPosition.y = Mathf.Floor(testObjectForGridSnapTestsTransformPositon.y / _nodeSize) * _nodeSize;
        _objectOnGridPosition.z = 0;
        testObjectForGridSnapTests.transform.position = _objectOnGridPosition;
    }
}
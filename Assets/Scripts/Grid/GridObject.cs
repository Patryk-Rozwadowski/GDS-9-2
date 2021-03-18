using System.Collections.Generic;
using UnityEngine;
public class GridObject : MonoBehaviour {
    [Tooltip("Objects on map are all objects which has to be snapped to grid tiles.")]
    [SerializeField] private List<GameObject> objectsOnMap;
    
    [Tooltip("GameObject prefabs for grid tiles.")]
    [SerializeField] private GameObject walkablePrefab, unwalkable;
    
    [Tooltip("Test object to check in editor mode how grid snapping is working.")]
    [SerializeField] private GameObject testObjectForGridSnapTests;
    
    private static GridPathfinding gridPathfinding;
    private Vector3 _objectOnGridPosition;
    private int _nodeSize;
    private bool _debugMode = true;
    private Vector3 _worldOrigin;
    
    private void Start() {
        // TODO nice to have - scriptable objects for sizes
        _nodeSize = 17;
        int mapWidth = 10;
        int mapHeight = 8;
        _worldOrigin = new Vector3(0, 0);

        gridPathfinding = new GridPathfinding(
            _worldOrigin + new Vector3(1, 1) * _nodeSize * .5f,
            new Vector3(mapWidth, mapHeight) * _nodeSize, _nodeSize
            );
        gridPathfinding.RaycastWalkable();
        gridPathfinding.PrintMap((Vector3 vec, Vector3 size, Color color) => Instantiate(walkablePrefab, vec, Quaternion.identity));
        foreach (var objectOnMap in objectsOnMap) {
            if (objectOnMap == null) return;
            
            // Objects has to be in objectOnMap list in order to snap to grid
            var objectOnMapTransformPosition = objectOnMap.transform.position;
            // _objectOnGridPosition.x = Mathf.Floor(objectOnMapTransformPosition.x / _nodeSize) * _nodeSize;
            // _objectOnGridPosition.y = Mathf.Floor(objectOnMapTransformPosition.y / _nodeSize) * _nodeSize;
            // _objectOnGridPosition.z = 0;
            objectOnMap.transform.position = _worldOrigin + new Vector3(objectOnMapTransformPosition.x / _nodeSize, objectOnMapTransformPosition.y / _nodeSize) * _nodeSize;
        }
        
        if (testObjectForGridSnapTests == null) {
            Debug.Log("No test object is provided");
            _debugMode = false;
        };
    }
    
    private void LateUpdate() {
        // TODO add method to eleminate duplicate code
        foreach (var objectOnMap in objectsOnMap) {
            if (objectOnMap == null) return;
            
            // Objects has to be in objectOnMap list in order to snap to grid
            var objectOnMapTransformPosition = objectOnMap.transform.position;
            // _objectOnGridPosition.x = Mathf.Floor(objectOnMapTransformPosition.x / _nodeSize) * _nodeSize;
            // _objectOnGridPosition.y = Mathf.Floor(objectOnMapTransformPosition.y / _nodeSize) * _nodeSize;
            // _objectOnGridPosition.z = 0;
            objectOnMap.transform.position = new Vector3(objectOnMapTransformPosition.x / _nodeSize, objectOnMapTransformPosition.y / _nodeSize) * _nodeSize;
        }
    }
}
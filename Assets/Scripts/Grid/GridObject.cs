using System.Collections.Generic;
using UnityEngine;

public class GridObject : MonoBehaviour {
    [SerializeField] private GameObject walkablePrefab, unwalkable;
    public static GridPathfinding gridPathfinding;

    private void Start() {
        gridPathfinding = new GridPathfinding(Vector3.zero, new Vector3(100, 80), 10f);
        gridPathfinding.RaycastWalkable();
        gridPathfinding.PrintMap(walkablePrefab.transform, unwalkable.transform);
    }
}
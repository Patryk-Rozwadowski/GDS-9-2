using System;
using UnityEngine;

public class Unit : MonoBehaviour {
    private bool isActive;
    private void OnMouseDown() {
        Debug.Log($"Mouse over {gameObject.name}");
        GetComponent<MovePositionPathfinding>().isActive = !isActive;
    }
}
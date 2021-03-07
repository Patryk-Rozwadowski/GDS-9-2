using UnityEngine;

public class Unit : MonoBehaviour {
    private bool _isActive;

    private MovePositionPathfinding _thisUnitPathFinding;
    private void Start() {
        _thisUnitPathFinding = GetComponent<MovePositionPathfinding>();
        _isActive = false;
    }

    private void OnMouseDown() {
        _isActive = !_isActive;
        Debug.Log($"{gameObject.name} is active: {_isActive}");
        _thisUnitPathFinding.isActive = !_isActive;
    }
}
using UnityEngine;

public class GridObject : MonoBehaviour {

    private void Start() {
        Pathfinding pathfinding = new Pathfinding(10, 10);
    }
}
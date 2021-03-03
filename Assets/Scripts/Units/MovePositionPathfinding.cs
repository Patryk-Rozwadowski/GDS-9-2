using System.Collections.Generic;
using UnityEngine;

public class MovePositionPathfinding : MonoBehaviour, IMovePosition {
    private List<Vector3> pathVectorList;
    private int pathIndex = -1;

    public void SetMovePosition(Vector3 movePosition) {
        pathVectorList = GridPathfinding.instance.GetPathRouteWithShortcuts(transform.position, movePosition).pathVectorList;
        if (pathVectorList.Count > 0) {
            // Remove first position so he doesn't go backwards
            pathVectorList.RemoveAt(0);
        }
        if (pathVectorList.Count > 0) {
            pathIndex = 0;
        } else {
            pathIndex = -1;
        }
    }

    private void Update() {

        if (pathIndex != -1) {
            // Move to next path position
            Vector3 nextPathPosition = pathVectorList[pathIndex];
            // Debug.Log($"next path position {nextPathPosition}");
            Vector3 moveVelocity = (nextPathPosition - transform.position).normalized;
            // Debug.Log($"move velocity {moveVelocity}");
            GetComponent<IMoveVelocity>().SetVelocity(moveVelocity);

            int reachedPathPositionDistance = 1;
            if (Vector3.Distance(transform.position, nextPathPosition) <= reachedPathPositionDistance) {
                pathIndex++;
                Debug.Log($"pathIndex {pathIndex}");
                if (pathIndex >= pathVectorList.Count) {
                    // End of path
                    pathIndex = -1;
                }
            }
        } else {
            // Idle
            GetComponent<IMoveVelocity>().SetVelocity(Vector3.zero);
        }
    }
}
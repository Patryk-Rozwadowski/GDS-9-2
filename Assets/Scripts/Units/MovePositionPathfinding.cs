using System;
using System.Collections.Generic;
using UnityEngine;

public class MovePositionPathfinding : MonoBehaviour {
    private MoveTransformVelocity _moveVelocity;
    private Action _onReachedTargetPosition;
    private int pathIndex = -1;
    private List<Vector3> pathVectorList;

    private void Update() {
        if (pathIndex != -1) {
            // Move to next path position
            var nextPathPosition = pathVectorList[pathIndex];
            var moveVelocity = (nextPathPosition - transform.position).normalized;
            GetComponent<IMoveVelocity>().SetVelocity(moveVelocity);

            var reachedPathPositionDistance = 0.75f;
            if (Vector3.Distance(transform.position, nextPathPosition) <= reachedPathPositionDistance) {
                pathIndex++;
                if (pathIndex >= pathVectorList.Count) {
                    // End of path
                    pathIndex = -1;
                    _onReachedTargetPosition();
                }
            }
        }
        else {
            GetComponent<IMoveVelocity>().SetVelocity(Vector3.zero);
        }
    }

    public void SetMovePosition(Vector3 movePosition, Action onReachedTargetPosition) {
        _onReachedTargetPosition = onReachedTargetPosition;
        pathVectorList = GridPathfinding.instance.GetPathRouteWithShortcuts(transform.position, movePosition)
            .pathVectorList;
        if (pathVectorList.Count > 0)
            pathIndex = 0;
        else
            pathIndex = -1;
    }
}
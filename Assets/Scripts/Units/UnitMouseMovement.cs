using System;
using UnityEngine;

public class UnitMouseMovement : MonoBehaviour {
    private void Update() {
        // TODO scriptableobject controll
        if (Input.GetMouseButtonDown(0)) {
            GetComponent<IMovePosition>().SetMovePosition(CursorUtils.GetMouseWorldPosition());
        }
    }
}
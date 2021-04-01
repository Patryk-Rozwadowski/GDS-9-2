using UnityEngine;

public static class CursorUtils {
    public static Vector3 GetMouseWorldPosition() {
        var vec = GetMouseWorldPositionWithZ(Input.mousePosition, Camera.main);
        vec.z = 0;
        return vec;
    }

    public static Vector3 GetMouseWorldPositionWithZ(Vector3 screenPosition, Camera worldCamera) {
        var worldPosition = worldCamera.ScreenToWorldPoint(screenPosition);
        return worldPosition;
    }
}
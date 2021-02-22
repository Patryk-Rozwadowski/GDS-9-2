using UnityEngine;

public class GridUtils {
    public static Vector3 GetWorldPosition(int x, int y, float cellSize, Vector3 originPosition) =>  new Vector3(x, y) * cellSize + originPosition;
    
    public static void DrawDebugCoordinates(int x, int y, float cellSize, Vector3 originPosition) {
        var cellCenter = GetWorldPosition(x, y, cellSize, originPosition) + new Vector3(cellSize, cellSize) * .5f;
        CreateWorldText($"X:{x.ToString()}\nY:{y.ToString()}", null, cellCenter, 20, Color.black, TextAlignment.Center,
            TextAnchor.MiddleCenter);
    }
    
    public static TextMesh CreateWorldText(
        string text,
        Transform parent,
        Vector3 localPosition,
        int fontSize,
        Color color,
        TextAlignment textAligment,
        TextAnchor textAnchor
    ) {
        var gameObject = new GameObject("Grid_text", typeof(TextMesh));
        var transform = gameObject.transform;
        var textMesh = gameObject.GetComponent<TextMesh>();
        transform.SetParent(parent, false);
        transform.localPosition = localPosition;
        textMesh.anchor = textAnchor;
        textMesh.alignment = textAligment;
        textMesh.text = text;
        textMesh.fontSize = fontSize;
        textMesh.color = color;
        return textMesh;
    }
}
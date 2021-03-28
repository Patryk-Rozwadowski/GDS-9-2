using UnityEngine;

public class GameController_GridCombatSystem : MonoBehaviour {
    [SerializeField] private int cellSize = 10;
    [SerializeField] private GameObject walkablePrefab, unwalkablePrefab;
    public static GameController_GridCombatSystem Instance { get; private set; }
    public GridPathfinding gridPathfinding;
    private Grid<GridCombatSystem.GridObject> _grid;

    
    private void Awake() {
        Instance = this;
        
        var mapWidth = 10;
        var mapHeight = 8;
        
        Vector3 origin = new Vector3(0, 0);
        
        _grid = new Grid<GridCombatSystem.GridObject>(
            mapWidth,
            mapHeight,
            cellSize,
            origin,
            (Grid<GridCombatSystem.GridObject> g, int x, int y) => new GridCombatSystem.GridObject(g, x, y)
        );

        gridPathfinding = new GridPathfinding(origin + new Vector3(1, 1) * cellSize * .5f,
            new Vector3(mapWidth, mapHeight) * cellSize, cellSize);
        gridPathfinding.RaycastWalkable();

        var gridTile = Resources.Load("Sprites/grid", typeof(GameObject)) as GameObject;
        gridTile.transform.localScale = new Vector3(14, 14, 10);
        gridPathfinding.PrintMap((Vector3 vec, Vector3 size, Color color) => Instantiate(gridTile, vec, Quaternion.identity));
    }

    public Grid<GridCombatSystem.GridObject> GetGrid() {
        return _grid;
    }
}
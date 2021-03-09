using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController_GridCombatSystem : MonoBehaviour {
    public static GameController_GridCombatSystem Instance { get; private set; }
    public GridPathfinding gridPathfinding;
    private Grid<GridCombatSystem.GridObject> _grid;
    private GridPathfinding _gridPathfinding;
    
    void Start() {
        Instance = this;
        
        int mapWidth = 10;
        int mapHeight = 8;
        float cellSize = 10f;
        Vector3 origin = new Vector3(0, 0);

        _grid = new Grid<GridCombatSystem.GridObject>(mapWidth, mapHeight, cellSize, origin, (Grid<GridCombatSystem.GridObject> g, int x, int y) => new GridCombatSystem.GridObject(g, x, y));

        gridPathfinding = new GridPathfinding(origin + new Vector3(1, 1) * cellSize * .5f, new Vector3(mapWidth, mapHeight) * cellSize, cellSize);
        // _gridPathfinding.RaycastWalkable();
        // gridPathfinding.PrintMap((Vector3 vec, Vector3 size, Color color) => World_Sprite.Create(vec, size, color));

    }

    public Grid<GridCombatSystem.GridObject> GetGrid() {
        return _grid;
    }
}
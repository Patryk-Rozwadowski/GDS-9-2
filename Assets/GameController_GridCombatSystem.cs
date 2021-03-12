using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController_GridCombatSystem : MonoBehaviour {
    public static GameController_GridCombatSystem Instance { get; private set; }
    public GridPathfinding gridPathfinding;
    private Grid<GridCombatSystem.GridObject> _grid;
    
    
    public Texture2D tex;
    private Sprite mySprite;
    private SpriteRenderer sr;

    private void Awake() {
        Instance = this;
        
        int mapWidth = 40;
        int mapHeight = 25;
        float cellSize = 10f;
        Vector3 origin = new Vector3(0, 0);

        _grid = new Grid<GridCombatSystem.GridObject>(mapWidth, mapHeight, cellSize, origin, (Grid<GridCombatSystem.GridObject> g, int x, int y) => new GridCombatSystem.GridObject(g, x, y));

        gridPathfinding = new GridPathfinding(origin + new Vector3(1, 1) * cellSize * .5f, new Vector3(mapWidth, mapHeight) * cellSize, cellSize);
        gridPathfinding.RaycastWalkable();
        

    }

    private void Start() {
        gridPathfinding.PrintMap((Vector3 vec, Vector3 size, Color color) => {
            mySprite = Sprite.Create(tex, new Rect(0.0f, 0.0f, tex.width, tex.height), vec, 100.0f);

            transform.position = vec;
        });
    }

    public Grid<GridCombatSystem.GridObject> GetGrid() {
        return _grid;
    }
}
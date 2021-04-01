using System;
using UnityEngine;

public class GameController_GridCombatSystem : MonoBehaviour {
    [SerializeField] private int cellSize = 10;
    [SerializeField] private GameObject walkablePrefab, unwalkablePrefab;
    [SerializeField] private GameObject _leftTeamRespawn, _rightTeamRespawn;

    public static GameController_GridCombatSystem Instance { get; private set; }
    public GridPathfinding gridPathfinding;
    private Grid<GridCombatSystem.GridObject> _grid;

    private Transform _gridRespawnLeftContainer, _gridRespawnRightContainer;
    private GameObject _gridTileBorder, _gridTileMovement, _gridTileAttackRange;

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
        gridPathfinding.PrintMap((Vector3 vec, Vector3 size, Color color) =>
            Instantiate(gridTile, vec, Quaternion.identity));

        _gridRespawnLeftContainer = GameObject.Find("GridRespawnLeftContainer").transform;
        _gridRespawnRightContainer = GameObject.Find("GridRespawnRightContainer").transform;
    }

    private void Start() {
        _gridTileMovement = Resources.Load("Sprites/grid-move", typeof(GameObject)) as GameObject;

        var cellCenter = cellSize / 2;
        for (var y = 0; y < _grid.GetHeight(); y++) {
            var leftTeamRespawnTile = Instantiate(
                _gridTileMovement,
                new Vector3(
                    cellCenter, cellCenter + (y * cellSize)) +
                new Vector3(1, 1) * 0.5f,
                Quaternion.identity
            );

            leftTeamRespawnTile.tag = "LeftRespawn";
            leftTeamRespawnTile.transform.parent = _gridRespawnLeftContainer.transform;
            _gridTileMovement.transform.localScale = new Vector3(14, 14, 10);
            if (leftTeamRespawnTile == null || _grid == null) return;
            _grid
                .GetGridObject(leftTeamRespawnTile.transform.position)
                .SetRespawn(leftTeamRespawnTile);

        }

        for (var y = 0; y < _grid.GetHeight(); y++) {
            var rightTeamRespawnTile = Instantiate(
                _gridTileMovement,
                new Vector3(
                    cellCenter + ((_grid.GetWidth() - 1) * cellSize), cellCenter + (y * cellSize)) +
                new Vector3(1, 1) * 0.5f,
                Quaternion.identity);

            if (rightTeamRespawnTile == null || _grid == null) return;
            _grid.GetGridObject(rightTeamRespawnTile.transform.position)
                .SetRespawn(rightTeamRespawnTile);
            rightTeamRespawnTile.tag = "RightRespawn";
            rightTeamRespawnTile.transform.parent = _gridRespawnRightContainer.transform;
            _gridTileMovement.transform.localScale = new Vector3(14, 14, 10);
        }
    }

    public Grid<GridCombatSystem.GridObject> GetGrid() {
        return _grid;
    }
}
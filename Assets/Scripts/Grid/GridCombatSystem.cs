﻿using System;
using System.Collections.Generic;
using UnityEngine;


public class GridCombatSystem : MonoBehaviour {
    [SerializeField] public List<UnitCombatSystem> leftTeam, rightTeam;
    [SerializeField] private UnitStatsControllerUI _unitStatsControllerUI;
    public TeamsStateSO _teamsState;

    private UnitCombatSystem _unitCombatSystem;
    private State _state;
    private GameObject _gridTileBorder, _gridTileMovement, _gridTileAttackRange;
    private Transform _gridMovementContainer;
    private bool _canMoveThisTurn, _canAttackThisTurn;
    private int _lefTeamActiveUnitIndex, _rightTeamActiveUnitIndex;
    private int _cellSize = 17;
    private int _cellCenter;
    private enum State {
        Normal,
        Waiting
    }

    private void Awake() {
        _state = State.Normal;
        _gridMovementContainer = GameObject.Find("GridMovementContainer").transform;
        _cellCenter = _cellSize / 2;
    }
    

    public void SetupGame() {
        _unitStatsControllerUI.HideDraftPickPanels();

        _unitStatsControllerUI.HidePanelPlayerPanel(UnitCombatSystem.Team.Left, GameModeEnum.Game);
        leftTeam = _teamsState.leftTeam;
        rightTeam = _teamsState.rightTeam;
        _gridTileMovement = Resources.Load("Sprites/grid-move", typeof(GameObject)) as GameObject;
        _gridTileBorder = Resources.Load("Sprites/grid", typeof(GameObject)) as GameObject;
        _gridTileAttackRange = Resources.Load("Sprites/grid-attack", typeof(GameObject)) as GameObject;

        foreach (UnitCombatSystem unit in _teamsState.allUnitsInBothTeams) {
            CombatSystemUnitDebugLogger(unit);

            GameController_GridCombatSystem
                .Instance
                .GetGrid()
                .GetGridObject(unit.GetPosition())
                .SetUnitGridCombat(unit.GetComponent<UnitCombatSystem>());
        }

        _teamsState.areTeamsReady = true;
        GameController_GridCombatSystem.Instance.gridPathfinding.RaycastWalkable();
        SelectNextActiveUnit();
        UpdateValidMovePositionsAndAttackRange();
    }

    private UnitCombatSystem GetNextActiveUnit(UnitCombatSystem.Team team) {
        if (team == UnitCombatSystem.Team.Left) {
            _lefTeamActiveUnitIndex = (_lefTeamActiveUnitIndex + 1) % leftTeam.Count;
            _unitStatsControllerUI.HidePanelPlayerPanel(UnitCombatSystem.Team.Left, GameModeEnum.Game);
            _unitStatsControllerUI.ViewActiveUnitInGame(leftTeam[_lefTeamActiveUnitIndex].GetUnitStats(),
                UnitCombatSystem.Team.Left);

            if (leftTeam[_lefTeamActiveUnitIndex] == null) {
                return GetNextActiveUnit(team);
            }
            else {
                return leftTeam[_lefTeamActiveUnitIndex];
            }
        }
        else {
            _rightTeamActiveUnitIndex = (_rightTeamActiveUnitIndex + 1) % rightTeam.Count;
            _unitStatsControllerUI.HidePanelPlayerPanel(UnitCombatSystem.Team.Right, GameModeEnum.Game);
            _unitStatsControllerUI.ViewActiveUnitInGame(rightTeam[_rightTeamActiveUnitIndex].GetUnitStats(),
                UnitCombatSystem.Team.Right);

            if (rightTeam[_rightTeamActiveUnitIndex] == null) {
                return GetNextActiveUnit(team);
            }
            else {
                return rightTeam[_rightTeamActiveUnitIndex];
            }
        }
    }

    private void Update() {
        if (!_teamsState.areTeamsReady) return;
        switch (_state) {
            case State.Normal:
                if (Input.GetKeyDown(KeyCode.Space)) {
                    Debug.Log("Force turn over.");
                    ForceTurnOver();
                }

                if (Input.GetMouseButtonDown(0)) {
                    Grid<GridObject> grid = GameController_GridCombatSystem.Instance.GetGrid();
                    GridObject gridObject = grid.GetGridObject(CursorUtils.GetMouseWorldPosition());
                    // Check if clicking on a unit position
                    if (gridObject == null) return;
                    if (gridObject.GetUnitGridCombat() != null) {
                        // Clicked on top of a Unit
                        if (_unitCombatSystem.IsEnemy(gridObject.GetUnitGridCombat())) {
                            if (_unitCombatSystem.CanMeleeAttack(gridObject.GetUnitGridCombat()) &&
                                _unitCombatSystem.unitStats.unitType == UnitTypeEnum.Melee) {
                                if (_canAttackThisTurn) {
                                    _canAttackThisTurn = false;
                                    _state = State.Normal;
                                    _unitCombatSystem.AttackUnit(gridObject.GetUnitGridCombat(), () => {
                                        _state = State.Normal;
                                        ForceTurnOver();
                                    });
                                }
                            }
                            else if (_unitCombatSystem.CanDistanceAttack(gridObject.GetUnitGridCombat()) &&
                                     _unitCombatSystem.unitStats.unitType == UnitTypeEnum.Distance) {
                                if (_canAttackThisTurn) {
                                    _canAttackThisTurn = false;
                                    _state = State.Normal;
                                    _unitCombatSystem.AttackUnit(gridObject.GetUnitGridCombat(), () => {
                                        _state = State.Normal;
                                        ForceTurnOver();
                                    });
                                }
                            }

                            break;
                        }
                        else {
                            // Not an enemy
                        }
                    }
                    else {
                        // No unit here
                    }

                    if (gridObject.GetIsValidMovePosition()) {
                        if (_canMoveThisTurn) {
                            _canMoveThisTurn = false;
                            grid.GetGridObject(_unitCombatSystem.GetPosition()).ClearUnitGridCombat();
                            gridObject.SetUnitGridCombat(_unitCombatSystem);

                            ClearMovementGridVisualization();
                            _unitCombatSystem.MoveTo(CursorUtils.GetMouseWorldPosition(), () => {
                                UpdateValidMovePositionsAndAttackRange(true);
                            });
                        }
                    }
                }

                break;

            case State.Waiting:
                if (Input.GetKeyDown(KeyCode.Space)) {
                    Debug.Log("Force turn over.");
                    ForceTurnOver();
                }

                break;
        }
    }

    private void TestTurnOver() {
        if (!_canMoveThisTurn || !_canAttackThisTurn) {
            ClearMovementGridVisualization();
            Debug.Log($"Unit: {gameObject.name} cannot move or attack");
        }
    }

    private void ClearMovementGridVisualization() {
        foreach (Transform child in _gridMovementContainer.transform) {
            Destroy(child.gameObject);
        }
    }

    private void ForceTurnOver() {
        _unitCombatSystem.SetInactive();
        ClearMovementGridVisualization();
        SelectNextActiveUnit();
        UpdateValidMovePositionsAndAttackRange();
    }

    private void UpdateValidMovePositionsAndAttackRange(bool showOnlyAttackRange = false) {
        Grid<GridObject> grid = GameController_GridCombatSystem.Instance.GetGrid();
        GridPathfinding gridPathfinding = GameController_GridCombatSystem.Instance.gridPathfinding;

        // Get Unit Grid Position X, Y
        grid.GetXY(_unitCombatSystem.GetPosition(), out int unitX, out int unitY);

        // Reset Entire Grid ValidMovePositions
        for (int x = 0; x < grid.GetWidth(); x++) {
            for (int y = 0; y < grid.GetHeight(); y++) {
                grid.GetGridObject(x, y).SetIsValidMovePosition(false);
            }
        }

        int maxMoveDistance = _unitCombatSystem.unitStats.movementRange;
        int maxAttackRange = _unitCombatSystem.unitStats.attackRange;
        if (_unitCombatSystem.unitStats.unitType == UnitTypeEnum.Melee) {
            maxAttackRange = 1;
        }

        for (int x = unitX - maxMoveDistance; x <= unitX + maxMoveDistance; x++) {
            for (int y = unitY - maxMoveDistance; y <= unitY + maxMoveDistance; y++) {
                if (gridPathfinding.IsWalkable(x, y)) {
                    // Position is Walkable
                    if (gridPathfinding.HasPath(unitX, unitY, x, y)) {
                        // There is a Path

                        if (gridPathfinding.GetPath(unitX, unitY, x, y).Count <= maxMoveDistance && showOnlyAttackRange == false) {
                            // Path within Move Distance

                            var movementTileObject = Instantiate(
                                _gridTileMovement,
                                new Vector3(
                                    _cellCenter + (x * _cellSize), _cellCenter + (y * _cellSize)) +
                                new Vector3(1, 1) * 0.5f,
                                Quaternion.identity
                            );

                            movementTileObject.transform.parent = _gridMovementContainer.transform;
                            _gridTileMovement.transform.localScale = new Vector3(14, 14, 10);

                            grid.GetGridObject(x, y).SetIsValidMovePosition(true);
                        }

                        if (gridPathfinding.GetPath(unitX, unitY, x, y).Count <= maxAttackRange) {
                            RenderUnitRangeGrid(x, y);
                        }
                    }
                    else {
                        // No valid Path
                    }
                }
                else {
                    // Position is not Walkable
                }
            }
        }
    }

    private void RenderUnitRangeGrid(int x, int y) {
        var attackRangeTileGameObject = Instantiate(
            _gridTileAttackRange,
            new Vector3(
                _cellCenter + (x * _cellSize), _cellCenter + (y * _cellSize)) +
            new Vector3(1, 1) * 0.5f,
            Quaternion.identity
        );

        attackRangeTileGameObject.transform.parent = _gridMovementContainer.transform;
        attackRangeTileGameObject.transform.localScale = new Vector3(14, 14, 10);
    }
    
    private void SelectNextActiveUnit() {
        if (_unitCombatSystem == null || _unitCombatSystem.GetTeam() == UnitCombatSystem.Team.Right) {
            _unitCombatSystem = GetNextActiveUnit(UnitCombatSystem.Team.Left);
            Debug.Log($"Next unit is: {_unitCombatSystem}");
        }
        else {
            _unitCombatSystem = GetNextActiveUnit(UnitCombatSystem.Team.Right);
            Debug.Log($"Next unit is: {_unitCombatSystem}");
        }

        _unitCombatSystem.SetActive();
        _canMoveThisTurn = true;
        _canAttackThisTurn = true;
    }

    private void ShootUnit(UnitCombatSystem unitGridCombat, Action onShootComplete) {
        GetComponent<IMoveVelocity>().Disable();
        Debug.Log($"Shoot unit");
    }

    private void CombatSystemUnitDebugLogger(UnitCombatSystem unit) {
        if (unit == null) return;
        Debug.Log(
            $"Unit name: {unit.name}, team: {unit.GetTeam()}, POSITION: X:{unit.transform.position.x} Y:{unit.transform.position.y} ");
    }

    public class GridObject {
        private Grid<GridObject> _grid;
        private UnitCombatSystem _unitCombatSystem;
        private GameObject _respawn;

        private int _x, _y;
        private bool _isValidMovePosition;

        public GridObject(Grid<GridObject> grid, int x, int y) {
            _grid = grid;
            _x = x;
            _y = y;
        }

        public void SetIsValidMovePosition(bool set) {
            _isValidMovePosition = set;
        }

        public bool GetIsValidMovePosition() => _isValidMovePosition;

        public void ClearUnitGridCombat() {
            SetUnitGridCombat(null);
        }

        public UnitCombatSystem GetUnitGridCombat() {
            return _unitCombatSystem;
        }

        public void SetUnitGridCombat(UnitCombatSystem unit) {
            _unitCombatSystem = unit;
        }

        public GameObject GetRespawn() {
            return _respawn;
        }

        public void SetRespawn(GameObject respawn) {
            _respawn = respawn;
        }
    }
}
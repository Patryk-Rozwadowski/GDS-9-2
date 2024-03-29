﻿using System;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class GridCombatSystem : MonoBehaviour {
    [SerializeField] public List<UnitCombatSystem> leftTeam, rightTeam;
    [SerializeField] private UnitStatsControllerUI _unitStatsControllerUI;
    public TeamsStateSO _teamsState;
    private Transform _gridMovementContainer;
    private State _state;
    private GameObject _gridTileBorder, _gridTileMovement, _gridTileAttackRange;
    private bool _canMoveThisTurn, _canAttackThisTurn;
    private int _cellCenter;
    private int _lefTeamActiveUnitIndex, _rightTeamActiveUnitIndex;
    private readonly int _cellSize = 17;

    private UnitCombatSystem _unitCombatSystem;

    private void Awake() {
        _state = State.Normal;
        _gridMovementContainer = GameObject.Find("GridMovementContainer").transform;
        _cellCenter = _cellSize / 2;
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
                    var grid = GameController_GridCombatSystem.Instance.GetGrid();
                    var gridObject = grid.GetGridObject(CursorUtils.GetMouseWorldPosition());
                    // Check if clicking on a unit position
                    if (gridObject == null) return;
                    if (gridObject.GetUnitGridCombat() != null) // Clicked on top of a Unit
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

                            if (_unitCombatSystem.CanDistanceAttack(gridObject.GetUnitGridCombat()) &&
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

                    if (gridObject.GetIsValidMovePosition())
                        if (_canMoveThisTurn) {
                            _canMoveThisTurn = false;
                            grid.GetGridObject(_unitCombatSystem.GetPosition()).ClearUnitGridCombat();
                            gridObject.SetUnitGridCombat(_unitCombatSystem);

                            ClearMovementGridVisualization();
                            _state = State.Waiting;
                            _unitCombatSystem.MoveTo(CursorUtils.GetMouseWorldPosition(),
                                () => {
                                    UpdateValidMovePositionsAndAttackRange(true);
                                    _state = State.Normal;
                                });
                        }
                }

                break;
        }
    }


    public void SetupGame() {
        _unitStatsControllerUI.HideDraftPickPanels();

        _unitStatsControllerUI.HidePanelPlayerPanel(UnitCombatSystem.Team.Left, GameModeEnum.Game);
        leftTeam = _teamsState.leftTeam;
        rightTeam = _teamsState.rightTeam;
        _gridTileMovement = Resources.Load("Sprites/grid-move", typeof(GameObject)) as GameObject;
        _gridTileBorder = Resources.Load("Sprites/grid", typeof(GameObject)) as GameObject;
        _gridTileAttackRange = Resources.Load("Sprites/grid-attack", typeof(GameObject)) as GameObject;

        foreach (var unit in _teamsState.allUnitsInBothTeams) {
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

            if (leftTeam.Count(x => x != null) == 0) {
                WinController.Win(false);
            }

            if (leftTeam[_lefTeamActiveUnitIndex] == null || leftTeam[_lefTeamActiveUnitIndex].IsDead()) {
                return GetNextActiveUnit(team);
            }

            if (leftTeam[_lefTeamActiveUnitIndex] == null)
                return GetNextActiveUnit(team);
            return leftTeam[_lefTeamActiveUnitIndex];
        }

        if (rightTeam.Count(x => x != null) == 0) {
            WinController.Win(true);
        }

        _rightTeamActiveUnitIndex = (_rightTeamActiveUnitIndex + 1) % rightTeam.Count;
        _unitStatsControllerUI.HidePanelPlayerPanel(UnitCombatSystem.Team.Right, GameModeEnum.Game);
        _unitStatsControllerUI.ViewActiveUnitInGame(rightTeam[_rightTeamActiveUnitIndex].GetUnitStats(),
            UnitCombatSystem.Team.Right);

        if (rightTeam[_rightTeamActiveUnitIndex] == null || rightTeam[_rightTeamActiveUnitIndex].IsDead())
            return GetNextActiveUnit(team);
        return rightTeam[_rightTeamActiveUnitIndex];
    }

    private void TestTurnOver() {
        if (!_canMoveThisTurn || !_canAttackThisTurn) {
            ClearMovementGridVisualization();
            Debug.Log($"Unit: {gameObject.name} cannot move or attack");
        }
    }

    private void ClearMovementGridVisualization() {
        foreach (Transform child in _gridMovementContainer.transform) Destroy(child.gameObject);
    }

    private void ForceTurnOver() {
        _unitCombatSystem.SetInactive();
        ClearMovementGridVisualization();
        SelectNextActiveUnit();
        UpdateValidMovePositionsAndAttackRange();
    }

    private void UpdateValidMovePositionsAndAttackRange(bool showOnlyAttackRange = false) {
        var grid = GameController_GridCombatSystem.Instance.GetGrid();
        var gridPathfinding = GameController_GridCombatSystem.Instance.gridPathfinding;

        grid.GetXY(_unitCombatSystem.GetPosition(), out var unitX, out var unitY);

        for (var x = 1; x < grid.GetWidth(); x++)
        for (var y = 1; y < grid.GetHeight(); y++)
            grid.GetGridObject(x, y).SetIsValidMovePosition(false);

        var maxMoveDistance = _unitCombatSystem.unitStats.movementRange;
        var maxAttackRange = _unitCombatSystem.unitStats.attackRange;

        for (var x = unitX - maxMoveDistance; x <= unitX + maxMoveDistance; x++)
        for (var y = unitY - maxMoveDistance; y <= unitY + maxMoveDistance; y++)
            if (gridPathfinding.IsWalkable(x, y)) // Position is Walkable
                if (gridPathfinding.HasPath(unitX, unitY, x, y)) {
                    // There is a Path

                    if (gridPathfinding.GetPath(unitX, unitY, x, y).Count <= maxMoveDistance &&
                        showOnlyAttackRange == false) {
                        // Path within Move Distance

                        var movementTileObject = Instantiate(
                            _gridTileMovement,
                            new Vector3(
                                _cellCenter + x * _cellSize, _cellCenter + y * _cellSize) +
                            new Vector3(1, 1) * 0.5f,
                            Quaternion.identity
                        );

                        movementTileObject.transform.parent = _gridMovementContainer.transform;
                        _gridTileMovement.transform.localScale = new Vector3(14, 14, 10);

                        grid.GetGridObject(x, y).SetIsValidMovePosition(true);
                    }
                }


        for (var x = unitX - maxAttackRange; x <= unitX + maxAttackRange; x++)
        for (var y = unitY - maxAttackRange; y <= unitY + maxAttackRange; y++)
            if (gridPathfinding.IsWalkable(x, y)) {
                if (gridPathfinding.HasPath(unitX, unitY, x, y)) {
                    if (maxAttackRange == 2 || maxAttackRange == 1) {
                        if (gridPathfinding.GetPath(unitX, unitY, x, y).Count - 1 <= maxAttackRange)
                            RenderUnitRangeGrid(x, y);
                    }

                    if (gridPathfinding.GetPath(unitX, unitY, x, y).Count <= maxAttackRange)
                        RenderUnitRangeGrid(x, y);
                }
            }
    }

    private void RenderUnitRangeGrid(int x, int y) {
        var attackRangeTileGameObject = Instantiate(
            _gridTileAttackRange,
            new Vector3(
                _cellCenter + x * _cellSize, _cellCenter + y * _cellSize) +
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

    private void CombatSystemUnitDebugLogger(UnitCombatSystem unit) {
        if (unit == null) return;
        Debug.Log(
            $"Unit name: {unit.name}, team: {unit.GetTeam()}, POSITION: X:{unit.transform.position.x} Y:{unit.transform.position.y} ");
    }

    private enum State {
        Normal,
        Waiting
    }

    public class GridObject {
        private Grid<GridObject> _grid;
        private bool _isValidMovePosition;
        private GameObject _respawn;
        private UnitCombatSystem _unitCombatSystem;

        private int _x, _y;

        public GridObject(Grid<GridObject> grid, int x, int y) {
            _grid = grid;
            _x = x;
            _y = y;
        }

        public void SetIsValidMovePosition(bool set) {
            _isValidMovePosition = set;
        }

        public bool GetIsValidMovePosition() {
            return _isValidMovePosition;
        }

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
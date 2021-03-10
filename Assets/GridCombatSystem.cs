using System;
using System.Collections.Generic;
using UnityEngine;

public class GridCombatSystem : MonoBehaviour {
    [SerializeField] private UnitCombatSystem[] unitCombatSystemsArray;
    
    private UnitCombatSystem _unitCombatSystem;
    private int _lefTeamActiveUnitIndex, _rightTeamActiveUnitIndex;
    private List<UnitCombatSystem> _leftTeam, _rightTeam;
    private State _state;
    private bool _canMoveThisTurn, _canAttackThisTurn;
    
    private enum State {
        Normal,
        Waiting
    }

    private void Awake() {
        _state = State.Normal;
    }

    void Start() {
        _leftTeam = new List<UnitCombatSystem>();
        _rightTeam = new List<UnitCombatSystem>();

        foreach (var unit in unitCombatSystemsArray) {
            CombatSystemUnitDebugLogger(unit);
            GameController_GridCombatSystem.Instance.GetGrid().GetGridObject(unit.GetPosition()).SetUnitGridCombat(unit);
            if (unit.GetTeam() == UnitCombatSystem.Team.Left) {
                _leftTeam.Add(unit);
            }
            else {
                _rightTeam.Add(unit);
            }
        }
    }
    
    public void Damage(GridCombatSystem attacker, int damageAmount) {
        // TODO Hp damgage
        Debug.Log($"Damage done: {damageAmount}");
    }
    
    // private void SelectNextActiveUnit() {
    //     if (_unitCombatSystem == null || _unitCombatSystem.GetTeam() == _unitCombatSystem.Team.Right) {
    //         _unitCombatSystem = GetNextActiveUnit(_unitCombatSystem.Team.Left);
    //     } else {
    //         _unitCombatSystem = GetNextActiveUnit(_unitCombatSystem.Team.Right);
    //     }
    //     _canMoveThisTurn = true;
    //     _canAttackThisTurn = true;
    // }

    private UnitCombatSystem GetNextActiveUnit(UnitCombatSystem.Team team) {
        if (team == UnitCombatSystem.Team.Left) {
            _lefTeamActiveUnitIndex = (_lefTeamActiveUnitIndex + 1) % _leftTeam.Count;
            
            // TODO Hp system
            // if (_leftTeam[_lefTeamActiveUnitIndex] == null || _leftTeam[_lefTeamActiveUnitIndex].IsDead()) {
            //     // Unit is Dead, get next one
            //     return GetNextActiveUnit(team);
            // } else {
            //     return _leftTeam[_lefTeamActiveUnitIndex];
            // }
            return _leftTeam[_lefTeamActiveUnitIndex];
        } else {
            _rightTeamActiveUnitIndex = (_rightTeamActiveUnitIndex + 1) % _rightTeam.Count;
            
            // TODO hp system
            // if (_rightTeam[_rightTeamActiveUnitIndex] == null || _rightTeam[_rightTeamActiveUnitIndex].IsDead()) {
            //     // Unit is Dead, get next one
            //     return GetNextActiveUnit(team);
            // } else {
            //     return _rightTeam[_rightTeamActiveUnitIndex];
            // }
            return _rightTeam[_rightTeamActiveUnitIndex];
        }
    }

    private void Update() {
        switch (_state) {
            case State.Normal:
                if (Input.GetMouseButtonDown(0)) {
                    Grid<GridObject> grid = GameController_GridCombatSystem.Instance.GetGrid();
                    GridObject gridObject = grid.GetGridObject(CursorUtils.GetMouseWorldPosition());

                    // Check if clicking on a unit position
                    if (gridObject.GetUnitGridCombat() != null) {
                        // Clicked on top of a Unit
                        if (_unitCombatSystem.IsEnemy(gridObject.GetUnitGridCombat())) {
                            // Clicked on an Enemy of the current unit
                            if (_unitCombatSystem.CanAttackUnit(gridObject.GetUnitGridCombat())) {
                                // Can Attack Enemy
                                if (_canAttackThisTurn) {
                                    _canAttackThisTurn = false;
                                    // Attack Enemy
                                    _state = State.Waiting;
                                    _unitCombatSystem.AttackUnit(gridObject.GetUnitGridCombat(), () => {
                                        _state = State.Normal;
                                        TestTurnOver();
                                    });
                                }
                            } else {
                                // Cannot attack enemy
                                Debug.Log("CANNOT ATTACK");
                            }
                            break;
                        } else {
                            // Not an enemy
                        }
                    } else {
                        // No unit here
                    }

                    if (gridObject.GetIsValidMovePosition()) {
                        // Valid Move Position

                        if (_canMoveThisTurn) {
                            _canMoveThisTurn = false;

                            _state = State.Waiting;

                            // Remove Unit from current Grid Object
                            grid.GetGridObject(_unitCombatSystem.GetPosition()).ClearUnitGridCombat();
                            // Set Unit on target Grid Object
                            gridObject.SetUnitGridCombat(_unitCombatSystem);

                            _unitCombatSystem.MoveTo(CursorUtils.GetMouseWorldPosition(), () => {
                                _state = State.Normal;
                                UpdateValidMovePositions();
                                TestTurnOver();
                            });
                        }
                    }
                }

                if (Input.GetKeyDown(KeyCode.Space)) {
                    ForceTurnOver();
                }
                break;
            case State.Waiting:
                break;
        }
    }
    
    private void TestTurnOver() {
        if (!_canMoveThisTurn && !_canAttackThisTurn) {
            // Cannot move or attack, turn over
            ForceTurnOver();
        }
    }
    
    private void ForceTurnOver() {
        SelectNextActiveUnit();
        UpdateValidMovePositions();
    }
    
       private void UpdateValidMovePositions() {
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

        int maxMoveDistance = 5;
        for (int x = unitX - maxMoveDistance; x <= unitX + maxMoveDistance; x++) {
            for (int y = unitY - maxMoveDistance; y <= unitY + maxMoveDistance; y++) {
                if (gridPathfinding.IsWalkable(x, y)) {
                    // Position is Walkable
                    if (gridPathfinding.HasPath(unitX, unitY, x, y)) {
                        // There is a Path
                        if (gridPathfinding.GetPath(unitX, unitY, x, y).Count <= maxMoveDistance) {
                            // Path within Move Distance
                            grid.GetGridObject(x, y).SetIsValidMovePosition(true);
                        } else { 
                            // Path outside Move Distance!
                        }
                    } else {
                        // No valid Path
                    }
                } else {
                    // Position is not Walkable
                }
            }
        }
    }
    
    private void SelectNextActiveUnit() {
        if (_unitCombatSystem == null || _unitCombatSystem.GetTeam() == UnitCombatSystem.Team.Right) {
            _unitCombatSystem = GetNextActiveUnit(UnitCombatSystem.Team.Left);
        } else {
            _unitCombatSystem = GetNextActiveUnit(UnitCombatSystem.Team.Right);
        }
        _canMoveThisTurn = true;
        _canAttackThisTurn = true;
    }
    
    private void ShootUnit(UnitCombatSystem unitGridCombat, Action onShootComplete) {
        GetComponent<IMoveVelocity>().Disable();
        Debug.Log($"Shoot unit");
    }
    
    private void CombatSystemUnitDebugLogger(UnitCombatSystem unit) {
        Debug.Log($"Unit name: {unit.name}, team: {unit.GetTeam()}");
    }
    
    public class GridObject {
        private Grid<GridObject> _grid;
        private UnitCombatSystem _unitCombatSystem;
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
        public UnitCombatSystem GetUnitGridCombat() => _unitCombatSystem;
        public void SetUnitGridCombat(UnitCombatSystem unit) => _unitCombatSystem = unit;
    }
}
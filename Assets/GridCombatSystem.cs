using System.Collections.Generic;
using UnityEngine;

public class GridCombatSystem : MonoBehaviour {
    [SerializeField] private UnitCombatSystem[] unitCombatSystemsArray;
    
    private UnitCombatSystem _unitCombatSystem;
    private List<UnitCombatSystem> _leftTeam, _rightTeam;
    
    void Start() {
        _leftTeam = new List<UnitCombatSystem>();
        _rightTeam = new List<UnitCombatSystem>();

        foreach (var unit in unitCombatSystemsArray) {
            CombatSystemUnitDebugLogger(unit);

            if (unit.GetTeam() == Team.Left) {
                _leftTeam.Add(unit);
            }
            else {
                _rightTeam.Add(unit);
            }
        }
    }
    // Update is called once per frame
    void Update() {
    }

    private void CombatSystemUnitDebugLogger(UnitCombatSystem unit) {
        Debug.Log($"Unit name: {unit.name}, team: {unit.GetTeam()}");
    }
    
    public class GridObject {
        private Grid<GridObject> _grid;
        private UnitCombatSystem _unitGridCombat;
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

        public bool GetIsValidMovePosition() {
            return _isValidMovePosition;
        }

        public void SetUnitGridCombat(UnitCombatSystem unitGridCombat) {
            _unitGridCombat = unitGridCombat;
        }

        public void ClearUnitGridCombat() {
            SetUnitGridCombat(null);
        }

        public UnitCombatSystem GetUnitGridCombat() {
            return _unitGridCombat;
        }

    }
}
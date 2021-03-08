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
}
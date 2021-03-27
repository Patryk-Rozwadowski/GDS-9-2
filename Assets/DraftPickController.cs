using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using Debug = UnityEngine.Debug;

public class DraftPickController : MonoBehaviour {
    [SerializeField] private UnitStatsControllerUI _unitStatsControllerUI;
    [SerializeField] private GameObject _leftTeamPanel, _rightTeamPanel;

    public enum Team {
        Left,
        Right
    }

    private bool
        _pickedPosition,
        _leftPickedUnit,
        _rightPicked,
        _leftPutUnitOnMap;

    private int _numberOfUnitsInTeams;
    private UnitCombatSystem _pickedUnit;
    private Team _teamPicking, _nextTeamPicking;
    private GridCombatSystem _gridCombatSystem;
    private Vector3 _unitScale;
    
    private void Awake() {
        _gridCombatSystem = GameObject.Find("GridCombatSystem").GetComponentInChildren<GridCombatSystem>();
    }

    void Start() {
        _rightTeamPanel.SetActive(false);
        _pickedUnit = null;
        _numberOfUnitsInTeams = 5;
        _unitScale = new Vector3(5,5,1);
        
        _leftPickedUnit = false;
        _teamPicking = Team.Left;
        
        Debug.LogWarning($"START INIT: TEAM PICKING : {_teamPicking}");
    }

    public void PickUnit(GameObject element) {
        var elementCombatSystem = element.GetComponent<UnitCombatSystem>();
        _pickedPosition = false;
        _unitStatsControllerUI.ViewUnitStats(elementCombatSystem.GetUnitStats(),
            _teamPicking);
        _pickedUnit = element.GetComponent<UnitCombatSystem>();
    }
    
    private void Update() {
        if (!Input.GetMouseButtonDown(0) || _pickedUnit == null) return;
        Grid<GridCombatSystem.GridObject> grid = GameController_GridCombatSystem.Instance.GetGrid();
        GridCombatSystem.GridObject gridObject = grid.GetGridObject(CursorUtils.GetMouseWorldPosition());

        if (_pickedUnit == null || gridObject == null) return;
        _leftPickedUnit = !_leftPickedUnit;
        Debug.Log($"LEFT PICKED UNIT: {_leftPickedUnit}");
        if (_teamPicking == Team.Left && _leftPickedUnit == false) {
            if (_gridCombatSystem.leftTeam.Count > 5) {
                Debug.Log($"{_gridCombatSystem.leftTeam} is FULL");
                return;
            }

            // TURN PICK, PICK INCREMENT
            
            Debug.LogWarning($"UPDATE: TEAM PICKING (LEFT CONTEXT): {_teamPicking}");
            _teamPicking = Team.Right;
            
            _gridCombatSystem.leftTeam.Add(_pickedUnit.GetComponent<UnitCombatSystem>());
            var pickedUnit = Instantiate(_pickedUnit, CursorUtils.GetMouseWorldPosition(), Quaternion.identity);
            pickedUnit.transform.localScale = _unitScale;
            pickedUnit.transform.position = GridUtils.SetUnitOnTileCenter(pickedUnit.gameObject);
            _pickedUnit = null;
            _leftTeamPanel.SetActive(true);
            _rightTeamPanel.SetActive(false);
        }
        else {
            if (_gridCombatSystem.rightTeam.Count > 5) {
                Debug.Log($"{_gridCombatSystem.rightTeam} is FULL");
                return;
            }

            Debug.LogWarning($"UPDATE: TEAM PICKING (RIGHT CONTEXT): {_teamPicking}");
            _gridCombatSystem.rightTeam.Add(_pickedUnit.GetComponent<UnitCombatSystem>());
            _teamPicking = Team.Left;
            
            var pickedUnit = Instantiate(_pickedUnit, CursorUtils.GetMouseWorldPosition(), Quaternion.identity);
            pickedUnit.transform.localScale = _unitScale;
            pickedUnit.transform.position = GridUtils.SetUnitOnTileCenter(pickedUnit.gameObject);
            _pickedUnit = null;
            _leftTeamPanel.SetActive(false);
            _rightTeamPanel.SetActive(true);
        }
    }
}
﻿using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class DraftPickController : MonoBehaviour {
    [SerializeField] private GameObject _leftTeamPanel, _rightTeamPanel;

    private enum Team {
        Left,
        Right
    }

    private bool
        _pickedPosition,
        _leftPickedUnit,
        _rightPicked,
        _leftPutUnitOnMap;

    private int _numberOfUnitsInTeams;
    private GameObject _pickedUnit;
    private Team _teamPicking, _nextTeamPicking;
    private GridCombatSystem _gridCombatSystem;

    private void Awake() {
        _gridCombatSystem = GameObject.Find("GridCombatSystem").GetComponentInChildren<GridCombatSystem>();
    }

    void Start() {
        
        _teamPicking = Team.Left;

        _leftPickedUnit = false;
        _pickedUnit = null;
        _numberOfUnitsInTeams = 5;
        _rightTeamPanel.SetActive(false);
    }

    public void PickUnit(GameObject element) {
        var elementCombatSystem = element.GetComponent<UnitCombatSystem>();
        var pickedElement = elementCombatSystem.GetTeam();

        Debug.Log($"PICKED ELEMENT TEAM: {pickedElement}");
        Debug.Log($"TEAM PICKING: {_teamPicking}");
        _pickedPosition = false;

        if ((Team) pickedElement == _teamPicking && _teamPicking == Team.Left) {
            if (_gridCombatSystem.leftTeam.Count > 5) {
                Debug.Log($"{_gridCombatSystem.leftTeam} is FULL");
                return;
            }

            _gridCombatSystem.leftTeam.Add(elementCombatSystem);
            _pickedUnit = element;
            _teamPicking = Team.Right;
            return;
        }

        if ((Team) pickedElement == _teamPicking && _teamPicking == Team.Right) {
            if (_gridCombatSystem.rightTeam.Count > 5) {
                Debug.Log($"{_gridCombatSystem.rightTeam} is FULL");
                return;
            }

            _gridCombatSystem.rightTeam.Add(elementCombatSystem);
            _pickedUnit = element;
            _teamPicking = Team.Left;
        }
    }

    void OnMouseDown() {
        Debug.Log(this.gameObject.name);
    }

    private void Update() {
        if (!Input.GetMouseButtonDown(0) || _pickedUnit == null) return;
        Grid<GridCombatSystem.GridObject> grid = GameController_GridCombatSystem.Instance.GetGrid();
        GridCombatSystem.GridObject gridObject = grid.GetGridObject(CursorUtils.GetMouseWorldPosition());
        
        if (gridObject == null) return;
        var pickedUnit = Instantiate(_pickedUnit, CursorUtils.GetMouseWorldPosition(), Quaternion.identity);
        pickedUnit.transform.localScale = new Vector3(5, 5, 1);
        pickedUnit.transform.position = GridUtils.SetUnitOnTileCenter(pickedUnit);
        _pickedUnit = null;

        if (_teamPicking == Team.Left) {
         
            _leftTeamPanel.SetActive(true);
            _rightTeamPanel.SetActive(false);
            return;
        }

        if (_teamPicking == Team.Right) {
            _leftTeamPanel.SetActive(false);
            _rightTeamPanel.SetActive(true);

        }
    }
}
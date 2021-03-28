﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
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
    private Grid<GridCombatSystem.GridObject> _grid;

    private int _draftPickPoint;

    private void Awake() {
        _gridCombatSystem = GameObject.Find("GridCombatSystem").GetComponentInChildren<GridCombatSystem>();
    }

    void Start() {
        _grid = GameController_GridCombatSystem.Instance.GetGrid();
        _rightTeamPanel.SetActive(false);
        _pickedUnit = null;
        _numberOfUnitsInTeams = 5;
        _draftPickPoint = 0;
        _unitScale = new Vector3(5, 5, 1);
        Debug.LogWarning($"START INIT: TEAM PICKING : {_teamPicking}");
    }

    public void PickUnit(GameObject element) {
        var elementCombatSystem = element.GetComponent<UnitCombatSystem>();
        _pickedPosition = false;
        _unitStatsControllerUI.ViewUnitStats(elementCombatSystem.GetUnitStats(),
            _teamPicking);
        _pickedUnit = element.GetComponent<UnitCombatSystem>();
    }

    private void OnMouseDown() {
        Debug.Log(gameObject.name);
    }

    private void HidePanel() {
        if (_teamPicking == Team.Left) {
            _leftTeamPanel.SetActive(false);
            _rightTeamPanel.SetActive(true);
        }
        else {
            _leftTeamPanel.SetActive(true);
            _rightTeamPanel.SetActive(false);
        }
    }

    private void PlaceUnitOnMap() {
        var pickedUnit = Instantiate(_pickedUnit, CursorUtils.GetMouseWorldPosition(), Quaternion.identity);
        pickedUnit.transform.localScale = _unitScale;
        pickedUnit.transform.position = GridUtils.SetUnitOnTileCenter(pickedUnit.gameObject);
        _pickedUnit = null;
    }

    private void Update() {
        if (Input.GetMouseButtonDown(0)) {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit)) {
                Debug.Log($"RAYCAST HIT: {hit.transform.name}");
            }
        }

        if (!Input.GetMouseButtonDown(0) || _pickedUnit == null) return;
        var gridObject = _grid.GetGridObject(CursorUtils.GetMouseWorldPosition());
        if (_pickedUnit == null || gridObject == null) return;

        if (_teamPicking == Team.Left) {
            _gridCombatSystem.leftTeam.Add(_pickedUnit.GetComponent<UnitCombatSystem>());
            PlaceUnitOnMap();
            _draftPickPoint++;

            if (
                _draftPickPoint == 1 ||
                _draftPickPoint == 5 ||
                _draftPickPoint == 9
            ) {
                HidePanel();
                _teamPicking = Team.Right;
                Debug.Log($"DRAFT PICK POINT: {_draftPickPoint} NEXT PICK: {_teamPicking}");
                return;
            }
        }

        if (_teamPicking == Team.Right) {
            _gridCombatSystem.rightTeam.Add(_pickedUnit.GetComponent<UnitCombatSystem>());
            PlaceUnitOnMap();
            if (_gridCombatSystem.rightTeam.Count > 4) {
                Debug.Log($"{_gridCombatSystem.rightTeam} is FULL");
                int nextSceneIndex = SceneManager.GetActiveScene().buildIndex + 1;
                SceneManager.LoadScene(nextSceneIndex);
                return;
            }


            _draftPickPoint++;
            if (_draftPickPoint == 3 || _draftPickPoint == 7) {
                HidePanel();
                _teamPicking = Team.Left;
                Debug.Log($"DRAFT PICK POINT: {_draftPickPoint} NEXT PICK: {_teamPicking}");
            }
        }
    }
}
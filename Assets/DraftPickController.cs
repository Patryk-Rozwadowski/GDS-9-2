using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class DraftPickController : MonoBehaviour {
    [SerializeField] private GameObject _leftTeamPanel, _rightTeamPanel;
    public List<GameObject> _leftTeam, _rightTeam;

    private enum Team {
        Left,
        Right
    }

    private bool
        _pickedPosition,
        _leftPickedUnit,
        _rightPickedUnit,
        _rightPicked,
        _leftPutUnitOnMap,
        _rightPutUnitOnMap;

    private int _numberOfUnitsInTeams;
    private GameObject _pickedUnit;
    private Team _teamPicking, _nextTeamPicking;

    void Start() {
        // _rightTeamPanel.SetActive(false);
        _leftTeam = new List<GameObject>();
        _rightTeam = new List<GameObject>();

        _teamPicking = Team.Left;

        _leftPickedUnit = false;
        _rightPickedUnit = false;

        _leftPutUnitOnMap = false;
        _rightPutUnitOnMap = false;

        _pickedUnit = null;
        _numberOfUnitsInTeams = 5;
    }

    public void PickUnit(GameObject element) {
        var pickedElement = element.GetComponent<UnitCombatSystem>().GetTeam();
        Debug.Log($"PICKED ELEMENT TEAM: {pickedElement}");
        Debug.Log($"TEAM PICKING: {_teamPicking}");
        _pickedPosition = false;
        if ((Team) pickedElement == _teamPicking && _teamPicking == Team.Left) {
            if (_leftTeam.Count > 5) {
                Debug.Log($"{_rightTeam} is FULL");
                return;
            }

            _leftTeam.Add(element);
            _pickedUnit = element;
            _teamPicking = Team.Right;
            Debug.Log($"Left picked: {_leftPickedUnit}");
            return;
        }

        if ((Team) pickedElement == _teamPicking && _teamPicking == Team.Right) {
            if (_rightTeam.Count > 5) {
                Debug.Log($"{_rightTeam} is FULL");
                return;
            }

            _rightTeam.Add(element);
            _pickedUnit = element;
            Debug.Log($"Right picked: {_rightTeam}");
            _teamPicking = Team.Left;
        }
    }

    void OnMouseDown() {
        Debug.Log(this.gameObject.name);
    }

    private void Update() {
        if (Input.GetMouseButtonDown(0) && _pickedUnit != null) {
            if (_pickedPosition == false) {
                var el = Instantiate(_pickedUnit, CursorUtils.GetMouseWorldPosition(), Quaternion.identity);
                el.transform.localScale = new Vector3(5, 5, 1);

                _pickedUnit = null;
            }
        }
    }
}
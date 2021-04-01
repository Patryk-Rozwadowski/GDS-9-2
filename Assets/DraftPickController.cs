using UnityEngine;
using UnityEngine.EventSystems;
using Debug = UnityEngine.Debug;

public class DraftPickController : MonoBehaviour {
    [SerializeField] private UnitStatsControllerUI _unitStatsControllerUI;
    [SerializeField] private GameObject _leftTeamPanel, _rightTeamPanel;
    [SerializeField] private TeamsStateSO _teamsState;
    [SerializeField] private GameObject _leftTeamRespawn, _rightTeamRespawn;
    
    public enum Team {
        Left,
        Right
    }

    private bool
        _pickedPosition,
        _leftPickedUnit,
        _rightPicked,
        _leftPutUnitOnMap;

    private int _numberOfUnitsInTeam;
    private int _draftPickPoint;
    private UnitCombatSystem _pickedUnit;
    private Team _teamPicking, _nextTeamPicking;
    private GridCombatSystem _gridCombatSystem;
    private Vector3 _unitScale;
    private Grid<GridCombatSystem.GridObject> _grid;
    private GameObject _pickButtonSelected;

    
    private bool _debug = false;
    void Start() {
        _grid = GameController_GridCombatSystem.Instance.GetGrid();
        _pickedUnit = null;
        _numberOfUnitsInTeam = 5;
        _draftPickPoint = 0;
        _unitScale = new Vector3(5, 5, 1);
        _teamsState.areTeamsReady = false;
        
        _rightTeamRespawn.SetActive(false);
        _rightTeamPanel.SetActive(false);
        
        _teamsState.leftTeam.Clear();
        _teamsState.rightTeam.Clear();
        _teamsState.allUnitsInBothTeams.Clear();
        
        Debug.LogWarning($"START INIT: TEAM PICKING : {_teamPicking}");
        if (_debug) {
            _numberOfUnitsInTeam = 1;
        }
    }

    public void PickUnit(GameObject element) {
        var elementCombatSystem = element.GetComponent<UnitCombatSystem>();
        _pickedPosition = false;
        _pickButtonSelected = EventSystem.current.currentSelectedGameObject;
        _unitStatsControllerUI.ViewUnitStats(elementCombatSystem.GetUnitStats(),
            _teamPicking);
        _pickedUnit = element.GetComponent<UnitCombatSystem>();
    }


    private void HidePanel() {
        if (_teamPicking == Team.Left) {
            _leftTeamPanel.SetActive(false);
            _rightTeamPanel.SetActive(true);

            _leftTeamRespawn.SetActive(false);
            _rightTeamRespawn.SetActive(true);
        }
        else {
            _leftTeamPanel.SetActive(true);
            _rightTeamPanel.SetActive(false);

            _leftTeamRespawn.SetActive(true);
            _rightTeamRespawn.SetActive(false);
        }
    }

    private void PlaceUnitOnMap() {
        var pickedUnit = Instantiate(_pickedUnit, CursorUtils.GetMouseWorldPosition(), Quaternion.identity);
        pickedUnit.transform.localScale = _unitScale;
        pickedUnit.transform.position = GridUtils.SetUnitOnTileCenter(pickedUnit.gameObject);
        GameController_GridCombatSystem
            .Instance
            .GetGrid()
            .GetGridObject(pickedUnit.GetPosition())
            .SetUnitGridCombat(pickedUnit);
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
        if (_pickedUnit == null || gridObject == null || gridObject.GetRespawn() == null) return;

        // TODO nice to have removed duplicates
        if (_teamPicking == Team.Left) {
            if (gridObject.GetRespawn().CompareTag("RightRespawn")  ) return;
            var pickedUnit = Instantiate(_pickedUnit, CursorUtils.GetMouseWorldPosition(), Quaternion.identity);
            pickedUnit.transform.localScale = _unitScale;
            pickedUnit.transform.position = GridUtils.SetUnitOnTileCenter(pickedUnit.gameObject);

            _teamsState.leftTeam.Add(pickedUnit);
            _teamsState.allUnitsInBothTeams.Add(pickedUnit);

            _pickButtonSelected.SetActive(false);
            _pickedUnit = null;
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
            if (gridObject.GetRespawn().CompareTag("LeftRespawn")) return;
            var pickedUnit = Instantiate(_pickedUnit, CursorUtils.GetMouseWorldPosition(), Quaternion.identity);
            pickedUnit.transform.localScale = _unitScale;
            pickedUnit.transform.position = GridUtils.SetUnitOnTileCenter(pickedUnit.gameObject);

            _teamsState.rightTeam.Add(pickedUnit);
            _teamsState.allUnitsInBothTeams.Add(pickedUnit);
            
            _pickedUnit = null;
            _pickButtonSelected.SetActive(false);

            if (_teamsState.rightTeam.Count == _numberOfUnitsInTeam) {
                Debug.Log($"{_teamsState.rightTeam} is FULL");

                _rightTeamPanel.SetActive(false);
                _leftTeamRespawn.SetActive(false);
                _rightTeamRespawn.SetActive(false);
                var gcs = gameObject.AddComponent<GridCombatSystem>();
                gcs._teamsState = _teamsState;
                gcs.SetupGame();
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
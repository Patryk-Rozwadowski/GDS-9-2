using UnityEngine;
using UnityEngine.EventSystems;

public class DraftPickController : MonoBehaviour {
    [SerializeField] private UnitStatsControllerUI _unitStatsControllerUI;
    [SerializeField] private TeamsStateSO _teamsState;
    [SerializeField] private GameObject _leftTeamRespawn, _rightTeamRespawn;
    [SerializeField] private GridCombatSystem _gridCombatSystem;

    private readonly bool _debug = false;
    private int _draftPickPoint;
    private Grid<GridCombatSystem.GridObject> _grid;

    private int _numberOfUnitsInTeam;
    private GameObject _pickButtonSelected;

    private bool
        _pickedPosition,
        _leftPickedUnit,
        _rightPicked,
        _leftPutUnitOnMap;

    private UnitCombatSystem _pickedUnit;
    private UnitCombatSystem.Team _teamPicking, _nextTeamPicking;

    private Vector3 _unitScale;

    private void Start() {
        _grid = GameController_GridCombatSystem.Instance.GetGrid();
        _pickedUnit = null;
        _numberOfUnitsInTeam = 5;
        _draftPickPoint = 0;
        _unitScale = new Vector3(5, 5, 1);
        _teamsState.areTeamsReady = false;

        _rightTeamRespawn.SetActive(false);

        _teamsState.leftTeam.Clear();
        _teamsState.rightTeam.Clear();
        _teamsState.allUnitsInBothTeams.Clear();

        _unitStatsControllerUI.HideGamePanels();
        Debug.LogWarning($"START INIT: TEAM PICKING : {_teamPicking}");
        if (_debug) _numberOfUnitsInTeam = 1;
    }


    private void Update() {
        if (Input.GetMouseButtonDown(0)) {
            RaycastHit hit;
            var ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit)) Debug.Log($"RAYCAST HIT: {hit.transform.name}");
        }

        if (!Input.GetMouseButtonDown(0) || _pickedUnit == null) return;
        var gridObject = _grid.GetGridObject(CursorUtils.GetMouseWorldPosition());
        if (_pickedUnit == null || gridObject == null || gridObject.GetRespawn() == null) return;

        // TODO nice to have removed duplicates
        if (_teamPicking == UnitCombatSystem.Team.Left) {
            if (gridObject.GetRespawn().CompareTag("RightRespawn")) return;
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
                HideRespawn();
                _teamPicking = UnitCombatSystem.Team.Right;
                _unitStatsControllerUI.HidePanelPlayerPanel(_teamPicking, GameModeEnum.DraftPick);
                Debug.Log($"DRAFT PICK POINT: {_draftPickPoint} NEXT PICK: {_teamPicking}");
                return;
            }
        }

        if (_teamPicking == UnitCombatSystem.Team.Right) {
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

                _leftTeamRespawn.SetActive(false);
                _rightTeamRespawn.SetActive(false);
                _gridCombatSystem._teamsState = _teamsState;
                _gridCombatSystem.SetupGame();
                return;
            }

            _draftPickPoint++;
            if (_draftPickPoint == 3 || _draftPickPoint == 7) {
                HideRespawn();
                _unitStatsControllerUI.HidePanelPlayerPanel(_teamPicking, GameModeEnum.DraftPick);
                _unitStatsControllerUI.HidePanelPlayerPanel(UnitCombatSystem.Team.Right, GameModeEnum.DraftPick);
                _teamPicking = UnitCombatSystem.Team.Left;
                Debug.Log($"DRAFT PICK POINT: {_draftPickPoint} NEXT PICK: {_teamPicking}");
            }
        }
    }

    public void PickUnit(GameObject element) {
        var elementCombatSystem = element.GetComponent<UnitCombatSystem>();
        _pickedPosition = false;
        _pickButtonSelected = EventSystem.current.currentSelectedGameObject;
        _unitStatsControllerUI.ViewClickedUnitStatsDraftPick(elementCombatSystem.GetUnitStats(),
            _teamPicking);
        _pickedUnit = element.GetComponent<UnitCombatSystem>();
    }


    private void HideRespawn() {
        if (_teamPicking == UnitCombatSystem.Team.Left) {
            _leftTeamRespawn.SetActive(false);
            _rightTeamRespawn.SetActive(true);
        }
        else {
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
}
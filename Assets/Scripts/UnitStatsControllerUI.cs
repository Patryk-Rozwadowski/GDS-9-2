using UnityEngine;
using UnityEngine.UI;


// TODO Of course there is much better way to do this
public class UnitStatsControllerUI : MonoBehaviour {
    [SerializeField] private GameObject _leftPanelDraftPick, _rightPanelDraftPick;

    [SerializeField] private GameObject _leftPanelGameMode, _rightPanelGameMode;

    [Header("DRAFT PANEL - Right team:")] [SerializeField]
    private Text
        unitNameRightDraft,
        hpRightDraft,
        attackRightDraft,
        attackRangeRightDraft,
        movementRangeRightDraft;

    [Header("DRAFT PANEL - Left team:")] [SerializeField]
    private Text
        unitNameLeftDraft,
        hpLeftDraft,
        attackLeftDraft,
        attackRangeLeftDraft,
        movementRangeLeftDraft;


    [Header("GAME PANEL - Right team:")] [SerializeField]
    private Text
        unitNameRightGame,
        hpRightGame,
        attackRightGame,
        attackRangeRightGame,
        movementRangeRightGame;

    [Header("GAME PANEL - Left team:")] [SerializeField]
    private Text
        unitNameLeftGame,
        hpLeftGameGame,
        attackLeftGame,
        attackRangeLeftGame,
        movementRangeLeftGame;

    public void ViewClickedUnitStatsDraftPick(
        UnitStatsSO unitStats,
        UnitCombatSystem.Team team
    ) {
        if (team == UnitCombatSystem.Team.Left) {
            unitNameLeftDraft.text = unitStats.unitName;
            hpLeftDraft.text = $"{unitStats.maxHealth}";
            attackLeftDraft.text = $"{unitStats.damage}";
            attackRangeLeftDraft.text = $"{unitStats.attackRange}";
            movementRangeLeftDraft.text = $"{unitStats.movementRange}";
        }
        else {
            unitNameRightDraft.text = unitStats.unitName;
            hpRightDraft.text = $"{unitStats.maxHealth}";
            attackRightDraft.text = $"{unitStats.damage}";
            attackRangeRightDraft.text = $"{unitStats.attackRange}";
            movementRangeRightDraft.text = $"{unitStats.movementRange}";
        }
    }

    public void ViewActiveUnitInGame(
        UnitStatsSO unitStats,
        UnitCombatSystem.Team team
    ) {
        if (team == UnitCombatSystem.Team.Left) {
            unitNameLeftGame.text = unitStats.unitName;
            hpLeftGameGame.text = $"{unitStats.maxHealth}";
            attackLeftGame.text = $"{unitStats.damage}";
            attackRangeLeftGame.text = $"{unitStats.attackRange}";
            movementRangeLeftGame.text = $"{unitStats.movementRange}";
        }
        else {
            unitNameRightGame.text = unitStats.unitName;
            hpRightGame.text = $"{unitStats.maxHealth}";
            attackRightGame.text = $"{unitStats.damage}";
            attackRangeRightGame.text = $"{unitStats.attackRange}";
            movementRangeRightGame.text = $"{unitStats.movementRange}";
        }
    }

    public void HidePanelPlayerPanel(UnitCombatSystem.Team activeTeam, GameModeEnum gameMode) {
        if (activeTeam == UnitCombatSystem.Team.Left) {
            if (gameMode == GameModeEnum.Game) {
                _rightPanelGameMode.SetActive(false);
                _leftPanelGameMode.SetActive(true);
                return;
            }

            if (gameMode == GameModeEnum.DraftPick) {
                _rightPanelDraftPick.SetActive(false);
                _leftPanelDraftPick.SetActive(true);
            }
        }
        else {
            if (gameMode == GameModeEnum.Game) {
                _rightPanelGameMode.SetActive(true);
                _leftPanelGameMode.SetActive(false);
                return;
            }

            if (gameMode == GameModeEnum.DraftPick) {
                _rightPanelDraftPick.SetActive(true);
                _leftPanelDraftPick.SetActive(false);
            }
        }
    }

    public void HideGamePanels() {
        _rightPanelGameMode.SetActive(false);
        _leftPanelGameMode.SetActive(false);
    }

    public void HideDraftPickPanels() {
        _leftPanelDraftPick.SetActive(false);
        _rightPanelDraftPick.SetActive(false);
    }
}

public enum GameModeEnum {
    DraftPick,
    Game
}
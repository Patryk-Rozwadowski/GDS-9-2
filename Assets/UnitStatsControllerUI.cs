using UnityEngine;
using UnityEngine.UI;

public class UnitStatsControllerUI : MonoBehaviour {
    [SerializeField]
    private GameObject _leftPanelDraftPick, _rightPanelDraftPick;

    [SerializeField] private GameObject _leftPanelGameMode, _rightPanelGameMode;
    
    [Header("Right team:")] [SerializeField]
    private Text
        unitNameRight,
        hpRight,
        attackRight,
        attackRangeRight,
        movementRangeRight,
        passiveAbilityRight;

    [Header("Left team:")] [SerializeField]
    private Text
        unitNameLeft,
        hpLeft,
        attackLeft,
        attackRangeLeft,
        movementRangeLeft,
        passiveAbilityLeft;

    public void ViewClickedUnitStatsDraftPick(
        UnitStatsSO unitStats,
        UnitCombatSystem.Team team
    ) {
        if (team == UnitCombatSystem.Team.Left) {
            unitNameLeft.text = unitStats.unitName;
            hpLeft.text = $"{unitStats.maxHealth}";
            attackLeft.text = $"{unitStats.damage}";
            attackRangeLeft.text = $"{unitStats.attackRange}";
            movementRangeLeft.text = $"{unitStats.movementRange}";
        }
        else {
            unitNameRight.text = unitStats.unitName;
            hpRight.text = $"{unitStats.maxHealth}";
            attackRight.text = $"{unitStats.damage}";
            attackRangeRight.text = $"{unitStats.attackRange}";
            movementRangeRight.text = $"{unitStats.movementRange}";
        }
    }

    public void HidePanelPlayerPanel(UnitCombatSystem.Team team, GameModeEnum gameMode) {
        if (team == UnitCombatSystem.Team.Left) {
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
                _leftPanelDraftPick.SetActive(false);
                _rightPanelDraftPick.SetActive(true);
                return;
            }

            if (gameMode == GameModeEnum.DraftPick) {
                _leftPanelDraftPick.SetActive(false);
                _rightPanelDraftPick.SetActive(true);
            }
        }
    }

};

public enum GameModeEnum {
    DraftPick,
    Game
}
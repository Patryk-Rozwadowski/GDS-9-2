using UnityEngine;
using UnityEngine.UI;

public class UnitStatsControllerUI : MonoBehaviour {
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

    public void ViewUnitStats(
        UnitStatsSO unitStats,
        DraftPickController.Team team
    ) {
        if (team == DraftPickController.Team.Left) {
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
}
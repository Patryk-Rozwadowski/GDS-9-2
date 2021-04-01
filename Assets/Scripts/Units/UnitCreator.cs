using UnityEngine;

public class UnitCreator : MonoBehaviour {
    [Header("Scriptable object with all required parameters and stats for unit")] [SerializeField]
    public UnitStatsSO unitScriptableObject;

    private MovePositionPathfinding _movePositionPathfinding;
    private MoveTransformVelocity _moveTransformVelocity;
    private UnitCombatSystem _unitCombatSystem;
    private GameObject _unitSpriteGameObject;

    private void Start() {
    }

    public UnitStatsSO GetUnitStats() {
        return unitScriptableObject;
    }
}
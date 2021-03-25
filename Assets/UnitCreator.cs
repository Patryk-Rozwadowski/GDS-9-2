using UnityEditor.SceneManagement;
using UnityEngine;

[RequireComponent(
    typeof(Rigidbody2D),
    typeof(SpriteRenderer)
)]
[RequireComponent(
    typeof(MoveTransformVelocity),
    typeof(MovePositionPathfinding)
)]
public class UnitCreator : MonoBehaviour {
    [Header("Scriptable object with all required parameters and stats for unit")] [SerializeField]
    public UnitStatsSO unitScriptableObject;

    private MovePositionPathfinding _movePositionPathfinding;
    private MoveTransformVelocity _moveTransformVelocity;
    private UnitCombatSystem _unitCombatSystem;
    private GameObject _unitSpriteGameObject;

    void Start() {
        gameObject.AddComponent<UnitCombatSystem>();
        
        GetComponent<SpriteRenderer>().sprite = unitScriptableObject.sprite;
    }

    public UnitStatsSO GetUnitStats() {
        return unitScriptableObject;
    }
}
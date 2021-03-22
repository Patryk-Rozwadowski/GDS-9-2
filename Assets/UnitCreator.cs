using UnityEngine;
[RequireComponent(
    typeof(Rigidbody2D),
    typeof(SpriteRenderer)
)]
[RequireComponent(
    typeof(UnitCombatSystem),
    typeof(MoveTransformVelocity),
    typeof(MovePositionPathfinding)
)]
public class UnitCreator : MonoBehaviour {
    [Header("Scriptable object with all required parameters and stats for unit")] [SerializeField]
    private UnitStatsSO unitScriptableObject;

    private Rigidbody2D _rigidbody2D;
    private MovePositionPathfinding _movePositionPathfinding;
    private MoveTransformVelocity _moveTransformVelocity;
    private UnitCombatSystem _unitCombatSystem;

    private GameObject _unitSpriteGameObject;
    
    void Start() {
        _rigidbody2D = GetComponent<Rigidbody2D>();
        // _rigidbody2D.simulated = false;

        _unitSpriteGameObject = gameObject.transform.GetChild(0).gameObject;
        _unitSpriteGameObject.AddComponent<SpriteRenderer>();
        _unitSpriteGameObject.GetComponent<SpriteRenderer>().sprite = unitScriptableObject.sprite;
    }
}
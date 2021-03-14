using System;
using System.Collections.Generic;
using UnityEngine;



public class UnitCombatSystem : MonoBehaviour {
    [SerializeField] private Team team;
    public enum Team {
        Left,
        Right
    }
    
    private MovePositionPathfinding _movePositionPathfinding;
    private State _state;
    private SpriteRenderer _spriteRenderer;
    private IsActive _isUnitActive;
    
    private enum State {
        Normal,
        Moving,
        Attacking
    }

    public void SetActive() {
        _isUnitActive.SetUnitActive(true);
    }

    public void SetInactive() {
        _isUnitActive.SetUnitActive(false);
    }
    
    private void Awake() {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _movePositionPathfinding = GetComponent<MovePositionPathfinding>();
        _isUnitActive = GetComponentInChildren<IsActive>();
        _state = State.Normal;
    }

    private void Update() {
        switch (_state) {
            case State.Normal:
                break;
            case State.Moving:
                break;
            case State.Attacking:
                break;
        }
    }
    
    public void AttackUnit(UnitCombatSystem unitGridCombat, Action onAttackComplete) {
        _state = State.Attacking;
        Debug.Log($"Attack unit");
        onAttackComplete();
    }

    public Team GetTeam() {
        return team;
    }

    public Vector3 GetPosition() => transform.position;
    public void MoveTo(Vector3 targetPosition, Action onReachedPosition) {
        _state = State.Moving;

        // PATHFINDING
        _movePositionPathfinding.SetMovePosition(targetPosition + new Vector3(1, 1), () => {
            _state = State.Normal;
            onReachedPosition();
        });
    }
    
    public bool IsEnemy(UnitCombatSystem unitGridCombat) {
        return unitGridCombat.GetTeam() != team;
    }
    
    public bool CanAttackUnit(UnitCombatSystem unitGridCombat) {
        return Vector3.Distance(GetPosition(), unitGridCombat.GetPosition()) < 50f;
    }

}
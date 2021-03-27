using System;
using UnityEngine;

public class UnitCombatSystem : MonoBehaviour {
    [SerializeField] private Team team;
    [SerializeField] private UnitStatsSO _unitStats;
    private HealthSystem _healthSystem;

    private int
        _hp,
        _damage,
        _attackRange,
        _movementRange,
        _passiveAbility;

    public enum Team {
        Left,
        Right
    }

    private void Start() {
        _unitStats = GetComponent<UnitCreator>().GetUnitStats();
    }

    private MovePositionPathfinding _movePositionPathfinding;
    private State _state;
    private IsActive _isUnitActive;
    private HealthBar _healthbar;

    private enum State {
        Normal,
        Moving,
        Attacking
    }

    public UnitStatsSO GetUnitStats() {
        return _unitStats;
    }
    
    public void SetActive() {
        // TODO active sprite
        // _isUnitActive.SetUnitActive(true);
    }

    public void SetInactive() {
        // _isUnitActive.SetUnitActive(false);
    }

    private void Awake() {
        // TODO change damage
        _movePositionPathfinding = GetComponent<MovePositionPathfinding>();
        _isUnitActive = GetComponent<IsActive>();
        _healthbar = GetComponentInChildren<HealthBar>();
        _state = State.Normal;
        Debug.Log(gameObject.name);

        if (_unitStats == null) return;
        _healthSystem = new HealthSystem(_unitStats.maxHealth);
        _healthbar.Init(_healthSystem);
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
        Debug.Log($"Attack unit {unitGridCombat.name}");
        unitGridCombat._healthSystem.Damage(_damage);
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
using System;
using System.Globalization;
using UnityEngine;

public class UnitCombatSystem : MonoBehaviour {
    [SerializeField] private Team team;
    [SerializeField] public UnitStatsSO unitStats;
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

    private MovePositionPathfinding _movePositionPathfinding;
    private State _state;
    public bool _isUnitActive;
    private HealthBar _healthbar;
    private SpriteRenderer _sr;

    private enum State {
        Normal,
        Moving,
        Attacking
    }

    public UnitStatsSO GetUnitStats() {
        return unitStats;
    }

    public void SetActive() {
        // TODO active sprite
        _isUnitActive = true;
    }

    public void SetInactive() {
        _isUnitActive = false;
    }


    private void Awake() {
        _healthbar = GetComponentInChildren<HealthBar>();
        _state = State.Normal;
        _isUnitActive = false;
        if (unitStats == null) return;
        _healthSystem = new HealthSystem(unitStats.maxHealth);
        _healthbar.Init(_healthSystem);
        _sr = GetComponent<SpriteRenderer>();
        _sr.sprite = unitStats.sprite;
    }

    private void Start() {
        _movePositionPathfinding = GetComponent<MovePositionPathfinding>();
        Debug.Log($"MOVE POSITION PATH FINDING {_movePositionPathfinding}");
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

        if (_isUnitActive) {
            _sr.sprite = unitStats.SelectedSprite;
        }
        else _sr.sprite = unitStats.sprite;
    }

    public void AttackUnit(UnitCombatSystem unitCombatSystem, Action onAttackComplete) {
        _state = State.Attacking;
        AttackWithTagDamage(unitCombatSystem);
        unitCombatSystem._healthSystem.Damage(unitStats.damage);
        Debug.Log(
            $"Attack unit {unitCombatSystem.name}, normal damage: {unitStats.damage}, tag damage: none, overall dmg: {unitStats.damage}");
        onAttackComplete();
    }

    private void AttackWithTagDamage(UnitCombatSystem unitCombatSystem) {
        var attackedUnitStats = unitCombatSystem.GetUnitStats();
        var attackedUnitName = attackedUnitStats.unitName;
        if(attackedUnitStats.ability == AbilitiesEnum.Counter) _healthSystem.Damage(attackedUnitStats.counterDamage);
        
        foreach (var attackingTag in unitStats.attackTags) {
            if (attackingTag.tagName != attackedUnitName) return;
            unitCombatSystem._healthSystem.Damage(unitStats.damage + attackingTag.tagDamage);
            Debug.Log(
                $"Attack unit {unitCombatSystem.name}, normal damage: {unitStats.damage}, tag damage: {attackingTag.tagDamage}, overall dmg: {unitStats.damage +attackingTag.tagDamage}");
            return;
        }
    }
    
    public Team GetTeam() {
        return team;
    }

    public Vector3 GetPosition() => transform.position;

    public void MoveTo(Vector3 targetPosition, Action onReachedPosition) {
        _state = State.Moving;
        // PATHFINDING
        _movePositionPathfinding.SetMovePosition(targetPosition + new Vector3(1, 1), () => { _state = State.Normal; });

        onReachedPosition();
    }

    public bool IsEnemy(UnitCombatSystem unitGridCombat) {
        return unitGridCombat.GetTeam() != team;
    }

    public bool CanAttackUnit(UnitCombatSystem unitGridCombat) {
        return Vector3.Distance(GetPosition(), unitGridCombat.GetPosition()) < 50f;
    }
}
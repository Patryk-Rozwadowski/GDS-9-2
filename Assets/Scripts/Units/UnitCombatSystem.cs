﻿using System;
using UnityEngine;
using Object = UnityEngine.Object;

public class UnitCombatSystem : MonoBehaviour {
    public enum Team {
        Left,
        Right
    }

    [SerializeField] private Team team;
    [SerializeField] public UnitStatsSO unitStats;
    
    public bool _isUnitActive;
    private HealthBar _healthbar;
    private HealthSystem _healthSystem;
    private GridCombatSystem _gridCombatSystem;

    private int
        _hp,
        _damage,
        _attackRange,
        _movementRange,
        _passiveAbility;

    private MovePositionPathfinding _movePositionPathfinding;
    private SpriteRenderer _sr;
    private State _state;
    private Grid<GridCombatSystem.GridObject> _grid;

    private void Awake() {
        _healthbar = GetComponentInChildren<HealthBar>();
        _gridCombatSystem = GameObject.Find("GridCombatSystem").GetComponent<GridCombatSystem>();
        _state = State.Normal;
        _isUnitActive = false;
        if (unitStats == null) return;
        _healthSystem = new HealthSystem(unitStats.maxHealth);
        _healthbar.Init(_healthSystem);
        _sr = GetComponent<SpriteRenderer>();
        _sr.sprite = unitStats.sprite;
        _grid = GameController_GridCombatSystem.Instance.GetGrid();
    }

    private void Start() {
        _movePositionPathfinding = GetComponent<MovePositionPathfinding>();
        Debug.Log($"MOVE POSITION PATH FINDING {_movePositionPathfinding}");
        OnActiveChanged += ActiveUnitChanged;
    }

    public event EventHandler OnActiveChanged;

    private void ActiveUnitChanged(object sender, EventArgs e) {
        if (_isUnitActive)
            _sr.sprite = unitStats.SelectedSprite;
        else _sr.sprite = unitStats.sprite;
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

    public UnitStatsSO GetUnitStats() {
        return unitStats;
    }

    public void SetActive() {
        _isUnitActive = true;
        OnActiveChanged?.Invoke(this, EventArgs.Empty);
    }

    public void SetInactive() {
        _isUnitActive = false;
        OnActiveChanged?.Invoke(this, EventArgs.Empty);
    }

    public void AttackUnit(UnitCombatSystem unitCombatSystem, Action onAttackComplete) {
        _state = State.Attacking;

        AttackWithAdditionalDamage(unitCombatSystem);
        unitCombatSystem._healthSystem.Damage(unitStats.damage);
        if (unitCombatSystem.IsDead()) {
            var grid = GameController_GridCombatSystem.Instance.GetGrid();
            grid.SetGridObject(transform.position, null);
        }
        Debug.Log(
            $"Attack unit {unitCombatSystem.name}, normal damage: {unitStats.damage}, tag damage: none, overall dmg: {unitStats.damage}");
        onAttackComplete();
    }

    public bool IsDead() {
        return _healthSystem.GetHealth() <= 0;
    }
    
    private void AttackWithAdditionalDamage(UnitCombatSystem unitCombatSystem) {
        var attackedUnitStats = unitCombatSystem.GetUnitStats();
        var attackedUnitName = attackedUnitStats.unitName;

        var attackedCounter = attackedUnitStats.counterType;

        if (attackedUnitStats.ability == AbilitiesEnum.Counter)
            if (attackedCounter == unitStats.unitType)
                _healthSystem.Damage(attackedUnitStats.counterDamage);
        foreach (var attackingTag in unitStats.attackTags) {
            if (attackingTag.tagName != attackedUnitName) return;
            unitCombatSystem._healthSystem.Damage(unitStats.damage + attackingTag.tagDamage);
            Debug.Log(
                $"Attack unit {unitCombatSystem.name}, normal damage: {unitStats.damage}, tag damage: {attackingTag.tagDamage}, overall dmg: {unitStats.damage + attackingTag.tagDamage}");
            return;
        }
    }

    public Team GetTeam() {
        return team;
    }

    public Vector3 GetPosition() {
        
        return transform.position;
    }

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

    public bool CanMeleeAttack(UnitCombatSystem unitGridCombat) {
        // Number depends on grid tile size
        return Vector3.Distance(GetPosition(), unitGridCombat.GetPosition()) < 18f;
    }

    public bool CanDistanceAttack(UnitCombatSystem unitGridCombat) {
        // Number depends on grid tile size
        Debug.Log(Vector3.Distance(GetPosition() / 2, unitGridCombat.GetPosition() / 2));
        Debug.Log($"Range {unitStats.attackRange * 18f / 2}");
        return Vector3.Distance(GetPosition() / 2f, unitGridCombat.GetPosition() / 2f) <
               (unitStats.attackRange) * 18f / 2;
    }

    private enum State {
        Normal,
        Moving,
        Attacking
    }
}
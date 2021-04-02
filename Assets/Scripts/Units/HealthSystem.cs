using System;
using UnityEngine;

public class HealthSystem {
    private float _health;
    private readonly float _maxHealth;
    public float MaxHealth { get => _maxHealth; }

    public HealthSystem(int maxHealth) {
        _maxHealth = maxHealth;
        _health = _maxHealth;
    }

    public event EventHandler OnHealthChanged;

    public float GetHealth() {
        return _health;
    }

    public void Damage(int damageAmount) {
        _health -= damageAmount;
        if (_health <= 0) {
            Debug.Log("Unit dead");
            _health = 0;
        }

        OnHealthChanged?.Invoke(this, EventArgs.Empty);
    }

    public float GetHealthPercent() {
        return _health / _maxHealth;
    }
}
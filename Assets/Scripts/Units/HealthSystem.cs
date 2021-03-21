using System;
using UnityEngine;

public class HealthSystem {
    public event EventHandler OnHealthChanged;
    private float _health, _maxHealth;

    public HealthSystem(int maxHealth) {
        _maxHealth = maxHealth;
        _health = _maxHealth;
    }

    public float GetHealth() => _health;

    public void Damage(int damageAmount) {
        _health -= damageAmount;
        if (_health <= 0) {
            Debug.Log("Unit dead");
            _health = 0;
        }

        OnHealthChanged?.Invoke(this, EventArgs.Empty);
    }

    public float GetHealthPercent() => _health / _maxHealth;
}
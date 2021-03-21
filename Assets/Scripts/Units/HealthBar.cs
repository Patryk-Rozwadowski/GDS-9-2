using System;
using UnityEngine;

public class HealthBar : MonoBehaviour {
    private HealthSystem _healthSystem;

    public void Init(HealthSystem healthSystem) {
        _healthSystem = healthSystem;
        _healthSystem.OnHealthChanged += HealthSystemOnOnHealthChanged;
    }

    private void HealthSystemOnOnHealthChanged(object sender, EventArgs e) {
        var bar = transform.Find("Bar");
        bar.localScale = new Vector3(_healthSystem.GetHealthPercent(), 1f);
        Debug.Log($"{gameObject.transform.parent.gameObject} hp: {_healthSystem.GetHealth()}");
    }
}
using System;
using UnityEngine;
using TMPro;

public class HealthBar : MonoBehaviour {
    private HealthSystem _healthSystem;
    [SerializeField]
    private TextMeshProUGUI healthValueInfo;

    public void Init(HealthSystem healthSystem) {
        _healthSystem = healthSystem;
        _healthSystem.OnHealthChanged += HealthSystemOnOnHealthChanged;
        healthValueInfo.text = _healthSystem.MaxHealth.ToString() + "/" + _healthSystem.MaxHealth.ToString();
    }

    private void HealthSystemOnOnHealthChanged(object sender, EventArgs e) {
        var bar = gameObject.transform.Find("Bar");
        bar.localScale = new Vector3(_healthSystem.GetHealthPercent(), 1f);
        healthValueInfo.text = (_healthSystem.MaxHealth * _healthSystem.GetHealthPercent()).ToString() + "/" + _healthSystem.MaxHealth.ToString();
        Debug.Log($"{gameObject.transform.parent.gameObject} hp: {_healthSystem.GetHealth()}");
        if (_healthSystem.GetHealthPercent() == 0) {
            var unit = transform.parent.gameObject;
            transform.parent = null;
            Destroy(unit);
            Destroy(gameObject);
        }
    }
}
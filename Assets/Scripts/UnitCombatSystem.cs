using System.Collections.Generic;
using UnityEngine;

public enum Team {
    Left,
    Right
}

public class UnitCombatSystem : MonoBehaviour {
    [SerializeField] private Team team;

    // Update is called once per frame
    void Update() {
    }

    public Team GetTeam() {
        return team;
    }
}
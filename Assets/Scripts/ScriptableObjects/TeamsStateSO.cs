using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Teams state")]
public class TeamsStateSO : ScriptableObject {
    [Header("All units in both teams.")] [SerializeField]
    public List<UnitCombatSystem> allUnitsInBothTeams;

    [Header("Left player's team")] [SerializeField]
    public List<UnitCombatSystem> leftTeam;

    [Header("Right player's team")] [SerializeField]
    public List<UnitCombatSystem> rightTeam;

    [Header("When both teams are ready")] [SerializeField]
    public bool areTeamsReady;
}
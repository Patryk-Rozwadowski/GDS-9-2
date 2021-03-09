using System;
using System.Collections.Generic;
using UnityEngine;

public enum Team {
    Left,
    Right
}

public class UnitCombatSystem : MonoBehaviour {
    [SerializeField] private Team team;

    private MovePositionPathfinding _movePositionPathfinding;
    private State _state;

    private enum State {
        Normal,
        Moving,
        Attacking
    }

    private void Start() {
        _movePositionPathfinding = GetComponent<MovePositionPathfinding>();
        _state = State.Normal
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

    public Team GetTeam() {
        return team;
    }

    public void MoveTo(Vector3 targetPosition, Action onReachedPosition) {
        _state = State.Moving;

        // PATHFINDING
        _movePositionPathfinding.SetMovePosition(targetPosition + new Vector3(1, 1), () => {
            _state = State.Normal;
            onReachedPosition();
        });
    }
}
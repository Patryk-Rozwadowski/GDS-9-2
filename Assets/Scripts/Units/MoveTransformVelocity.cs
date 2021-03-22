using UnityEngine;

public class MoveTransformVelocity : MonoBehaviour, IMoveVelocity {
    [SerializeField] private float moveSpeed = 15;

    private Vector3 _velocityVector;
    private Rigidbody2D _rigidbody2D;

    private void Awake() {
        _rigidbody2D = GetComponent<Rigidbody2D>();
    }

    public void SetVelocity(Vector3 velocityVector) {
        _velocityVector = velocityVector;
    }
    
    private void FixedUpdate() {
        _rigidbody2D.velocity = _velocityVector * moveSpeed;
    }

    public void Disable() {
        enabled = false;
        _rigidbody2D.velocity = Vector3.zero;
    }

    public void Enable() {
        enabled = true;
    }
}
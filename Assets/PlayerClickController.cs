using UnityEngine;

public class PlayerClickController : MonoBehaviour {
    [SerializeField] private float moveSpeed = 10f;
    private Vector3 _velocityVector, _movePosition;
    private Rigidbody2D _rigidbody2D;

    private void Start() {
        _rigidbody2D = GetComponent<Rigidbody2D>();
    }

    private void SetVelocity(Vector3 velocityVector) {
        _velocityVector = velocityVector;
    }

    public void SetMovePosition(Vector3 movePosition) {
        _movePosition = movePosition;
    }

    private void Update() {
        if (Input.GetMouseButtonDown(0)) {
            Debug.Log("Click");
            
            var mousePosition = Input.mousePosition;
            var worldPosition = Camera.main.ScreenToWorldPoint(mousePosition);
            transform.position = worldPosition;
            
            // SetMovePosition(worldCamera);
            //
            // var moveDir = (_movePosition - transform.position).normalized;
            // Debug.Log(moveDir);
            // if (Vector3.Distance(_movePosition, transform.position) < 1f) moveDir = Vector3.zero;

        }
    }
}
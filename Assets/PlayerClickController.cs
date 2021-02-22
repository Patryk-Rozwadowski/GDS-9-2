using UnityEngine;

public class PlayerClickController : MonoBehaviour {
    [SerializeField] private float moveSpeed = 10f;
    private Rigidbody2D _rigidbody2D;

    private void Start() {
        _rigidbody2D = GetComponent<Rigidbody2D>();
    }

    private void Update() {
        if (Input.GetMouseButtonDown(0)) {
            Debug.Log("Click");
            
            var mousePosition = Input.mousePosition;
            var worldPosition = Camera.main.ScreenToWorldPoint(mousePosition);
            transform.position = worldPosition;
        }
    }
}
using UnityEngine;

namespace com.greghilston {
    public class MovementController : MonoBehaviour {
        [Tooltip("The speed of the character")]
        [SerializeField]
        private float speed;
        Vector3 moveTo;
        Rigidbody rb;
        private void Awake() {
            rb = GetComponent<Rigidbody>();
        }

        void CaptureInput() {
            Vector3 moveInput = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));
            moveTo = moveInput.normalized;
            Debug.DrawLine(transform.position, moveTo, Color.green);
        }

        void Update() {
            CaptureInput();
        }

        void ProcessMovement() {
            rb.MovePosition(transform.position + moveTo * speed * Time.deltaTime);
        }

        void FixedUpdate() {
            ProcessMovement();
        }
    }
}
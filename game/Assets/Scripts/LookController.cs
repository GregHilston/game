using UnityEngine;

namespace com.greghilston {
    public class LookController : MonoBehaviour {
        Camera camera;
        Vector3 lookAt;

        private void Awake() {
            this.camera = Camera.main;
        }

        void CaptureLookInput() {
            Ray ray = this.camera.ScreenPointToRay(Input.mousePosition);
            Plane groundPlane = new Plane(Vector3.up, Vector3.zero);
            float rayDistance;
            if (groundPlane.Raycast(ray, out rayDistance)) {
                lookAt = ray.GetPoint(rayDistance);
                Debug.DrawLine(ray.origin, lookAt, Color.red);
                Debug.DrawLine(transform.position, lookAt, Color.red);
            }
        }

        void Update() {
            CaptureLookInput();
        }

        void ProcessLook() {
            Vector3 heightCorrectedPoint = new Vector3(this.lookAt.x, transform.position.y, lookAt.z);
            transform.LookAt(heightCorrectedPoint);
        }

        void FixedUpdate() {
            ProcessLook();
        }
    }
}